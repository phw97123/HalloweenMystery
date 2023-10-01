using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGold : PickupItem
{
    [SerializeField] private GoldDataSO goldDataSO;
    private float _goldPercentage;

    public AudioClip pickUpClip; 
    protected override void OnPickedUp(GameObject receiver)
    {
        StatsHandler statsHandler = receiver.GetComponent<StatsHandler>();
        GoldSystem goldSystem = receiver.GetComponent<GoldSystem>();
        _goldPercentage = statsHandler.CurrentStats.goldPercentage;

        goldSystem.ChangeOwnedGold(Mathf.CeilToInt(goldDataSO.Gold * (1 + _goldPercentage)));

        if (pickUpClip)
            SoundManager.PlayClip(pickUpClip); 

        Destroy(gameObject);
    }
}
