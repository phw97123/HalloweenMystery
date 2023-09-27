using Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Data", menuName = "Component/PickUpItems/Buff")]
public class BuffDataSO : ScriptableObject
{
    [Header("Buff")]
    public CharacterStats[] Stats;
    public float BuffDuration;
}

