using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AchieveNotifyUI : UIPopup
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text contentsText;

        private AchiveManager _achieveManager;

        private void Awake()
        {
            _achieveManager = AchiveManager.Instance;
        }

        private void Start()
        {
            Time.timeScale = 0;
            closeButton.onClick.AddListener(() => {
                contentsText.text = "";
                Time.timeScale = 1;
                gameObject.SetActive(false); 
            });
            contentsText.text = _achieveManager.AchievementsString[(int)_achieveManager.CurAchieve];
        }
    }

}
