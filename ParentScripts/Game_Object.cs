using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game_Object : MonoBehaviour
{
    //Chuyển động
    protected Rigidbody2D rb;
    protected Vector2 directionMoving;
    protected SpriteRenderer sprite;
    protected Animator anim;

    //Chỉ số nhân vật
    [SerializeField] protected float speedMoving;
    [SerializeField] protected float health;
    [SerializeField] protected float defend;
    [SerializeField] protected float physicalAttack;
    [SerializeField] protected float magicalAttack;
    internal bool isLive;
    [SerializeField] protected ElementalType type;

    //Đối tượng gây sát thương
    [SerializeField] protected GameObject enemy;
    [SerializeField] protected GameObject enemyBullet;
    protected bool isEnemyInRange;

    //Giảm máu
    protected float damage;
    protected Transform blood;
    protected Transform bloodDefault;
    protected Vector3 currentBlood;


    //Hình ảnh nhân vật
    private GameObject playerInterface;

    protected virtual void Awake()
    {
        blood = transform.Find("Health/Blood").transform;
        rb = GetComponent<Rigidbody2D>();
        playerInterface = transform.Find("Interface").gameObject;
        sprite = playerInterface.GetComponent<SpriteRenderer>();
        anim = playerInterface.GetComponent<Animator>();
        bloodDefault = blood;
        isLive = true;
    }

    protected enum ElementalType
    {
        physical,
        magical,
        both
    }

    protected virtual void OnCollisionStay2D(Collision2D collision) //Nhận sát thương và giảm lượng máu
    {
        if (collision.gameObject.CompareTag(enemy.tag))
        {
            string enemyTag = enemy.tag; //Xác định loại kẻ thù
            if (enemyTag == "Enemy")
            {
                damage = collision.gameObject.GetComponent<AI_Enemy>().physicalAttack + collision.gameObject.GetComponent<AI_Enemy>().magicalAttack;
                DecreaseBlood();
            }
            else if(enemyTag == "Player")
            {
                damage = 0;
            }
        }
        else if (collision.gameObject.CompareTag("Boss") && gameObject.tag == "Player")
        {
            damage = collision.gameObject.GetComponent<AI_Boss1>().physicalAttack + collision.gameObject.GetComponent<AI_Boss1>().magicalAttack;
            DecreaseBlood();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(enemyBullet.tag))
        {
            damage = collision.gameObject.GetComponent<Bullet>().physicalDamage + collision.gameObject.GetComponent<Bullet>().magicalDamage;
            DecreaseBlood();
        }
        if (collision.gameObject.CompareTag("Healing"))
        {
            IncreaseBlood();
        }
    }

    protected void DecreaseBlood()
    {
        if (blood.localScale.x <= 0.05f)
        {
            damage = 0;
            isLive = false;
        }

        Vector3 sizeBlood = blood.localScale; //Kích thước hiện tại của thanh máu
        sizeBlood.x -= (damage * blood.localScale.x) / (health + defend);
        blood.localScale = sizeBlood; //Thay đổi kích thước thanh máu

        Vector3 defaultPos = blood.localPosition; //Vị trí ban đầu của thanh máu
        defaultPos.x -= (damage * blood.localScale.x) / ((health + defend) * 2); //Dịch chuyển dần vị trí sang trái với lượng máu bị giảm tương ứng
        blood.localPosition = defaultPos; //Thay đổi vị trí thanh máu
    }

    protected void IncreaseBlood()
    {
        currentBlood = blood.localScale; //Kích thước hiện tại của thanh máu

        if(currentBlood.x <= blood.localScale.x) //Nếu thanh máu ngắn hơn chiều dài tối đa của thanh máu mặc định thì mới có thể tiếp tục tăng chiều dài khi hồi máu
        {
            currentBlood.x += (20 * blood.localScale.x) / (health + defend);
            blood.localScale = currentBlood; //Thay đổi kích thước thanh máu
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(enemy.tag))
        {
            DecreaseBlood();
        }
    }
}
