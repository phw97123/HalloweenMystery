using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Data", menuName = "Component/PickUpItems/Shield")]
public class ShieldDataSO : ScriptableObject
{
    [Header("Shield")]
    public int ShieldCount;
}