using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Attack Data", menuName = "Component/Attack/Default", order = 1)]
public class AttackDataSO : ScriptableObject
{
    [Header("Attack Info")] public LayerMask target;
    public string prefabTag;
    public int damage;
    public float delay;
    public float speed;
    public float range;

    [Header("KnockBack Info")] public bool isKnockBackOn;
    public float knockBackDuration;
    public float knockBackPower;
}