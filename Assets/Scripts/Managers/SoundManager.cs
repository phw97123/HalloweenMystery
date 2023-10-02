using System;
using System.Collections;
using System.Collections.Generic;
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

    private ObjectPool _objectPool;
    private List<Poolable> _prefabs;

    private AudioSource _musicAudioSource;

    private const string START_SCENE = "StartScene";
    private const string INTRO_SCENE = "TownScene";
    private const string STAGE_SCENE = "RoomContent";
    private const string DEMO_SCENE = "DemoScene";
    private const string ROOM_SCENE = "RoomScene";
    private const string GAMEENDING_SCENE = "";

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
            Debug.Log(p.Prefab);
            _prefabs.Add(p);
        }

        //_objectPool.Initialize(_prefabs); 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
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

        if (sceneName == START_SCENE)
        {
            _musicClip = Resources.Load<AudioClip>("Sound/TitleBGM");
        }
        else if (sceneName == INTRO_SCENE)
        {
            _musicClip = Resources.Load<AudioClip>("Sound/IntroBGM");
        }
        else if (sceneName == STAGE_SCENE)
        {
            //TODO

            //if (stage == 1)
            //{
            //    _musicClip = Resources.Load<AudioClip>("Sound/Stage1");
            //}
            //else if (stage == 2)
            //{
            //    _musicClip = Resources.Load<AudioClip>("Sound/Stage2");
            //}
            //else
            //{
            //    _musicClip = Resources.Load<AudioClip>("Sound/Stage3");
            //}
        }
        else if (sceneName == DEMO_SCENE)
        {
            _musicClip = Resources.Load<AudioClip>("Sound/Stage1");
        }
        else if (sceneName == GAMEENDING_SCENE)
        {
            //TODO:EndingScene, bgm add
        }

        ChangeBGM(_musicClip);
    }

    private static void ChangeBGM(AudioClip music)
    {
        Instance._musicAudioSource.Stop();
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
        settingUI.OnMusicVolumeChanged += ChangeMusicVolume;
        settingUI.OnSoundVolumeChanged += ChangeSoundVolume;
    }

    private void ChangeSoundVolume(float value)
    {
        soundEffectVolume = value;
        PlayClip(_soundChangedClip);
    }

    private void ChangeMusicVolume(float value)
    {
        Instance._musicAudioSource.Stop();
        musicVolume = value;
        _musicAudioSource.volume = musicVolume;
        Instance._musicAudioSource.Play();
    }
}