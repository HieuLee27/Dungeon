using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    private string tagOfTarget;

    private void Awake()
    {
        tagOfTarget = target.transform.tag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagOfTarget) || collision.gameObject.CompareTag("Bouder"))
        {
            Destroy(gameObject);
        }
    }
}
