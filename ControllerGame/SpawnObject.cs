using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnObject : MonoBehaviour
{
    public GameObject map;
    public Tilemap mapPos;
    private HashSet<Vector2Int> path;
    [SerializeField] private GameObject enemy;
    private GameObject singleton;
    private GameObject player;

    [Header("Enemy count")]
    [SerializeField] private int maxEnemyCount;
    private List<Vector2Int> enemyPos;
    private Paint paintScript;
    public static Dictionary<Vector2, float> listEnemy;

    [Header("Boss")]
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject managerGame;

    [Header("ManagerGame")]
    public GameObject gameManager;

    private void Awake()
    {
        listEnemy = new Dictionary<Vector2, float>();
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(WaitScript());
        SetPosPlayer();
    }

    private void Update()
    {
        if (gameManager.GetComponent<ManagerGame>().mode == ManagerGame.ModeGame.FightingWithBoss && Time.time > 1.2f)
        {
            SpawnBoss();
        }
    }

    private void SetPosPlayer() //Đặt lại vị trí Player
    {
        player.transform.position = new Vector2(0.5f, 0.5f);
    }

    private void SpawnEnemy(HashSet<Vector2Int> path) //Sinh kẻ thù vị trị ngẫu nhiên trên bản đồ
    {
        List<Vector2Int> listPos = path.ToList();
        HashSet<Vector2Int> enemyPosHash = new();

        for (int i = 0; i < maxEnemyCount; i++) //tạo danh sách vị trí xuất hiện kẻ thù
        {
            int index = Random.Range(0, listPos.Count);
            enemyPosHash.Add(listPos[index]);
        }
        int indexPlayer = Random.Range(0, listPos.Count);
        enemyPos = enemyPosHash.ToList<Vector2Int>();
        

        foreach (var pos in enemyPos)
        {
            var enemyPart = Instantiate(enemy, (Vector2)pos + new Vector2(0.5f, 0.5f), enemy.transform.rotation);
            listEnemy.Add(enemyPart.transform.position, GetDistance.GetDistanceBetween(player.transform.position, enemyPart.transform.position));
        }
    }

    IEnumerator WaitScript()
    {
        path = new HashSet<Vector2Int>();

        yield return new WaitForSeconds(0.3f);
        paintScript = map.GetComponent<Paint>();
        if (paintScript != null)
        {

        }
        else
        {

        }

        path = paintScript.mapPos;
        
        SpawnEnemy(path);
        //LogPosition(enemyPos);
    }

    private void SpawnBoss()
    {
        if (singleton == null)
        {
            singleton = Instantiate(boss, new Vector2(0, 4f), boss.transform.rotation);
        }
    }
}
