using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeal : PickupItem
{
    [SerializeField] private HealDataSO healDataSO;
    private HealthSystem _healthSystem;

    protected override void OnPickedUp(GameObject receiver)
    {
        _healthSystem = receiver.GetComponent<HealthSystem>();
        _healthSystem.ChangeHealth(healDataSO.Heal);

        Destroy(gameObject);
    }
}

