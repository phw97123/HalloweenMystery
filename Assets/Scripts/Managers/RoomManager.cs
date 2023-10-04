using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        GameManager.Instance.CreatePlayer();

        WeaponType[] types = (WeaponType[])Enum.GetValues(typeof(WeaponType));
        WeaponManager.Singleton.CreateInteractableWeapon(WeaponType.Sword, spawnPostions[0].position);

        AchievementData[] achievementDatas = AchiveManager.Instance.GetAchievementData();
        IEnumerable<RewardData> rewards = achievementDatas
            .Where(data => data.isAchive )
            .Where(data => data.reward != null)
            .Select(data => data.reward);
        foreach (RewardData rewardData in rewards)
        {
            if (rewardData.weaponType == null) { continue; }

            int index = (int)rewardData.weaponType;
            WeaponManager.Singleton.CreateInteractableWeapon((WeaponType)rewardData.weaponType,
                spawnPostions[index].position);
        }
    }

    public void ChangeScene()
    {
        GameManager.Instance.ChangeScene(Scenes.RoomContent);
    }
}