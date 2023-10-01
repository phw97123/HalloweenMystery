using Components;
using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBuff : PickupItem
{
    [SerializeField] private BuffDataSO buffDataSO;
    private BuffSystem _buffSystem;

    public AudioClip buffClip; 

    protected override void OnPickedUp(GameObject receiver)
    {
        if (buffClip)
            SoundManager.PlayClip(buffClip); 

        _buffSystem = receiver.GetComponent<BuffSystem>();
        _buffSystem.AddBuffStats(buffDataSO);

        Invoke("DelayRemoveStatModifier", buffDataSO.BuffDuration);
        gameObject.SetActive(false);
    }

    protected void DelayRemoveStatModifier()
    {
        _buffSystem.RemoveBuffStats(buffDataSO);

        Destroy(gameObject);
    }
}
