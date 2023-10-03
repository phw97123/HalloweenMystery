using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UI;

public class AchievementButtonUI : UIPopup
{
    private Button _button;
    private UIManager _uiManager;

    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
        _uiManager = UIManager.Singleton;
    }

    private void Start()
    {
        _button.onClick.AddListener(ShowSettingUI);
    }

    private void ShowSettingUI()
    {
        _uiManager.ShowUIPopupByName(nameof(AchievementUI));
    }
}
