using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardData
{
    public WeaponType? weaponType { get; set; }
}

public enum Achievement
{
    StageClear1,
    StageClear2,
    LastBossClear,
    NoSkipGoing,
    MonsterKiller,
    NoDamageClear, // x
}

public class AchievementData
{
    public Achievement achivement;
    public RewardData reward;
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
    public AchievementData[] achievementDataArray { get; private set; }

    public string[] AchievementsString { get; private set; }
    public Achievement CurAchieve;

    private Dictionary<Achievement, RewardData> rewardMappings = new Dictionary<Achievement, RewardData>
    {
        {Achievement.StageClear1, new RewardData { weaponType = WeaponType.Axe} },
        {Achievement.StageClear2, new RewardData { weaponType = WeaponType.Dagger} },
        {Achievement.LastBossClear, new RewardData { weaponType = WeaponType.Rifle} },
        {Achievement.NoSkipGoing, new RewardData { weaponType = WeaponType.Pistol} },
        {Achievement.MonsterKiller, new RewardData { weaponType = WeaponType.Shotgun} },
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
        SetAchievementsString();
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

            CurAchieve = achivement;
            UIManager.Singleton.ShowUIPopupByName(nameof(AchieveNotifyUI));
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

    private void SetAchievementsString()
    {
        AchievementsString = new string[Enum.GetValues(typeof(Achievement)).Length];
        AchievementsString[(int)Achievement.StageClear1] = "스테이지1 클리어";
        AchievementsString[(int)Achievement.StageClear2] = "스테이지2 클리어";
        AchievementsString[(int)Achievement.LastBossClear] = "최종보스 처치";
        AchievementsString[(int)Achievement.NoSkipGoing] = "인트로 영상 시청 완료";
        AchievementsString[(int)Achievement.MonsterKiller] = "몬스터 20마리 이상 처치";
    }
}
