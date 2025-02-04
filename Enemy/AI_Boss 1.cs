using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI_Boss1 : Game_Object
{
    protected float currentDistance; //Khoảng cách hiện tại giữa player và boss
    [SerializeField] protected float rangedAttack; //Khoảng cách tấn công xa
    private GameObject player;

    //Tấn công
    protected bool startAttack;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected GameObject magicBullet;
    public float shootingCooldown = 1f; // Thời gian giữa các lần bắn
    private float lastShootTime;    // Thời điểm lần bắn trước đó

    // Chỉ số
    protected AngryLevel level = AngryLevel.Slightly_annoyed;

    private SpriteRenderer enemySprite;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
        startAttack = false;
        lastShootTime = -shootingCooldown;
    }

    private void Start()
    {
        enemySprite = transform.Find("Interface").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        currentDistance = GetDistance.GetDistanceBetween(transform.position, player.transform.position);
        if (Time.time >= lastShootTime + shootingCooldown)
        {
            FartherAttack();
            lastShootTime = Time.time;
        }
        FollowPlayer();

        FlipSprite();

        if(this.enabled)
        {
            Death();
        }
    }
    protected void FollowPlayer()
    {
        directionMoving = GetDistance.GetDirection(transform.position, player.transform.position);
        rb.velocity = directionMoving * speedMoving * Time.deltaTime * 20;
    }

    protected void Death()
    {
        if (!isLive)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            anim.SetTrigger("Die");
        }
    }

    public enum AngryLevel
    {
        Slightly_annoyed,
        Extremely_angry,
        Absolutely_furious
    }

    protected void AngryLevelChange()
    {
        if (currentBlood.x < (blood.localScale.x / 2 ))
        {
            level = AngryLevel.Extremely_angry;
        }
        if (currentBlood.x < (blood.localScale.x / 4 ))
        {
            level = AngryLevel.Absolutely_furious;
        }
    }

    protected void ChangeProperties(AngryLevel level)
    {
        if (level == AngryLevel.Extremely_angry)
        {
            physicalAttack += 20;
            health += 300;
        }
        else if(level == AngryLevel.Absolutely_furious)
        {
            physicalAttack += 100;
            health += 1000;
        }
    }

    IEnumerator NearlyAttack()
    {
        yield return new WaitForSeconds(1.5f);

        startAttack = true;
        if (startAttack)
        {
            physicalAttack = 100;
            magicalAttack = 10;
            startAttack = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(enemy.tag))
        {
            StartCoroutine(NearlyAttack());
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private void FartherAttack() //Tấn công xa
    {
        if(currentDistance < rangedAttack)
        {
            Instantiate(magicBullet, transform.position, Quaternion.identity);
        }
    }

    private void FlipSprite()
    {
        if (rb.velocity.x > 0)
        {
            enemySprite.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            enemySprite.flipX = true;
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Attack", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("Attack", false);
    }
}
