using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    private bool isTakingDamage;
    private Transform blood;
    public GameObject bullet;
    private string tagBullet, tagEnemy;
    public GameObject enemy;

    private void Awake()
    {
        blood = transform.parent.Find("Health/Blood");
        tagBullet = bullet.tag;
        tagEnemy = enemy.tag;
    }

    private void LateUpdate()
    {
        GetDamage(damage);
    }

    private void GetDamage(float damage)
    {
        while(isTakingDamage == true && blood.localScale.x > 0)
        {
            Vector3 bloodCountDecreased = blood.transform.localScale;
            bloodCountDecreased.x -= damage * Time.deltaTime; 

            blood.localScale = bloodCountDecreased;
            isTakingDamage = false;
        }
        if(blood.localScale.x <= 0)
        {
            MovePlayer.DestroyPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagBullet) || collision.gameObject.CompareTag(tagEnemy))
        {
            isTakingDamage = true;
        }
    }
}
