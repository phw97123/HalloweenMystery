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

    [System.Serializable]
    public class AchievementData
    {
        public Achievement achivement;
        public GameObject unLockedWeapon;
    }

    public AchievementData[] achievementDataList;

    private Dictionary<Achievement, bool> achivementDic;

    private DataManager dataManager;

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

        achivementDic = new Dictionary<Achievement, bool>();

        dataManager = GetComponent<DataManager>();

        LoadAchievementData();
    }

    private void LoadAchievementData()
    {
        Dictionary<Achievement, bool> loadedAchievmentData = dataManager.LoadData<Dictionary<Achievement, bool>>();

        if (loadedAchievmentData != null)
        {
            achivementDic = loadedAchievmentData;
        }
        else
        {
            Init();
        }
    }

    private void Init()
    {
        foreach (Achievement achive in Enum.GetValues(typeof(Achievement)))
        {
            achivementDic.Add(achive, false);
        }
    }

    private void Start()
    {
        Unlocked();
    }

    private void Unlocked()
    {
        foreach (var kvp in achivementDic)
        {
            if (kvp.Value)
            {
                Debug.Log("Achievement unlocked: " + kvp.Key);
            }
        }
    }

    public void UnlockAchieve(Achievement achivement)
    {
        if (achivementDic.ContainsKey(achivement) && achivementDic[achivement] == false)
        {
            achivementDic[achivement] = true;
            SaveAchievementData();
        }
    }

    private void SaveAchievementData()
    {
        dataManager.SaveData(achivementDic);
    }
}
