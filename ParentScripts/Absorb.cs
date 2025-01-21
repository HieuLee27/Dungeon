using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] private float distanceWithPlayer;
    [SerializeField] protected GameObject player;
    [SerializeField] private float speed;
    public float amountMana;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void FollowPlayer()
    {
        float currentDistance = Vector2.Distance(transform.position, player.transform.position);
        if(currentDistance <= distanceWithPlayer)
        {
            rb.velocity = GetDistance.GetDirection(transform.position, player.transform.position) * speed * Time.deltaTime * 20;
        }
    }
}
