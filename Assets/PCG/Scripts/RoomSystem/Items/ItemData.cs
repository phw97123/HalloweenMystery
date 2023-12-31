using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public Vector2Int size = new Vector2Int(1, 1);
    public PlacementType placementType;
    public bool addOffset;
    public bool isCollide;
    public bool isPartialCollide;
    public bool isTileLike;
}
