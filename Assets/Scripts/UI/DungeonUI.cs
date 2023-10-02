using Components;
using Components.Action;
using Components.Stats;
using Components.Weapon;
using Managers;
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
        private HealthSystem _healthSystem;
        private BaseAttack _attack;

        [SerializeField] private RectTransform container;
        [SerializeField] private Text elapsedTimeText;
        [SerializeField] private Text coinText;

        [SerializeField] private PlayerInfoUI playerInfo;
        [SerializeField] private Button openInfoButton;
        [SerializeField] private Button closeInfoButton;
        [SerializeField] private Image healthBar;
        [SerializeField] private Image atkDelayBar;

        private bool _isPaused;
        private float _playTime;
        private bool _isInfoOpened;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void Start()
        { 
            openInfoButton.onClick.AddListener(ToggleInfoButton);
            closeInfoButton.onClick.AddListener(ToggleInfoButton);
            
            _healthSystem = _gameManager.Player.gameObject.GetComponent<HealthSystem>();
            _healthSystem.OnDamage += UpdateHealthUi;
            _healthSystem.OnHeal += UpdateHealthUi;
            _healthSystem.OnChangeShieldCount += UpdateShieldUi;
            StatsHandler statsHandler = _gameManager.Player.gameObject.GetComponent<StatsHandler>();
            UpdateStatsUI(statsHandler.CurrentStats);
            statsHandler.OnStatsChanged += UpdateStatsUI;
            _gameManager.OnEquipped += UpdateWeaponUI;
        }

        private void UpdateWeaponUI(WeaponInfo? weaponInfo)
        {
            if (weaponInfo == null) { return; }
            _attack = _gameManager.Player.GetComponentInChildren<BaseAttack>();
            _attack.OnAttackDelayChanged += UpdateDelayUI;
            playerInfo.UpdateWeaponInfoUI(weaponInfo.Value);
        }

        private void UpdateStatsUI(CharacterStats stats) =>
            playerInfo.UpdateStatsUI(stats);

        private void UpdateShieldUi()
        {
        }

        private void UpdateHealthUi()
        {
            float healthPercent = _healthSystem.CurrentHealth / _healthSystem.MaxHealth;
            healthBar.transform.localScale = new Vector3(1, healthPercent);
        }

        private void UpdateDelayUI(float percent)
        {
            atkDelayBar.transform.localScale = new Vector3(1, percent);
        }

        private void Update()
        {
            _playTime += Time.deltaTime;
            elapsedTimeText.text = $"{(int)_playTime / 60:D2}:{(int)_playTime % 60:D2}";
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
            if (_isInfoOpened)
            {
                UpdateStatsUI(_gameManager.PlayerStats);
                if (_gameManager.WeaponInfo.HasValue)
                {
                    UpdateWeaponUI(_gameManager.WeaponInfo.Value);
                }
            }
        }
    }
}