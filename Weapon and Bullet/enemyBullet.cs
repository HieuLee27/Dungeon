using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : Bullet
{
    private GameObject target;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        DirectionBullet(target.transform.position);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameObject.Find("Player/Interface"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Bouder"))
        {
            Destroy(gameObject);
        }
    }
}
