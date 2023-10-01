using Managers;
using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : UIPopup
{
    public enum UIState
    {
        Default,
        Select
    }

    private UIManager _uiManager;

    [SerializeField] private GameObject characterList;
    [SerializeField] private Button characterSelectButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private int _selectedIndex = -1;
    public event Action<UIState> OnStateChanged;
    public event Action<int> OnCharacterSelected;

    private void Awake()
    {
        _uiManager = UIManager.Singleton;
    }

    private void Start()
    {
        _uiManager.ShowUIPopupByName(nameof(SettingButtonUI));
        _uiManager.ShowUIPopupWithPrefab(characterList,"CharacterList");
        characterSelectButton.onClick.AddListener(ShowSelectUI);
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(Quit);
        OnStateChanged += UpdateUI;
    }

    private void UpdateUI(UIState st)
    {
        
        startButton.gameObject.SetActive(st == UIState.Select);
    }

    private void StartGame()
    {
        //todo load game data from gameManager
    }

    private void ShowSelectUI()
    {
        _uiManager.ShowUIPopupByName(nameof(CharacterCardUI));
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}