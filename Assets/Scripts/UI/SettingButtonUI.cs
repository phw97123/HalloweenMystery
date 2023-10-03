using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtonUI : UIPopup
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
        _uiManager.ShowUIPopupByName(nameof(SettingUI));
    }
}