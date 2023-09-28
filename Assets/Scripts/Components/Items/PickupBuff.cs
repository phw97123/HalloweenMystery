using Components;
using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBuff : PickupItem
{
    [SerializeField] private BuffDataSO buffDataSO;
    private BuffSystem _buffSystem;

    protected override void OnPickedUp(GameObject receiver)
    {
        _buffSystem = receiver.GetComponent<BuffSystem>();
        _buffSystem.AddBuffStats(buffDataSO);

        gameObject.SetActive(false);
        StartCoroutine(DelayRemoveStatModifier());
    }

    IEnumerator DelayRemoveStatModifier()
    {
        yield return buffDataSO.BuffDuration;
        _buffSystem.RemoveBuffStats(buffDataSO);

        Destroy(gameObject);
    }
}
