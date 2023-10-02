using System.Collections.Generic;
using UnityEngine;

enum RoomType
{
    start = 0,
    weaponParts,
    boss,
    normal
}
public class DungeonData
{
    public Dictionary<Vector2Int, int> roomsType;
    public Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary;
    public Dictionary<Vector2Int, List<EnemyPlacementData>> roomsEnemy = new Dictionary<Vector2Int, List<EnemyPlacementData>>();
    public HashSet<Vector2Int> floorPositions;
    public HashSet<Vector2Int> corridorPositions;

    public HashSet<Vector2Int> GetRoomFloorWithoutCorridors(Vector2Int dictionaryKey)
    {
        HashSet<Vector2Int> roomFloorNoCorridors = new HashSet<Vector2Int>(roomsDictionary[dictionaryKey]);
        roomFloorNoCorridors.ExceptWith(corridorPositions);
        return roomFloorNoCorridors;
    }

    public HashSet<Vector2Int> GetCorridorsWithoutRoomFloor()
    {
        HashSet<Vector2Int> corridorsNoRoomFloor = new HashSet<Vector2Int>(corridorPositions);
        foreach(var roomPositions in roomsDictionary.Values)
        {
            corridorsNoRoomFloor.ExceptWith(roomPositions);
        }
        return corridorsNoRoomFloor;
    }
}