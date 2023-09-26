using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Melee Attack Data", menuName = "Component/Attack/Melee", order = 1)]
public class MeleeAttackDataSO : AttackDataSO
{
    [Header("Melee Attack Info")] public string attackTag;
    public float arc;
    public float attackCount;
    public int targets;
}