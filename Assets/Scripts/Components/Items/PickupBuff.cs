using Components;
using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBuff : PickupItem
{
    [SerializeField] private BuffDataSO buffDataSO;
    private StatsHandler _statsHandler;

    protected override void OnPickedUp(GameObject receiver)
    {
        _statsHandler = receiver.GetComponent<StatsHandler>();
        foreach (CharacterStats stat in buffDataSO.Stats)
        {
            _statsHandler.AddStatModifier(stat);
        }
        gameObject.SetActive(false);
        StartCoroutine(DelayRemoveStatModifier());
    }

    IEnumerator DelayRemoveStatModifier()
    {
        yield return buffDataSO.BuffDuration;
        foreach (CharacterStats stat in buffDataSO.Stats)
        {
            _statsHandler.RemoveStatModifier(stat);
        }

        Destroy(gameObject);
    }
}
