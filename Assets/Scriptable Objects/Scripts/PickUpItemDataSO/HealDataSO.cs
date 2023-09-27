using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Data", menuName = "Component/PickUpItems/Heal")]
public class HealDataSO : ScriptableObject
{
    [Header("Heal")]
    public float Heal;
}