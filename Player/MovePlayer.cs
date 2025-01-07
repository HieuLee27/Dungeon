using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private GameObject joystickObject;
    private Joystick joystick;
    private Rigidbody2D rb;
    [SerializeField] private float speedMoving;
    private GameObject child;

    private GameObject enemy;
    public float areaDetect;

    private Vector2 direction;
    private SpriteRenderer sprite;
    public float bloodCountDecreased;

    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        child = transform.Find("Interface").gameObject;
        sprite = child.GetComponent<SpriteRenderer>();
        anim = child.GetComponent<Animator>();

        joystickObject = GameObject.FindWithTag("Joystick");
        joystick = joystickObject.GetComponent<Joystick>();
    }

    private void LateUpdate()
    {
        Moving();
        FlipDirection();
    }

    private void Moving() //Di chuyển nhân vật theo Joystick
    {
        if(joystick.Direction.y != 0)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
            anim.SetBool("SkillThree", false);
            anim.SetBool("NormalAttack", false);
            rb.velocity = new Vector2(joystick.Direction.x * speedMoving, joystick.Direction.y * speedMoving);
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Idle", true);
            anim.SetBool("SkillThree", false);
            anim.SetBool("NormalAttack", false);
            rb.velocity = Vector2.zero;
        }
    }

    private void FlipDirection() //Quay hướng nhìn của Player theo kẻ thù khi chúng nằm trong phạm vi phát hiện
    {
        enemy = GameObject.FindWithTag("Enemy");
        if (enemy != null)
        {
            float distance = GetDistance.GetDistanceBetween(enemy.transform.position, this.gameObject.transform.position);
            if (areaDetect >= distance)
            {
                direction = GetDistance.GetDirection(this.gameObject.transform.position, enemy.transform.position);

                if (direction.x > 0)
                {
                    sprite.flipX = false;
                }
                else if (direction.x < 0)
                {
                    sprite.flipX = true;
                }
            }
            else
            {
                if (joystick.Direction.x > 0)
                {
                    sprite.flipX = false;
                }
                else if (joystick.Direction.x < 0)
                {
                    sprite.flipX = true;
                }
            }
        }
        else
        {
            if (joystick.Direction.x > 0)
            {
                sprite.flipX = false;
            }
            else if (joystick.Direction.x < 0)
            {
                sprite.flipX = true;
            }
        }
    }
 
    //private void OnCollisionStay2D(Collision2D collision) //Giảm lượng máu của Player khi va chạm với kẻ thù
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Vector3 changedBlood = blood.localScale;
    //        changedBlood.x -= bloodCountDecreased * Time.deltaTime;
    //        blood.localScale = changedBlood;
    //    }
    //}

    public static void DestroyPlayer()
    {
        Destroy(GameObject.FindWithTag("Player"));
    }
}
