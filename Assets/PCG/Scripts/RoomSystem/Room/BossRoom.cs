using System.Collections.Generic;
using UnityEngine;

public class BossRoom : RoomGenerator
{
    [SerializeField]
    private PrefabPlacer prefabPlacer;

    public List<EnemyPlacementData> enemyPlacementData;
    public List<ItemPlacementData> itemData;

    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, DungeonData dungeonData)
    {
        ItemPlacementHelper itemPlacementHelper =
            new ItemPlacementHelper(dungeonData.roomsDictionary[roomCenter], dungeonData.GetRoomFloorWithoutCorridors(roomCenter));

        List<GameObject> placedObjects = prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);
        //placedObjects.AddRange(prefabPlacer.PlaceEnemies(enemyPlacementData, itemPlacementHelper));
        dungeonData.roomsEnemy.Remove(roomCenter);
        dungeonData.roomsEnemy.Add(roomCenter, enemyPlacementData);

        return placedObjects;
    }
}
