using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeal : PickupItem
{
    [SerializeField] private HealDataSO healDataSO;
    private HealthSystem _healthSystem;

    public AudioClip healClip; 

    protected override void OnPickedUp(GameObject receiver)
    {
        _healthSystem = receiver.GetComponent<HealthSystem>();
        _healthSystem.ChangeHealth(healDataSO.Heal);

        if (healClip)
            SoundManager.PlayClip(healClip); 

        Destroy(gameObject);
    }
}

