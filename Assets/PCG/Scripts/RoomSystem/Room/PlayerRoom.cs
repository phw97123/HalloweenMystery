using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoom : RoomGenerator
{
    public GameObject player;

    public List<ItemPlacementData> itemData;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    public override List<GameObject> ProcessRoom(
        Vector2Int roomCenter, 
        DungeonData dungeonData)
    {

        ItemPlacementHelper itemPlacementHelper = 
            new ItemPlacementHelper(dungeonData.roomsDictionary[roomCenter], dungeonData.GetRoomFloorWithoutCorridors(roomCenter));

        List<GameObject> placedObjects = 
            prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        Vector2Int playerSpawnPoint = roomCenter;

        //GameObject playerObject 
        //    = prefabPlacer.CreateObject(player, playerSpawnPoint + new Vector2(0.5f, 0.5f));
 
        //placedObjects.Add(playerObject);

        return placedObjects;
    }
}

public abstract class PlacementData
{
    [Min(0)]
    public int minQuantity = 0;
    [Min(0)]
    [Tooltip("Max is inclusive")]
    public int maxQuantity = 0;
    public int Quantity
        => UnityEngine.Random.Range(minQuantity, maxQuantity + 1);
}

[Serializable]
public class ItemPlacementData : PlacementData
{
    public ItemData itemData;
}

[Serializable]
public class EnemyPlacementData : PlacementData
{
    public GameObject enemyPrefab;
    public Vector2Int enemySize = Vector2Int.one;
}

