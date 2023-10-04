using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomContentGenerator : MonoBehaviour
{
    [SerializeField]
    private RoomGenerator playerRoom, bossRoom, weaponPartsRoom, defaultRoom;

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;


    public Transform itemParent;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCamera;


    public void GenerateRoomContent(DungeonData dungeonData)
    {
        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        SelectPlayerSpawnPoint(dungeonData);
        SelectEnemySpawnPoints(dungeonData);
        SelectWeaponPartsSpawnPoints(dungeonData);
        SelectBossSpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            if(item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
        Vector2Int playerSpawnPoint = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        graphTest.RunDijkstraAlgorithm(playerSpawnPoint, dungeonData.floorPositions);

        Vector2Int roomIndex = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            dungeonData
            );

        //FocusCameraOnThePlayer(placedPrefabs[placedPrefabs.Count - 1].transform);

        spawnedObjects.AddRange(placedPrefabs);

        RoomContentManager.Instance.CreatePlayerInRoom(playerSpawnPoint);

        FocusCameraOnThePlayer(GameManager.Instance.Player);

        dungeonData.roomsDictionary.Remove(playerSpawnPoint);
    }

    private void FocusCameraOnThePlayer(Transform playerTransform)
    {
        //cinemachineCamera.LookAt = playerTransform;
        cinemachineCamera.Follow = playerTransform;
    }


    private void SelectBossSpawnPoints(DungeonData dungeonData)
    {
        Vector2Int farthestRoomKey = new Vector2Int();
        int max = 0;

        foreach (var key in dungeonData.roomsDictionary.Keys)
        {
            if (max < graphTest.dijkstraResult[key])
            {
                max = graphTest.dijkstraResult[key];
                farthestRoomKey = key;
            }
                
        }

        spawnedObjects.AddRange(
            bossRoom.ProcessRoom(
                farthestRoomKey,
                dungeonData
                )
        );
    }
    
    private void SelectWeaponPartsSpawnPoints(DungeonData dungeonData)
    {
        Vector2Int farthestRoomKey = new Vector2Int();
        int max = 0;

        foreach (var key in dungeonData.roomsDictionary.Keys)
        {
            if (max < graphTest.dijkstraResult[key])
            {
                max = graphTest.dijkstraResult[key];
                farthestRoomKey = key;
            }
                
        }

        
        Vector2Int weaponPartsRoomKey = new Vector2Int();
        int i = 0;
        do
        {
            int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
            weaponPartsRoomKey = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);
            i++;
            if (i > 100) break;
        } while (weaponPartsRoomKey == farthestRoomKey);

        spawnedObjects.AddRange(
            weaponPartsRoom.ProcessRoom(
                weaponPartsRoomKey,
                dungeonData
                )
        );
    }

    private void SelectEnemySpawnPoints(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int,HashSet<Vector2Int>> roomData in dungeonData.roomsDictionary)
        { 
            spawnedObjects.AddRange(
                defaultRoom.ProcessRoom(
                    roomData.Key,
                    dungeonData
                    )
            );

        }
    }
    

}
