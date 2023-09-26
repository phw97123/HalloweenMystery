using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGold : PickupItem
{
    [SerializeField] int goldAmount = 10;
    private float goldPercentage;

    protected override void OnPickedUp(GameObject receiver)
    {
        StatsHandler statsHandler = receiver.GetComponent<StatsHandler>();
        goldPercentage = statsHandler.CurrentStats.goldPercentage;
        // 골드 추가하는 코드 넣기
    }
}
