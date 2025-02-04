using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class AI_Enemy : Game_Object
{
    private float distanceWithPlayer;
    [SerializeField] private float areaDetect;
    private bool isCompleted = true;
    [SerializeField] private GameObject manaFood;
    private GameObject target;

    private SpriteRenderer enemySprite;

    protected override void Awake()
    {
        base.Awake();
        isCompleted = true;
        target = GameObject.FindWithTag(enemy.tag);
    }

    private void Start()
    {
        enemySprite = transform.Find("Interface").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        distanceWithPlayer = GetDistance.GetDistanceBetween(target.transform.position, transform.position);
        StartCoroutine(Death());
        if (isLive && isCompleted)
        {
            FollowPlayer();
        }

        FlipSprite();
    }

    IEnumerator WalkAround()
    {
        if(rb.velocity == Vector2.zero) 
        {
            isCompleted = false;

            directionMoving = GetDistance.GetRandomDirection();
            rb.velocity = directionMoving * (speedMoving - 0.1f) * Time.deltaTime * 20;

            yield return new WaitForSeconds(Random.Range(2, 3));

            rb.velocity = Vector2.zero;

            isCompleted = true;
        }
    }

    private void FollowPlayer() //Kẻ thù đuổi theo player khi khoảng cách giữa chúng nằm trong phạm vi có thể phát hiện
    {
        if(distanceWithPlayer <= areaDetect)
        {
            directionMoving = GetDistance.GetDirection(transform.position, target.transform.position);
            rb.velocity = directionMoving * speedMoving * Time.deltaTime * 20;
        }
        else
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(WalkAround());
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D (collision);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D (collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Attack", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("Attack", false);
    }

    IEnumerator Death()
    {
        if (!isLive)
        {
            GetComponent<Collider2D>().isTrigger = true;
            anim.SetTrigger("Die");

            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag(enemy.tag))
        {
            isCompleted = true;
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
}
