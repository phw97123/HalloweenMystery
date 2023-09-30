using Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes { RoomScene, StageScene, RoomContent }

public class GameManager : MonoBehaviour
{
    private Scenes _curScenes = Scenes.RoomScene;

    private bool _change = true;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance != null) { return _instance; }

            _instance = FindObjectOfType<GameManager>();
            if (_instance != null) { return _instance; }

            _instance = new GameObject(nameof(GameManager) + "-singleton").AddComponent<GameManager>();
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
        DontDestroyOnLoad(gameObject);
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
