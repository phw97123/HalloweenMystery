using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Achievement
{
    StageClear1,
    StageClear2,
    LastBossClear,
    MonsterKiller,
    NoDamageClear,
    WallShooter,
    NoItemClear
}

public class AchievementData
{
    public Achievement achivement;
    public GameObject unLockedWeapon;
    public bool isAchive;
}

public class AchiveManager : MonoBehaviour
{
    private static AchiveManager _instance;

    public static AchiveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AchiveManager>();
                if (_instance == null)
                {
                    GameObject achiveManagerObject = new GameObject("AchiveManager");
                    _instance = achiveManagerObject.AddComponent<AchiveManager>();
                }
            }
            return _instance;
        }
    }
    private DataManager dataManager;

    private Achievement[] achievements;
    private AchievementData[] achievementDataArray;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dataManager = DataManager.Instance;

        LoadAchievementData();
    }

    private void Start()
    {
        Unlocked();
    }

    private void LoadAchievementData()
    {
        AchievementData[] loadedAchievementDataArray = dataManager.LoadData<AchievementData[]>();

        if (loadedAchievementDataArray != null)
        {
            achievementDataArray = loadedAchievementDataArray;
        }
        else
        {
            Init();
        }
    }

    private void Init()
    {
        achievements = (Achievement[])Enum.GetValues(typeof(Achievement));
        achievementDataArray = new AchievementData[achievements.Length];

        for (int i = 0; i < achievements.Length; i++)
        {
            achievementDataArray[i] = new AchievementData
            {
                achivement = achievements[i],
                unLockedWeapon = null,
                isAchive = false
            };
        }
    }

    private void Unlocked()
    {
        foreach (var unlock in achievementDataArray)
        {
            if (unlock.isAchive)
            {
                Debug.Log("Achievement unlocked: " + unlock.achivement);
                SetAchievementRewards(unlock.achivement); 
            }
        }
    }

    public void UnlockAchieve(Achievement achivement)
    {
        if (achievementDataArray[(int)achivement].achivement == achivement && achievementDataArray[(int)achivement].isAchive == false)
        {
            achievementDataArray[(int)achivement].isAchive = true;
            SaveAchievementData();
        }
    }

    private void SaveAchievementData()
    {
        dataManager.SaveData(achievementDataArray);
    }

    private void SetAchievementRewards(Achievement achivement)
    {
        switch (achivement)
        {
            case Achievement.StageClear1:
                //achievementDataArray[(int)achivement].unLockedWeapon = WeaponManager.Singleton.CreateInteractableWeapon(weaponName,position); 
                break;
            case Achievement.StageClear2:
                break;
            case Achievement.LastBossClear:
                break;
            case Achievement.MonsterKiller:
                break;
            case Achievement.NoDamageClear:
                break;
            case Achievement.WallShooter:
                break;
            case Achievement.NoItemClear:
                break;
        }
    }
}
