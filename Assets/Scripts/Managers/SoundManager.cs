using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>(); 
                if(_instance == null)
                {
                    GameObject soundManagerObject = new GameObject("SoundManager");
                    _instance = soundManagerObject.AddComponent<SoundManager>(); 
                }
            }
            return _instance; 
        }
    }

    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;

    private ObjectPool _objectPool;
    private List<Poolable> _prefabs; 

    private AudioSource _musicAudioSource;
    public AudioClip musicClip;

    private void Awake()
    {
        if(_instance == null)
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
        Instance._objectPool.Initialize(_prefabs); 
    }

    private void Start()
    {
        ChangeBGM(musicClip); 
    }

    private static void ChangeBGM(AudioClip music)
    {
        Instance._musicAudioSource.Stop();
        Instance._musicAudioSource.clip = music;
        Instance._musicAudioSource.Play(); 
    }

    public static  void PlayClip(AudioClip clip)
    {
        GameObject obj = Instance._objectPool.Pop("SoundSource");
        Instance._objectPool.Push("SoundSource", obj);

        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance); 
    }
}
