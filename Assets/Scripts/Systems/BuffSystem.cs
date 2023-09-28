using Components;
using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffSystem : MonoBehaviour
{
    public event Action OnChangeBuff;

    private StatsHandler _statsHandler;

    private void Awake()
    {
        _statsHandler = GetComponent<StatsHandler>();
    }

    public void AddBuffStats(BuffDataSO buffDataSO)
    {
        foreach (CharacterStats stat in buffDataSO.Stats)
        {
            _statsHandler.AddStatModifier(stat);
        }

        OnChangeBuff?.Invoke();
    }

    public void RemoveBuffStats(BuffDataSO buffDataSO)
    {
        foreach (CharacterStats stat in buffDataSO.Stats)
        {
            _statsHandler.RemoveStatModifier(stat);
        }

        OnChangeBuff?.Invoke();
    }
}
