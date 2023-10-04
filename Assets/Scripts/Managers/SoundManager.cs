using Components.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    GameObject soundManagerObject = new GameObject("SoundManager");
                    _instance = soundManagerObject.AddComponent<SoundManager>();
                }
            }

            return _instance;
        }
    }

    private AudioClip _soundChangedClip;
    [SerializeField] [Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField] [Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField] [Range(0f, 1f)] private float musicVolume;


    private DataManager _dataManager;
    private ObjectPool _objectPool;
    private List<Poolable> _prefabs;

    private AudioSource _musicAudioSource;

    private const string START_SCENE = "StartScene";
    private const string INTRO_SCENE = "TownScene";
    private const string STAGE1_SCENE = "Stage1";
    private const string STAGE2_SCENE = "Stage2";
    private const string STAGE3_SCENE = "Stage3";

    private const string DEMO_SCENE = "DemoScene";

    //private const string ROOM_SCENE = "RoomScene";
    private const string GAMEENDING_SCENE = "EndingScene";

    private AudioClip _musicClip;


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

        _dataManager = DataManager.Instance;
        _musicAudioSource = GetComponent<AudioSource>();
        _musicAudioSource.volume = musicVolume;
        _musicAudioSource.loop = true;

        _objectPool = GetComponent<ObjectPool>();
        _prefabs = new List<Poolable>();

        GameObject[] obj = Resources.LoadAll<GameObject>("Prefabs/Sound");

        foreach (GameObject o in obj)
        {
            Poolable p = new Poolable();
            p.Prefab = o;
            p.Size = 5;
            p.Tag = o.name;
            _prefabs.Add(p);
        }

        _objectPool.Initialize(_prefabs);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SoundSettingsData soundSettings = _dataManager.LoadSoundSettings();
        musicVolume = soundSettings.musicVolume;
        soundEffectVolume = soundSettings.musicVolume;

        string sceneName = SceneManager.GetActiveScene().name;
        SetBGMByScene(sceneName);
        _soundChangedClip = ResourceManager.Instance.Load<AudioClip>("WeaponEquip");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 로드된 씬의 이름을 가져와 BGM 설정
        string sceneName = scene.name;
        SetBGMByScene(sceneName);
    }

    private void SetBGMByScene(string sceneName)
    {
        _musicClip = null;

        switch (sceneName)
        {
            case START_SCENE:
                _musicClip = Resources.Load<AudioClip>(Path.Combine("Sound", "TitleBGM"));
                break;
            case INTRO_SCENE:
                _musicClip = Resources.Load<AudioClip>(Path.Combine("Sound", "IntroBGM"));
                break;
            case STAGE1_SCENE:
                _musicClip = Resources.Load<AudioClip>(Path.Combine("Sound", "Stage1"));
                break;
            case STAGE2_SCENE:
                _musicClip = Resources.Load<AudioClip>(Path.Combine("Sound", "Stage2"));
                break;
            case STAGE3_SCENE:
                _musicClip = Resources.Load<AudioClip>(Path.Combine("Sound", "Stage3"));
                break;
            case DEMO_SCENE:
                _musicClip = Resources.Load<AudioClip>(Path.Combine("Sound", "Stage1"));
                break;
            case GAMEENDING_SCENE:
                _musicClip = Resources.Load<AudioClip>(Path.Combine("Sound", "GameOver"));
                break;
        }

        ChangeBGM(_musicClip);
    }

    private static void ChangeBGM(AudioClip music)
    {
        Instance._musicAudioSource.Stop();
        Instance._musicAudioSource.volume = Instance.musicVolume;
        Instance._musicAudioSource.clip = music;
        Instance._musicAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = Instance._objectPool.Pop("SoundSource");
        Instance._objectPool.Push("SoundSource", obj);

        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance);
    }

    public void SubscribeSettingsUI(SettingUI settingUI)
    {
        SoundSettingsData soundSettingsData = new SoundSettingsData(
            musicVolume: musicVolume,
            soundVolume: soundEffectVolume,
            isMusicOn: musicVolume <= 0f,
            isSoundOn: soundEffectVolume <= 0f);
        settingUI.Initialize(soundSettingsData);
        settingUI.OnMusicVolumeChanged += ChangeMusicVolume;
        settingUI.OnSoundVolumeChanged += ChangeSoundVolume;
    }

    private void ChangeSoundVolume(float value)
    {
        soundEffectVolume = value;
        PlayClip(_soundChangedClip);
        SoundSettingsData data = new SoundSettingsData(
            musicVolume: musicVolume,
            soundVolume: soundEffectVolume,
            isMusicOn: musicVolume <= 0f,
            isSoundOn: soundEffectVolume <= 0f);
        _dataManager.SaveSoundSettings(data);
    }

    private void ChangeMusicVolume(float value)
    {
        Instance._musicAudioSource.Stop();
        musicVolume = value;
        _musicAudioSource.volume = musicVolume;
        Instance._musicAudioSource.Play();

        SoundSettingsData data = new SoundSettingsData(
            musicVolume: musicVolume,
            soundVolume: soundEffectVolume,
            isMusicOn: musicVolume <= 0f,
            isSoundOn: soundEffectVolume <= 0f);
        _dataManager.SaveSoundSettings(data);
    }
}