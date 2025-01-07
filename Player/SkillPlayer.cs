using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class SkillPlayer : MonoBehaviour
{
    //Attack
    private Rigidbody2D rbBullet;
    public float speedBullet, areaDetec;
    public GameObject bullet, superBullet, specialBullet;
    private Vector2 bulletDirection;
    private bool isNear;
    public float timeShot;

    //Skill 1
    [SerializeField] private float teleportDistance;
    private Rigidbody2D rb;

    //Animation
    private Animator anim;

    private void Awake()
    {
        //Attack
        isNear = false;

        //Dash
        rb = GetComponent<Rigidbody2D>();

        //Animation
        anim = transform.Find("Interface").transform.GetComponent<Animator>();
    }

    public void NormalAttack() //Tấn công thường : Bắn đạn
    {
        //Animation
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("SkillThree", false);
        anim.SetBool("NormalAttack", true);

        GameObject bulletSpawn = Instantiate(bullet, this.transform.position, this.transform.rotation);
        rbBullet = bulletSpawn.GetComponent<Rigidbody2D>();
        StartCoroutine(SpawnBullet());
        StartCoroutine(SetAttackCooldown(bullet.name));
    }

    public void ExecuteSkill1() //Thực hiện kỹ năng 1 : Tốc biến
    {
        StartCoroutine(SkillOne());
    }

    public void ExecuteSkill2() //Thực hiện kỹ năng 2 : Bắn đạn lớn
    {
        SkillTwo();
    }

    public void ExecuteSkill3() //Thực hiện kỹ năng 3 : Bắn đạn hấp dẫn
    {
        SkillThree();
    }

    private IEnumerator SpawnBullet() //Điều chỉnh hướng đạn
    {
        Vector2 playerDirection = rb.velocity.normalized;
        if (isNear)
        {
            rbBullet.velocity = bulletDirection * speedBullet;
            Debug.Log("is near");
        }
        else if (!isNear && playerDirection != Vector2.zero)
        {
            rbBullet.velocity = playerDirection * speedBullet;
            Debug.Log("isn't near and joystick != 0");
        }
        else if (playerDirection == Vector2.zero)
        {
            Debug.Log("isn't near and joystick = 0");
        randomX:
            float x = Random.Range(-0.9f, 0.9f);
            if (x == 0) goto randomX;
            randomY:
            float y = Random.Range(-0.9f, 0.9f);
            if (y == 0) goto randomY;

            rbBullet.velocity = (new Vector2(x, y)).normalized * speedBullet;
        }

        yield return null;
    }

    private void SkillThree() //Kỹ năng 3 : Tạo một lớp lá chắn đẩy kẻ thù ra xa
    {
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("SkillThree", true);
        anim.SetBool("NormalAttack", false);

        GameObject bullet2 = Instantiate(specialBullet, this.transform.position, this.transform.rotation);
        rbBullet = bullet2.GetComponent<Rigidbody2D>();
        StartCoroutine(SpawnBullet());
    }

    private void SkillTwo() //Kỹ năng 2 : Bắn đạn ăn mòn
    {
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("SkillThree", false);
        anim.SetBool("NormalAttack", true);

        GameObject bullet2 = Instantiate(superBullet, this.transform.position, this.transform.rotation);
        rbBullet = bullet2.GetComponent<Rigidbody2D>();
        StartCoroutine(SpawnBullet());
        StartCoroutine(SetAttackCooldown(superBullet.name));
    }

    IEnumerator SkillOne() //Kỹ năng 1 : Tốc biến theo hướng Joystick
    {
        Vector2 directionMoving = rb.velocity.normalized;
        transform.position = (Vector2)transform.position + directionMoving * teleportDistance;

        yield return null;
    }

    private void OnTriggerStay2D(Collider2D collision) //Kiểm tra đối tượng trong phạm vi tấn công
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 playerPos = this.transform.position;
            Vector2 enemyPos = collision.transform.position;
            bulletDirection = GetDistance.GetDirection(playerPos, enemyPos);
            isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Kiểm tra kẻ thù ra khỏi phạm vi tấn công
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isNear = false;
        }
    }

    IEnumerator SetAttackCooldown(string nameOfBullet) //Thời gian giữa hai lần tấn công liên tiếp
    {
        GameObject bullet = GameObject.Find(nameOfBullet);
        if(bullet == null)
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
