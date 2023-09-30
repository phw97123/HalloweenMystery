using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace UI
{
    public class DungeonUI : UIPopup
    {
        private GameManager _gameManager;
        [SerializeField] private Text elapsedTimeText;
        [SerializeField] private Text coinText;

        [SerializeField] private Button openInfoButton;
        [SerializeField] private Button closeInfoButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private PlayerInfoUI playerInfo;
        
        private float _playTime;
        private bool _isInfoOpened;
        private void Awake()
        {
            //todo set model instance => gameManager
        }

        private void Start()
        {
            //todo subscribe events 
            openInfoButton.onClick.AddListener(ToggleInfoButton);
            closeInfoButton.onClick.AddListener(ToggleInfoButton);
            // pauseButton.onClick.AddListener(ToggleInfoButton);
        }

        private void Update()
        {
            _playTime += Time.deltaTime;
            elapsedTimeText.text = $"{(int)_playTime / 60:D2}:{(int)_playTime:D2}";
        }

        private void OnDestroy()
        {
            openInfoButton.onClick.RemoveAllListeners();
            closeInfoButton.onClick.RemoveAllListeners();
        }

        private void ToggleInfoButton()
        {
            _isInfoOpened = !_isInfoOpened;
            openInfoButton.gameObject.SetActive(!_isInfoOpened);
            closeInfoButton.gameObject.SetActive(_isInfoOpened);
            playerInfo.gameObject.SetActive(_isInfoOpened);
        }
    }
}