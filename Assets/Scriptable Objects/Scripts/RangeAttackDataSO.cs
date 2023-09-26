using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Range Attack Data", menuName = "Component/Attack/Range", order = 0)]
public class RangeAttackDataSO : AttackDataSO
{
    [Header("Range Attack Info")] public string bulletTag;
    public float projectilesPerAttack;
    public float anglePerShot;
    public float piercingCount;
}