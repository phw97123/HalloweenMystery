using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<DataManager>(); 
                if(_instance == null)
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
        if(_instance == null)
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
        return Application.dataPath + "/data_" + typeof(T).Name + ".json"; 
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
}
