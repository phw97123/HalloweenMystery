using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Object = UnityEngine.Object;

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

    public enum PrefabFolder { Attacks, Character, DropItems, Enemies, UI, VFX, WeaponParts, Weapons, Sound }

    private Dictionary<string, Object> _prefabsFolder = new Dictionary<string, Object>();
    private const string FolderPath = "Assets/Resources/Prefabs";
    private const string ResFolderPath = "Assets/Resources";

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
        foreach (UnityEngine.Object obj in Resources.LoadAll<Object>(""))
        {
            Debug.Log($"resourcesObj :{obj}");
            _prefabsFolder[obj.name] = obj;
        }
    }


    public T Load<T>(string resource) where T : UnityEngine.Object
    {
        Object go = _prefabsFolder[resource];
        return go as T;
    }

    public GameObject LoadPrefab(string prefabName)
    {
        GameObject prefab = Load<GameObject>(prefabName);
        return prefab;
    }
}