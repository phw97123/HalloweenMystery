using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager _instance;

    public static ResourceManager Instance
    {
        get
        {
            if (_instance != null) { return _instance; }

            _instance = FindObjectOfType<ResourceManager>();
            if (_instance != null) { return _instance; }

            _instance = new GameObject(nameof(ResourceManager) + "-singleton").AddComponent<ResourceManager>();
            return _instance;
        }
    }

    public enum PrefabFolder { Attacks, Character, DropItems, Enemies, UI, VFX, WeaponParts, Weapons, Sound}

    private Dictionary<string, int> _prefabsFolder = new Dictionary<string, int>();
    private string _folderPath = "Assets/Resources/Prefabs";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        GetPrefabName();
        DontDestroyOnLoad(gameObject);
    }

    private void GetPrefabName()
    {
        foreach (PrefabFolder folder in Enum.GetValues(typeof(PrefabFolder)))
        {
            string[] prefabPaths = Directory.GetFiles(_folderPath + "/" + folder.ToString(), "*.prefab");
            foreach (string prefabPath in prefabPaths)
            {
                _prefabsFolder.Add(Path.GetFileNameWithoutExtension(prefabPath), (int)folder);
            }
        }
    }

    public T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject LoadPrefab(string name)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{(PrefabFolder)_prefabsFolder[name]}/{name}");

        return prefab;
    }
}
//ResourceManager.Instance.LoadPrefab("BlueMan");