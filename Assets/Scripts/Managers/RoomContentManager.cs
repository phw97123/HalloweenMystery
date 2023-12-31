
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Entites;
using UnityEngine.SceneManagement;

public class RoomContentManager : MonoBehaviour
{
    public static RoomContentManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject player = null;

    private EntityController _controller = null;

    public DungeonData dungoenData = null;

    [SerializeField] private PrefabPlacer prefabPlacer;
    [SerializeField] public Transform roomEnemiesParent;
    [SerializeField] private GameObject corridorWall;
    [SerializeField] private Transform corridorWallParent;

    [SerializeField] private MinimapCamera minimapCamera;

    public GameObject portal;
    private bool isCheckCoroutineRun = false;

    public UnityEvent OnStart;

    // Start is called before the first frame update
    void Start()
    {
        AchievementCheck();
        OnStart?.Invoke();


        if (corridorWall != null)
        {
            foreach (var value in dungoenData.GetCorridorsWithoutRoomFloor())
            {
                Instantiate(corridorWall, value + new Vector2(0.5f, 0.5f), Quaternion.identity, corridorWallParent);
            }

            corridorWallParent.gameObject.SetActive(false);
        }
    }


    public void AchievementCheck()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            AchiveManager.Instance.UnlockAchieve(Achievement.StageClear1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            AchiveManager.Instance.UnlockAchieve(Achievement.StageClear2);
        }
    }

    //public void Update()
    //{
    //    if (roomEnemiesParent.childCount == 0)
    //    {
    //        portal.SetActive(true);
    //    }
    //    else
    //    {
    //        portal.SetActive(false);
    //    }
    //}


    private void SpawnPrefab(Vector2 vector)
    {
        List<GameObject> placedPrefab = null;
        foreach (var key in dungoenData.roomsDictionary.Keys)
        {
            if (dungoenData.roomsDictionary[key]
                .Contains(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y)))
            {
                placedPrefab = prefabPlacer.PlaceEnemies(dungoenData.roomsEnemy[key],
                    new ItemPlacementHelper(dungoenData.roomsDictionary[key],
                        dungoenData.GetRoomFloorWithoutCorridors(key)));


                dungoenData.roomsDictionary.Remove(key);
                break;
            }
        }

        if (placedPrefab != null)
        {
            foreach (GameObject prefab in placedPrefab)
            {
                if (prefab.layer == 7)
                {
                    prefab.transform.SetParent(roomEnemiesParent, false);
                    //prefab.GetComponent<HealthSystem>().OnDeath += CheckInBattle;
                }
            }

            if (roomEnemiesParent.childCount >= 2) corridorWallParent.gameObject.SetActive(true);
        }

        if (isCheckCoroutineRun == false)
        {
            StartCoroutine(Check());
        }
    }


    IEnumerator Check()
    {
        isCheckCoroutineRun = true;
        while (true)
        {
            if (roomEnemiesParent.childCount == 0)
            {
                corridorWallParent.gameObject.SetActive(false);
                if (portal)
                    portal.SetActive(true);
                isCheckCoroutineRun = false;
                break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void CreatePlayerInRoom(Vector2Int position)
    {
        if (player != null)
        {
            player.transform.position = new Vector3Int(position.x, position.y, 0);
        }
        else
        {
            GameManager.Instance.CreatePlayerAtPosition(position, Quaternion.identity);
            GameManager.Instance.ShowDungeonUI();
            player = GameManager.Instance.Player.gameObject;
            if (GameManager.Instance.WeaponInfo != null && GameManager.Instance.Player != null)
            {
                GameObject weapon = ResourceManager.Instance.LoadPrefab(
                    GameManager.Instance.WeaponInfo?.type.ToString() ?? "Sword");
                WeaponManager.Singleton.EquipWeapon(Instantiate(weapon), player);
                GameManager.Instance.AddPartData();
            }
        }

        _controller = player.GetComponent<EntityController>();
        _controller.OnMoveEvent += SpawnPrefab;
        _controller.OnMoveEvent += MoveMinimapCamera;
    }

    private void MoveMinimapCamera(Vector2 vector)
    {
        minimapCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,
            minimapCamera.transform.position.z);
    }
}