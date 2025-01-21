using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro.EditorUtilities;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UIElements;

public class AI_Enemy : Game_Object
{
    private float distanceWithPlayer;
    [SerializeField] private float areaDetect;
    private bool isCompleted = true;
    [SerializeField] private GameObject manaFood;
    private GameObject target;

    protected override void Awake()
    {
        base.Awake();
        isCompleted = true;
        target = GameObject.FindWithTag(enemy.tag);
    }

    private void Update()
    {
        distanceWithPlayer = GetDistance.GetDistanceBetween(target.transform.position, transform.position);
        Death();
        if (isLive && isCompleted)
        {
            FollowPlayer();
        }
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
    }

    private void Death()
    {
        if (!isLive)
        {
            GetComponent<Collider2D>().isTrigger = true;
            Instantiate(manaFood, transform.position, transform.rotation);
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
}
