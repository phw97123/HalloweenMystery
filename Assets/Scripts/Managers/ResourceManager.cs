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

    public enum PrefabFolder { Attacks, Character, DropItems, Enemies, UI, VFX, WeaponParts, Weapons, Sound }

    private Dictionary<string, string> _prefabsFolder = new Dictionary<string, string>();
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

    private void GetPrefabNameRecursive(string currentDirectory)
    {
        string[] directories = Directory.GetDirectories(currentDirectory);
        string[] files = Directory.GetFiles(currentDirectory);
        ;

        foreach (string directory in directories)
        {
            GetPrefabNameRecursive(directory);
        }

        foreach (string file in files)
        {
            if (Path.GetExtension(file) == ".meta") { continue; }

            _prefabsFolder[Path.GetFileNameWithoutExtension(file)] =
                currentDirectory.Replace(Path.Combine("Assets", "Resources") + Path.DirectorySeparatorChar, "");
        }
    }

    private void GetPrefabName()
    {
        string[] directories = Directory.GetDirectories(Path.Combine("Assets", "Resources"));
        foreach (string directory in directories)
        {
            GetPrefabNameRecursive(directory);
        }
    }


    public T Load<T>(string resource) where T : UnityEngine.Object
    {
        string path = Path.Combine(_prefabsFolder[resource], resource);
        return Resources.Load<T>(path);
    }

    public GameObject LoadPrefab(string prefabName)
    {
        GameObject prefab = Load<GameObject>(prefabName);
        return prefab;
    }
}