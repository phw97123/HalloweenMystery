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

    [SerializeField]
    private PrefabPlacer prefabPlacer;
    [SerializeField]
    public Transform roomEnemiesParent;
    [SerializeField]
    private GameObject corridorWall;
    [SerializeField]
    private Transform corridorWallParent;

    public GameObject portal;

    public UnityEvent OnStart;

    // Start is called before the first frame update
    void Start()
    {
        OnStart?.Invoke();

        _controller = player.GetComponent<EntityController>();
        _controller.OnMoveEvent += SpawnPrefab;

        if (corridorWall != null)
        {
            foreach (var value in dungoenData.GetCorridorsWithoutRoomFloor())
            {
                Instantiate(corridorWall, value + new Vector2(0.5f, 0.5f), Quaternion.identity, corridorWallParent);
            }
            corridorWallParent.gameObject.SetActive(false);
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
            if (dungoenData.roomsDictionary[key].Contains(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y)))
            {
                placedPrefab = prefabPlacer.PlaceEnemies(dungoenData.roomsEnemy[key], 
                                                         new ItemPlacementHelper(dungoenData.roomsDictionary[key], 
                                                         dungoenData.GetRoomFloorWithoutCorridors(key)));

                corridorWallParent.gameObject.SetActive(true);
                dungoenData.roomsDictionary.Remove(key);
                Debug.Log("spawn");
                break;
            }
        }
        if(placedPrefab != null)
        {
            foreach (GameObject prefab in placedPrefab)
            {
                Debug.Log(prefab.layer);
                if (prefab.layer == 7)
                {
                    
                    prefab.transform.SetParent(roomEnemiesParent, false);
                    prefab.GetComponent<HealthSystem>().OnDeath += CheckInBattle;
                }
            }
        }
    }

    private void CheckInBattle()
    {
        if(roomEnemiesParent.childCount == 0)
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
            player = FindObjectOfType<PlayerCharacterController>().gameObject;
            if (GameManager.Instance.WeaponInfo != null && FindObjectOfType<PlayerCharacterController>() != null)
            {
                GameObject weapon = ResourceManager.Instance.LoadPrefab(
                    GameManager.Instance.WeaponInfo?.Type.ToString() ?? "Sword");

                Transform pivot = player.GetComponentsInChildren<Transform>().First(t => t.name == Constants.ARM_PIVOT);
                weapon.transform.position = Vector3.zero;
                Instantiate(weapon, pivot.transform, false);
                

                CharacterStats stats = weapon.GetComponent<StatsHandler>().CurrentStats;
            }
        }
    }
}
