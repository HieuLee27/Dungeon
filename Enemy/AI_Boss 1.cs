using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI_Boss1 : Game_Object
{
    protected float currentDistance; //Khoảng cách hiện tại giữa player và boss
    [SerializeField] protected float rangedAttack; //Khoảng cách tấn công xa
    private GameObject player;
    [SerializeField] private GameObject gate;
    private SpriteRenderer spriteRenderer;

    //Tấn công
    protected bool startAttack;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected GameObject magicBullet;
    public float shootingCooldown = 1f; // Thời gian giữa các lần bắn
    private float lastShootTime;    // Thời điểm lần bắn trước đó

    // Chỉ số
    protected angryLevel level = angryLevel.Slightly_annoyed;
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
        startAttack = false;
        lastShootTime = -shootingCooldown;
    }

    private void Update()
    {
        currentDistance = GetDistance.GetDistanceBetween(transform.position, player.transform.position);
        if (Time.time >= lastShootTime + shootingCooldown)
        {
            fartherAttack();
            lastShootTime = Time.time;
        }
        FollowPlayer();
    }
    protected void FollowPlayer()
    {
        directionMoving = GetDistance.GetDirection(transform.position, player.transform.position);
        rb.velocity = directionMoving * speedMoving * Time.deltaTime * 20;
    }

    protected void Death()
    {
        int countGate = GameObject.FindGameObjectsWithTag("Gate").Length;
        if (!isLive && countGate == 1)
        {
            Instantiate(gate, transform.position, Quaternion.identity);
            spriteRenderer.color = Color.cyan;
        }
    }

    public enum angryLevel
    {
        Slightly_annoyed,
        Extremely_angry,
        Absolutely_furious
    }

    protected void AngryLevel()
    {
        if (currentBlood.x < (blood.localScale.x / 2 ))
        {
            level = angryLevel.Extremely_angry;
        }
        if (currentBlood.x < (blood.localScale.x / 4 ))
        {
            level = angryLevel.Absolutely_furious;
        }
    }

    protected void ChangeProperties(angryLevel level)
    {
        if (level == angryLevel.Extremely_angry)
        {
            physicalAttack += 20;
            health += 300;
        }
        else if(level == angryLevel.Absolutely_furious)
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

    private void fartherAttack() //Tấn công xa
    {
        if(currentDistance < rangedAttack)
        {
            Instantiate(magicBullet, transform.position, Quaternion.identity);
        }
    }
}
