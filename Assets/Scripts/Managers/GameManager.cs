using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes { RoomScene, StageScene, RoomContent }

public class GameManager : MonoBehaviour
{
    private Scenes _curScenes = Scenes.RoomScene;
    private bool _change = true;
    public Transform Player { get; private set; }

    private UIManager _uiManager;

    private static object _lock = new object();
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance != null) { return _instance; }

                _instance = FindObjectOfType<GameManager>();
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new GameObject(nameof(GameManager) + "-singleton").AddComponent<GameManager>();
                return _instance;
            }
        }
    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _uiManager = UIManager.Singleton;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player")?.transform;
            if (Player == null)
            {
                Debug.LogWarning("Player GameObject doesn't exists");
            }
        }

        _uiManager.ShowUIPopupByName(nameof(DungeonUI));
    }

    public void Update()
    {
        if (_change)
        {
            _change = false;
            switch (_curScenes)
            {
                case Scenes.RoomScene:
                    SceneManager.LoadScene(_curScenes.ToString());
                    break;
                case Scenes.StageScene:
                    SceneManager.LoadScene(_curScenes.ToString());
                    break;
                case Scenes.RoomContent:
                    SceneManager.LoadScene(_curScenes.ToString());
                    break;
            }
        }
    }

    public void ChangeScene(Scenes sceneName)
    {
        _change = true;
        _curScenes = sceneName;
    }
}