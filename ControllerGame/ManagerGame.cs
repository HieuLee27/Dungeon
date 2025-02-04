using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Linq;
using System.Runtime.CompilerServices;

public class ManagerGame : MonoBehaviour
{
    //Bản đồ game
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tileBaseGrid;
    [SerializeField] private TileBase tileBaseWall;

    //Đối tượng game
    private GameObject player;
    private List<GameObject> objList;

    //Điều kiện
    private GameObject[] enemyCounter;
    internal ModeGame mode;

    //Trạng thái game
    private StateGame state;

    private float timeCooldown = 2f;

    [Header("UI")]
    public GameObject panelChangeScene;
    private bool isChangeScene = false;

    [SerializeField] private GameObject panelResult;
    public TMP_Text textResult;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        List<GameObject> objList = new();
        state = StateGame.Playing;
    }

    private void Start()
    {
        mode = ModeGame.FightingWithEnemy;
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        CreateList(out objList, enemyCounter);
        enemyCounter = GameObject.FindGameObjectsWithTag("Enemy"); //Đếm số lượng enemy
        EnableScripts();
        if (Time.time >= timeCooldown)
        {
            timeCooldown = Time.time + 2f;
        }
        ChangeMode();

        if(isChangeScene == true)
        {
            panelChangeScene.SetActive(true);
            ChangeModeScene();
            isChangeScene = false;
        }
        StartCoroutine(ShowResult());
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
        yield return new WaitForSeconds(1f);

        state = StateGame.Playing;
        
    }

    private void ChangeMode() //Thay đổi chế độ game
    {
        if (Time.timeSinceLevelLoad > 3f)
        {
            if(enemyCounter.Length == 0)
            {
                mode = ModeGame.FightingWithBoss;
            }
        }
    }

    //UI thay đổi chế độ chơi
    private void ChangeModeScene()
    {
        Animator animator = panelChangeScene.GetComponent<Animator>();
        animator.SetBool("out", true);
        panelChangeScene.SetActive(false);
        isChangeScene = true;
    }

    IEnumerator ShowResult() //Kết quả game
    {
        GameObject player = GameObject.FindWithTag("Player");
        bool isLive = player.GetComponent<ControllerPlayer>().isLive;
        if (isLive == false)
        {
            mode = ModeGame.FightingWithEnemy;
            textResult.text = "YOU LOSE";
            SetStatePause();
            panelResult.SetActive(true);
        }
        else if(mode == ModeGame.FightingWithBoss)
        {
            yield return new WaitForSeconds(1f);
            bool isLiveBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AI_Boss1>().isLive;
            if (isLiveBoss == false)
            {
                textResult.text = "YOU WIN";
                SetStatePause();
                yield return new WaitForSeconds(1f);
                panelResult.SetActive(true);
            }
        }
        
    }
}
