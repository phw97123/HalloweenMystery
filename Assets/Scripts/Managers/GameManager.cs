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
        SetStats(statsHandler.CurrentStats);
        statsHandler.OnStatsChanged += SetStats;
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
        Debug.Log("LastBossClear"); 

        HealthSystem healthSystem = Player.GetComponent<HealthSystem>(); 

        if(healthSystem.IsNoDamage)
        {
            achiveManager.UnlockAchieve(Achievement.NoDamageClear);
            Debug.Log("NoDamageClear");

        }

        if (monsterKilled >= 20)
        {
            achiveManager.UnlockAchieve(Achievement.MonsterKiller);
            Debug.Log("MonsterKiller");
        }
    }
}