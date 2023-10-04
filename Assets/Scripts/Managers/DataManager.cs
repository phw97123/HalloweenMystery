using Components.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using Unity.VisualScripting;


public class DataManager : MonoBehaviour
{
    public static class PreferenceKeys
    {
        public const string SoundVolumeKey = "SoundVolume";
        public const string MusicVolumeKey = "MusicVolume";
        public const string SoundOnKey = "SoundOn";
        public const string MusicOnKey = "MusicOn";
    }


    private static DataManager _instance;

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataManager>();
                if (_instance == null)
                {
                    GameObject dataManagerObject = new GameObject("DataManager");
                    _instance = dataManagerObject.AddComponent<DataManager>();
                }
            }

            return _instance;
        }
    }

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
    }

    private string GetSavePath<T>()
    {
        return Application.persistentDataPath + Path.DirectorySeparatorChar + "data_" + typeof(T).Name + ".json";
    }

    public void SaveData<T>(T data)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(GetSavePath<T>(), jsonData);
        Debug.Log("Data saved : " + typeof(T).Name);
    }

    public T LoadData<T>()
    {
        string savePath = GetSavePath<T>();

        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return default(T);
        }
    }

    public void SaveSoundSettings(SoundSettingsData data)
    {
        PlayerPrefs.SetFloat(PreferenceKeys.SoundVolumeKey, data.soundVolume);
        PlayerPrefs.SetFloat(PreferenceKeys.MusicVolumeKey, data.musicVolume);
        PlayerPrefs.SetInt(PreferenceKeys.SoundOnKey, data.isSoundOn ? 1 : 0);
        PlayerPrefs.SetInt(PreferenceKeys.MusicOnKey, data.isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public SoundSettingsData LoadSoundSettings()
    {
        if (!PlayerPrefs.HasKey(PreferenceKeys.SoundVolumeKey))
        {
            return new SoundSettingsData(1f, 1f, true, true);
        }

        float soundVol = PlayerPrefs.GetFloat(PreferenceKeys.SoundVolumeKey, 1f);
        float musicVol = PlayerPrefs.GetFloat(PreferenceKeys.MusicVolumeKey, 1f);
        bool isSoundOn = PlayerPrefs.GetInt(PreferenceKeys.SoundOnKey, 1) == 1;
        bool isMusicOn = PlayerPrefs.GetInt(PreferenceKeys.MusicOnKey, 1) == 1;
        return new SoundSettingsData(soundVolume: soundVol, musicVolume: musicVol, isMusicOn: isSoundOn,
            isSoundOn: isMusicOn);
    }
}