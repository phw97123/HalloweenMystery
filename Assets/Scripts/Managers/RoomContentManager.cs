using Components.Stats;
using Components;
using Entities;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Components.Weapon;
using Utils;

public class RoomContentManager : MonoBehaviour
{
    public static RoomContentManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.CreatePlayer();
        if (GameManager.Instance.WeaponInfo != null && FindObjectOfType<PlayerCharacterController>() != null)
        {
            GameObject player = FindObjectOfType<PlayerCharacterController>().gameObject;
            GameObject weapon = ResourceManager.Instance.LoadPrefab(
                GameManager.Instance.WeaponInfo?.Type.ToString() ?? "Sword");

            Transform pivot = player.GetComponentsInChildren<Transform>().First(t => t.name == Constants.ARM_PIVOT);
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, pivot.transform, false);

            CharacterStats stats = weapon.GetComponent<StatsHandler>().CurrentStats;
        }
    }
}
