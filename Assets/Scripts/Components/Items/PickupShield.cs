using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShield : PickupItem
{
    [SerializeField] private ShieldDataSO shieldDataSO;
    private HealthSystem _healthSystem;

    protected override void OnPickedUp(GameObject receiver)
    {
        _healthSystem = receiver.GetComponent<HealthSystem>();
        _healthSystem.AddShieldCount(shieldDataSO.ShieldCount);

        Destroy(gameObject);
    }
}