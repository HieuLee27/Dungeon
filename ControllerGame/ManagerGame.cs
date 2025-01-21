using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Linq;

public class ManagerGame : MonoBehaviour
{
    //Bản đồ game
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tileBaseGrid;
    [SerializeField] private TileBase tileBaseWall;

    //Đối tượng game
    private GameObject player;
    private GameObject boss;
    private List<GameObject> objList;

    //Điều kiện
    private GameObject[] enemyCounter;
    internal ModeGame mode;

    //Trạng thái game
    private StateGame state;
    private ResultGame result;

    private float timeCooldown = 2f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
        List<GameObject> objList = new();
        mode = ModeGame.FightingWithEnemy;
        state = StateGame.Playing;
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        CreateList(out objList, enemyCounter);
        enemyCounter = GameObject.FindGameObjectsWithTag("Enemy"); //Đếm số lượng enemy
        EnableScripts();
        if (Time.time >= timeCooldown)
        {
            Debug.LogFormat("Enemy: {0}", enemyCounter.Length);
            Debug.LogFormat("ListObject = {0}", objList.Count);
            Debug.LogFormat("Mode : {0}", mode);
            timeCooldown = Time.time + 2f;
        }
        ChangeMode();
    }

    public enum StateGame
    {
        Playing,
        Pause
    }

    public enum ResultGame
    {
        Win,
        Lose
    }

    public enum ModeGame
    {
        FightingWithBoss,
        FightingWithEnemy
    }

    private void EnableScripts() //Bật tắt script
    {
        if (state == StateGame.Pause) //Tắt script
        {
            foreach (var target in objList)
            {
                target.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                if (target.CompareTag("Player"))
                {
                    target.GetComponent<ControllerPlayer>().enabled = false;
                }
                else if (target.CompareTag("Enemy"))
                {
                    target.GetComponent<AI_Enemy>().enabled = false;
                    target.GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (target.CompareTag("Boss"))
                {
                    target.GetComponent<AI_Boss1>().enabled = false;
                    target.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
        else if (state == StateGame.Playing) //Bật script
        {
            foreach (var target in objList)
            {
                target.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                if (target.CompareTag("Player"))
                {
                    target.GetComponent<ControllerPlayer>().enabled = true;
                }
                else if (target.CompareTag("Enemy"))
                {
                    target.GetComponent<AI_Enemy>().enabled = true;
                    target.GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (target.CompareTag("Boss"))
                {
                    target.GetComponent<AI_Boss1>().enabled = true;
                    target.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
    }

    public void SetStatePlaying() //Nút ấn
    {
        state = StateGame.Playing;
    }

    public void SetStatePause() //Nút ấn
    {
        state = StateGame.Pause;
    }

    private void CreateList(out List<GameObject> listOut, GameObject[] arrayOut) //Tạo danh sách đối tượng
    {
        listOut = new List<GameObject>();
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            arrayOut = GameObject.FindGameObjectsWithTag("Enemy");
            listOut = arrayOut.ToList();
        }
        if (GameObject.FindGameObjectWithTag("Boss") != null)
        {
            if (listOut.Contains(GameObject.FindGameObjectWithTag("Boss")) == false)
            {
                listOut.Add(GameObject.FindGameObjectWithTag("Boss"));
            }
        }
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            if (listOut.Contains(GameObject.FindGameObjectWithTag("Player")) == false)
            {
                listOut.Add(GameObject.FindGameObjectWithTag("Player"));
            }
        }
    }

    IEnumerator StartGame() //Bắt đầu game
    {
        state = StateGame.Pause;
        yield return new WaitForSeconds(2f);

        state = StateGame.Playing;
    }

    private void GameResult() //Kết quả game
    {
        if (player.GetComponent<ControllerPlayer>().isLive == false)
        {
            result = ResultGame.Lose;
        }
        else if (boss.GetComponent<AI_Boss1>().isLive == false)
        {
            result = ResultGame.Win;
        }
    }

    private void ChangeMode() //Thay đổi chế độ game
    {
        if (enemyCounter.Length == 0)
        {
            if(Time.time > 0.4f)
            {
                mode = ModeGame.FightingWithBoss;
            }
        }
    }

    private void SetPosPlayer() //Đặt lại vị trí Player
    {
        if(mode == ModeGame.FightingWithBoss)
        {
            player.transform.position = new Vector2(0.5f, 0.5f);
        }
        mode = ModeGame.FightingWithEnemy;
    }
}
