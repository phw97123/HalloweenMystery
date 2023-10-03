using Components.Stats;
using Components;
using Entities;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Components.Weapon;
using Utils;
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
                    prefab.GetComponent<HealthSystem>().OnDeath += CheckInBattle;
                }
            }
            if (roomEnemiesParent.childCount >= 2) corridorWallParent.gameObject.SetActive(true);
        }
    }

    private void CheckInBattle()
    {
        Invoke("Check", 1.0f);
    }
    
    private void Check()
    {
        if (roomEnemiesParent.childCount <= 1)
        {
            corridorWallParent.gameObject.SetActive(false);
            portal.SetActive(true);
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
                    GameManager.Instance.WeaponInfo?.Type.ToString() ?? "Sword");
                WeaponManager.Singleton.EquipWeapon(Instantiate(weapon), player);
            }
        }
        _controller = player.GetComponent<EntityController>();
        _controller.OnMoveEvent += SpawnPrefab;
        _controller.OnMoveEvent += MoveMinimapCamera;
    }

    private void MoveMinimapCamera(Vector2 vector)
    {
        minimapCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, minimapCamera.transform.position.z);
    }
}