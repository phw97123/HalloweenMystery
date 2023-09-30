using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IReward { }

public class WeaponTypeReward : IReward
{
    public WeaponType weaponType { get; set; }
}

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
    public IReward reward;
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

    private Dictionary<Achievement, IReward> rewardMappings = new Dictionary<Achievement, IReward>
    {
        {Achievement.StageClear1, new WeaponTypeReward { weaponType = WeaponType.Axe} },
        {Achievement.StageClear2, new WeaponTypeReward { weaponType = WeaponType.Dagger} },
        {Achievement.LastBossClear, new WeaponTypeReward { weaponType = WeaponType.Rifle} },
        {Achievement.MonsterKiller, new WeaponTypeReward { weaponType = WeaponType.Pistol} },
        {Achievement.WallShooter, new WeaponTypeReward { weaponType = WeaponType.Shotgun} },
    };

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
                isAchive = false
            };
            SetAchievementRewards(achievements[i]);
        }
    }

    private void SetAchievementRewards(Achievement achivement)
    {
        if (rewardMappings.ContainsKey(achivement))
        {
            achievementDataArray[(int)achivement].reward = rewardMappings[achivement];
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

    public AchievementData[] GetAchievementData()
    {
        return achievementDataArray;
    }
}
