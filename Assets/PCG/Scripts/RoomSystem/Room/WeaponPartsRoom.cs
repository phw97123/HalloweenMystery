using System.Collections.Generic;
using UnityEngine;

public class WeaponPartsRoom : RoomGenerator
{
    [SerializeField]
    private PrefabPlacer prefabPlacer;

    public List<EnemyPlacementData> enemyPlacementData;
    public List<ItemPlacementData> itemData;

    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, DungeonData dungeonData)
    {
        ItemPlacementHelper itemPlacementHelper =
            new ItemPlacementHelper(dungeonData.roomsDictionary[roomCenter], dungeonData.GetRoomFloorWithoutCorridors(roomCenter));

        List<EnemyPlacementData> Data = new List<EnemyPlacementData>();
        Data.Add(enemyPlacementData[UnityEngine.Random.Range(0,enemyPlacementData.Count-1)]);

        List<GameObject> placedObjects = prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);
        //placedObjects.AddRange(prefabPlacer.PlaceEnemies(enemyPlacementData, itemPlacementHelper));
        dungeonData.roomsEnemy.Remove(roomCenter);
        dungeonData.roomsEnemy.Add(roomCenter, Data);

        return placedObjects;
    }
}
