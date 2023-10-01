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
        WeaponManager.Singleton.CreateInteractableWeapon(WeaponType.Sword, spawnPostions[0].position);

        AchievementData[] achievementDatas = AchiveManager.Instance.GetAchievementData();

        for (int i = 0; i < achievementDatas.Length; i++)
        {
            if (achievementDatas[i].reward is RewardData weaponTypeReward)
            {
                if (achievementDatas[i].isAchive)
                {
                    WeaponManager.Singleton.CreateInteractableWeapon((WeaponType)weaponTypeReward.weaponType, spawnPostions[i + 1].position);
                }
            }
        }
    }

    public void ChangeScene()
    {
        GameManager.Instance.ChangeScene(Scenes.RoomContent);
    }
}