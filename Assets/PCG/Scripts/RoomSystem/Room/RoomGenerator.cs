using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomGenerator : MonoBehaviour
{
    public abstract List<GameObject> ProcessRoom(
        Vector2Int roomCenter, 
        DungeonData dungeonData);
}
