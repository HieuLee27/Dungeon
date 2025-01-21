using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControllerPlayer : Game_Object
{
    //Di chuyển
    private GameObject joystickObject;
    private Joystick joystick;
    

    //Attack
    public GameObject bullet, superBullet, specialBullet;

    //Mục tiêu tấn công
    private GameObject target1;

    //Kỹ năng nhân vật
    [SerializeField] private float timeCooldownAttack;
    [SerializeField] private float speedRaising;

    //Cấp độ nhân vật
    [SerializeField] private Slider levelbar;
    [SerializeField] private TMP_Text textLevel;

    protected override void Awake()
    {
        base.Awake();
        joystickObject = GameObject.FindWithTag("Joystick");
        joystick = joystickObject.GetComponent<Joystick>();
    }

    private void LateUpdate()
    {
        Moving();
        FlipDirection();
    }

    private void Moving() //Di chuyển nhân vật theo Joystick
    {
        if(joystick.Direction.y != 0)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
            anim.SetBool("SkillThree", false);
            anim.SetBool("NormalAttack", false);
            rb.velocity = new Vector2(joystick.Direction.x * speedMoving, joystick.Direction.y * speedMoving);
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Idle", true);
            anim.SetBool("SkillThree", false);
            anim.SetBool("NormalAttack", false);
            rb.velocity = Vector2.zero;
        }
    }

    private void FlipDirection() //Điều khiển hướng xoay nhân vật
    {
        if (isEnemyInRange && target1 != null)
        {
            directionMoving = GetDistance.GetDirection(this.gameObject.transform.position, target1.transform.position);

            if (directionMoving.x > 0)
            {
                sprite.flipX = false;
            }
            else if (directionMoving.x < 0)
            {
                sprite.flipX = true;
            }
        }
        else
        {
            if (joystick.Direction.x > 0)
            {
                sprite.flipX = false;
            }
            else if (joystick.Direction.x < 0)
            {
                sprite.flipX = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Kẻ thù ra khỏi tầm nhìn
    {
        if (collision.gameObject.CompareTag(enemy.tag) || collision.gameObject.CompareTag("Boss"))
        {
            isEnemyInRange = false;
            target1 = null;
        }
    }

    public void ExecuteAttack() //Tấn công thường : Bắn đạn
    {
        //Animation
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("SkillThree", false);
        anim.SetBool("NormalAttack", true);

        StartCoroutine(NormalAttack(bullet));
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

    private IEnumerator NormalAttack(GameObject typeBullet)
    {
        Bullet bulletComponent = Instantiate(typeBullet, transform.position, transform.rotation).GetComponent<Bullet>();
        if (isEnemyInRange && target1 != null)
        {
            bulletComponent.GetComponent<Bullet>().DirectionBullet(target1.transform.position);
        }
        else
        {
            bulletComponent.GetComponent<Bullet>().RandomDirectionBullet();
        }
        yield return new WaitForSeconds(timeCooldownAttack);
    }

    private void SkillThree() //Kỹ năng 3 : Bắn đạn trọng trường
    {
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("SkillThree", true);
        anim.SetBool("NormalAttack", false);

        StartCoroutine(NormalAttack(specialBullet));
    }

    private void SkillTwo() //Kỹ năng 2 : Bắn đạn ăn mòn
    {
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        anim.SetBool("SkillThree", false);
        anim.SetBool("NormalAttack", true);

        StartCoroutine(NormalAttack(superBullet));
    }

    IEnumerator SkillOne() //Kỹ năng 1 : Lướt nhanh theo hướng Joystick
    {
        directionMoving = joystick.Direction;
        rb.velocity = directionMoving * speedMoving * speedRaising;

        yield return new WaitForSeconds(timeCooldownAttack);
    }

    private void OnTriggerStay2D(Collider2D collision) //Kiểm tra đối tượng trong phạm vi tấn công
    {
        if (collision.gameObject.CompareTag(enemy.tag) || collision.gameObject.CompareTag("Boss"))
        {
            isEnemyInRange = true;
            target1 = collision.gameObject;
        }
        else if (collision.gameObject == null)
        {
            isEnemyInRange = false;
            target1 = null;
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D (collision);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Tăng cấp nhân vật
        if (collision.gameObject.CompareTag("ManaFood"))
        {
            levelbar.value += collision.gameObject.GetComponent<Food>().amountMana / 100;
        }
        if (levelbar.value >= 1)
        {
            levelbar.value = 0;
            textLevel.text = (int.Parse(textLevel.text) + 1).ToString();
            blood = bloodDefault;
        }
    }
}
