using Components;
using Components.Action;
using Components.Stats;
using Components.Weapon;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes { RoomScene, StageScene, RoomContent, StartScene, EndingScene }

public enum Ending { GameOver, GameClear }

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private Scenes _curScenes = Scenes.RoomScene;
    private bool _isChanged;
    private bool _isIntroShown;
    public Transform Player { get; private set; }
    private UIManager _uiManager;
    private WeaponManager _weaponManager;

    public CharacterStats PlayerStats { get; private set; }
    public WeaponInfo? WeaponInfo => _weaponManager.CurrentEquippedWeapon;

    public Ending _ending = Ending.GameClear;
    private bool _isEquipped;

    private int monsterKilled = 0;
    public int MonstersKilled => monsterKilled;

    public PlayerData playerData;

    public void Monsterkilled()
    {
        monsterKilled++;
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance != null) { return _instance; }

            _instance = FindObjectOfType<GameManager>();
            if (_instance != null)
            {
                return _instance;
            }

            _instance = new GameObject(nameof(GameManager) + " - singleton").AddComponent<GameManager>();
            return _instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        _instance = null;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _uiManager = UIManager.Singleton;
        _weaponManager = WeaponManager.Singleton;
        DontDestroyOnLoad(this);

        playerData = new PlayerData();
    }

    private void Start()
    {
        _weaponManager.OnWeaponEquipped += CallEquippedEvent;
    }

    private void CallEquippedEvent(WeaponInfo? weaponInfo)
    {
        _isEquipped = true;
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == Scenes.StartScene.ToString())
        {
            ShowIntroUI();
            return;
        }

        if (_isChanged)
        {
            _isChanged = false;
            SceneManager.LoadScene(_curScenes.ToString());
        }

        if (_isEquipped)
        {
            _isEquipped = false;
            UIPopup popup = UIManager.Singleton.FindPopup(nameof(DungeonUI));
            if (popup == null) { return; }

            DungeonUI dungeonUI = popup.GetComponent<DungeonUI>();
            if (dungeonUI == null) { return; }

            BaseAttack baseAttack = Player.GetComponentInChildren<BaseAttack>();
            baseAttack.OnAttackDelayChanged += dungeonUI.UpdateDelayUI;
        }
    }

    private void SetStats(CharacterStats stat)
    {
        PlayerStats = stat;
        SavePlayerData();
    }

    public void CreatePlayer()
    {
        CreatePlayerAtPosition(Vector2.zero, Quaternion.identity);
    }

    public void CreatePlayerAtPosition(Vector2 startPosition, Quaternion rotation)
    {
        if (Player == null)
        {
            //todo load data of playerQuaternion rotation = Quaternion.identity
            GameObject playerPrefab = ResourceManager.Instance.LoadPrefab("BlueMan");
            Player = Instantiate(playerPrefab, startPosition, rotation).transform;
            if (Player == null)
            {
                Debug.LogWarning("Player GameObject doesn't exists");
            }
        }

        StatsHandler statsHandler = Player.GetComponent<StatsHandler>();
        HealthSystem healthSystem = Player.GetComponentInChildren<HealthSystem>();
        GoldSystem goldSystem = Player.GetComponentInChildren<GoldSystem>();
        BaseAttack baseAttack = Player.GetComponentInChildren<BaseAttack>();
        if (baseAttack != null)
        {
            StatsHandler weaponHandler = baseAttack.GetComponent<StatsHandler>();
            
            foreach (CharacterStats stats in playerData.weaponInfo.PartsDataList)
            {
                weaponHandler.AddStatModifier(stats);
            }
        }


        healthSystem.OnDamage += SavePlayerData;
        healthSystem.OnHeal += SavePlayerData;
        goldSystem.OnChangeOwnedGold += SavePlayerData;
        statsHandler.OnStatsChanged += SetStats;


        Debug.Log($"-------[Start]CreatePlayer--------");
        Debug.Log($"CurrentStats : {playerData.playerStats}");
        Debug.Log($"CurrentHealth : {playerData.currentHealth}");
        Debug.Log($"CurrentGold : {playerData.OwnedGold}");
        Debug.Log($"CurrentWeapon Atk: {playerData.weaponInfo.AttackData.damage}");
        Debug.Log($"-------[End]CreatePlayer--------");

        healthSystem.CurrentHealth = playerData.currentHealth;
        goldSystem.ChangeOwnedGold(playerData.OwnedGold);
        SetStats(statsHandler.CurrentStats);
    }


    public void ShowDungeonUI()
    {
        _uiManager.ShowUIPopupByName(nameof(DungeonUI));
    }

    private void ShowIntroUI()
    {
        if (_isIntroShown) { return; }

        _isIntroShown = true;
        _uiManager.ShowUIPopupByName(nameof(IntroUI));
    }

    public void InitIntroUI()
    {
        _isIntroShown = false;
    }

    public void ChangeScene(Scenes sceneName)
    {
        _isChanged = true;
        _curScenes = sceneName;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void LastBossDied()
    {
        AchiveManager achiveManager = AchiveManager.Instance;

        achiveManager.UnlockAchieve(Achievement.LastBossClear);

        HealthSystem healthSystem = Player.GetComponent<HealthSystem>();

        if (healthSystem.IsNoDamage)
        {
            achiveManager.UnlockAchieve(Achievement.NoDamageClear);
        }

        if (monsterKilled >= 20)
        {
            achiveManager.UnlockAchieve(Achievement.MonsterKiller);
        }
    }

    public void SavePlayerData()
    {
        if (Player != null)
        {
            if (playerData == null)
            {
                playerData = new PlayerData();
                playerData.weaponInfo.PartsDataList = new List<CharacterStats>();
            }

            playerData.playerStats = Player.gameObject.GetComponent<StatsHandler>().CurrentStats;
            playerData.currentHealth = Player.GetComponent<HealthSystem>().CurrentHealth;
            playerData.weaponInfo = WeaponInfo.GetValueOrDefault();
            playerData.OwnedGold = Player.GetComponent<GoldSystem>().OwnedGold;
        }

        Debug.Log($"-------[Start]SavePlayerData--------");
        Debug.Log($"CurrentStats : {playerData.playerStats}");
        Debug.Log($"CurrentHealth : {playerData.currentHealth}");
        Debug.Log($"CurrentGold : {playerData.OwnedGold}");
        Debug.Log($"CurrentWeapon : {playerData.weaponInfo.ToString()}");

        Debug.Log("------------parts");
        foreach (CharacterStats characterStats in playerData.weaponInfo.PartsDataList)
        {
            Debug.Log($"------------part: {characterStats.attackData.damage}");
        }

        Debug.Log($"-------[End]SavePlayerData--------");
    }

    public void BuyParts(CharacterStats weaponPartsStats)
    {
        playerData.weaponInfo.PartsDataList.Add(weaponPartsStats);
    }
}