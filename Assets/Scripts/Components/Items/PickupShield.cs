using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShield : PickupItem
{
    [SerializeField] private ShieldDataSO shieldDataSO;
    private HealthSystem _healthSystem;

    public AudioClip shieldClip; 
    protected override void OnPickedUp(GameObject receiver)
    {
        _healthSystem = receiver.GetComponent<HealthSystem>();
        _healthSystem.AddShieldCount(shieldDataSO.ShieldCount);

        if (shieldClip)
            SoundManager.PlayClip(shieldClip); 

        Destroy(gameObject);
    }
}