using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Chỉ số sát thương
    public float physicalDamage;
    public float magicalDamage;
    [SerializeField] protected int speedBullet;
    internal Vector2 direction;
    [SerializeField] protected GameObject enemy;

    //Thành phần vật lý
    protected Rigidbody2D rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Điều khiển hướng đạn bắn
    public void DirectionBullet(Vector2 target)
    {
        direction = GetDistance.GetDirection(transform.position, target).normalized;
        rb.velocity = direction * speedBullet;
    }

    //Đạn biến mất khi va chạm với kẻ thù
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(enemy.tag) || collision.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Bouder"))
        {
            Destroy(gameObject);
        }
    }

    public void RandomDirectionBullet()
    {
        direction = GetDistance.GetRandomDirection();
        rb.velocity = direction * speedBullet;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag(enemy.tag) || collision.gameObject.CompareTag("Bouder"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
