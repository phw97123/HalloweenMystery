using Components;
using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBuff : PickupItem
{
    [SerializeField] private List<CharacterStats> statsModifier;
    [SerializeField] private int buffDuration = 1; //나중에 그냥 인스펙터창에서 넣을 수 있도록 수정하기
    private StatsHandler _statsHandler;

    protected override void OnPickedUp(GameObject receiver)
    {
        _statsHandler = receiver.GetComponent<StatsHandler>();
        foreach (CharacterStats stat in statsModifier)
        {
            _statsHandler.AddStatModifier(stat);
            DelayRemoveStatModifier(stat);
        }
    }

    IEnumerator DelayRemoveStatModifier(CharacterStats stat)
    {
        yield return buffDuration;
        _statsHandler.RemoveStatModifier(stat);
    }
}
