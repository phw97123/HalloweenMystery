using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.dataPath + "/data.json";
    }

    public void SaveData<T>(T data)
    {
        string jsonData = JsonConvert.SerializeObject(data);
        File.WriteAllText(savePath, jsonData);
        print(savePath);
    }
    
    public T LoadData<T>()
    {
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
