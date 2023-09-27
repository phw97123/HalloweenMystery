using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gold Data", menuName = "Component/PickUpItems/Gold")]
public class GoldDataSO : ScriptableObject
{
    [Header("Gold")]
    public int Gold;
}
