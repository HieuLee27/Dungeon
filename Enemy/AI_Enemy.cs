using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UIElements;

public class AI_Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;
    protected bool isCollision = false;

    [SerializeField] private float areaDetect, speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            FollowPlayer(player.transform.position);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) //Kiểm tra va chạm với Player
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollision = true;
        }
    }

    private void FollowPlayer(Vector2 playerPos) //Kẻ thù đuổi theo player khi khoảng cách giữa chúng nằm trong phạm vi có thể phát hiện
    {
        float distance = GetDistance.GetDistanceBetween(playerPos, this.gameObject.transform.position);

        if (distance <= areaDetect)
        {
            Vector2 directionMoving = GetDistance.GetDirection(this.gameObject.transform.position, player.transform.position);
            rb.velocity = directionMoving * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
