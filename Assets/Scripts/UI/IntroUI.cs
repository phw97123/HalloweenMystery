using Managers;
using Scriptable_Objects.Scripts;
using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Button startButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private UIPopup _characterList;
    private int _selectedIndex = -1;
    private CharacterDataSO _characterData;

    public event Action<UIState> OnStateChanged;
    public event Action<CharacterDataSO> OnPlayGame;
    public event Action<int> OnCharacterSelected;

    private void Awake()
    {
        _uiManager = UIManager.Singleton;
    }

    private void Start()
    {
        _uiManager.ShowUIPopupByName(nameof(SettingButtonUI));
        startButton.onClick.AddListener((() => UpdateUI(UIState.Select)));
        quitButton.onClick.AddListener(Quit);
        playButton.onClick.AddListener(PlayGame);
        OnStateChanged += UpdateUI;
    }

    private void PlayGame()
    {
        if (_selectedIndex == -1)
        {
            //todo refactor i18n
            AlertDialogUI alert = _uiManager.ShowUIPopupByName(nameof(AlertDialogUI)).GetComponent<AlertDialogUI>();
            alert.MessageText.text = "캐릭터를 선택해주세요.";
            alert.CancelButton.gameObject.SetActive(false);
            alert.SubmitButton.onClick.AddListener(CloseAlertDialog);
            return;
        }

        StartCoroutine(LoadRoomSceneAsync());
    }

    private void CloseAlertDialog()
    {
        _uiManager.ClosePopup(nameof(AlertDialogUI));
    }

    private void UpdateUI(UIState st)
    {
        startButton.gameObject.SetActive(st == UIState.Default);
        playButton.gameObject.SetActive(st == UIState.Select);

        if (st == UIState.Select)
        {
            ShowSelectUI();
        }
    }

    private void ShowSelectUI()
    {
        _characterList = _uiManager.ShowUIPopupByName(nameof(CharacterListUI));
        _characterList.GetComponent<CharacterListUI>().OnCharacterItemSelected += SelectCharacterCharacterItem;
    }

    private void SelectCharacterCharacterItem(CharacterDataSO data, int index)
    {
        OnCharacterSelected?.Invoke(index);
        _selectedIndex = index;
        _characterData = data;
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private IEnumerator LoadRoomSceneAsync()
    {
        OnPlayGame?.Invoke(_characterData);
        AsyncOperation operation = SceneManager.LoadSceneAsync("TownScene");
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}