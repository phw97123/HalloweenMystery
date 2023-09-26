using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGold : PickupItem
{
    [SerializeField] int goldAmount = 10;
    private float _goldPercentage;

    protected override void OnPickedUp(GameObject receiver)
    {
        StatsHandler statsHandler = receiver.GetComponent<StatsHandler>();
        GoldSystem goldSystem = receiver.GetComponent<GoldSystem>();
        _goldPercentage = statsHandler.CurrentStats.goldPercentage;

        goldSystem.ChangeOwnedGold(Mathf.CeilToInt(goldAmount * (1 + _goldPercentage)));
    }
}
