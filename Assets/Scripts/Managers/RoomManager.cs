using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private Transform[] spawnPostions;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        WeaponType[] types = (WeaponType[])Enum.GetValues(typeof(WeaponType));
        WeaponManager.Singleton.CreateInteractableWeapons(types, spawnPostions[0].position, new Vector2(2, 0));
    }

    public void ChangeScene()
    {
        GameManager.Instance.ChangeScene(Scenes.RoomContent);
    }
}