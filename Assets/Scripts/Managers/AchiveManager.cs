using System;
using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<Achievement, bool> achiveDic;

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

        achiveDic = new Dictionary<Achievement, bool>();

        //if(해금된 업적 데이터가 없다면) 
        Init();
    }

    private void Init()
    {
        foreach (Achievement achive in Enum.GetValues(typeof(Achievement)))
        {
            achiveDic.Add(achive, false);
        }
    }

    private void Start()
    {
        Unlocked();
    }

    private void Unlocked()
    {
        for (int i = 0; i < achievementDataList.Length; i++)
        {
            //bool isUnlock = achiveDic[(Achive)i];
            //무기 UI 활성화
        }
    }

    public void UnlockAchieve(Achievement achive)
    {
        if (achiveDic.ContainsKey(achive))
        {
            achiveDic[achive] = true;
        }
    }
}

