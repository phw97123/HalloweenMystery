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
        private GoldSystem _goldSystem;
        private HealthSystem _healthSystem;
        private BaseAttack _attack;

        [SerializeField] private RectTransform container;
        [SerializeField] private Text elapsedTimeText;
        [SerializeField] private Text coinText;

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
            _healthSystem = _gameManager.Player.gameObject.GetComponent<HealthSystem>();
            _healthSystem.OnDamage += UpdateHealthUi;
            _healthSystem.OnHeal += UpdateHealthUi;
            _healthSystem.OnChangeShieldCount += UpdateShieldUi;
            _goldSystem = _gameManager.Player.GetComponent<GoldSystem>();
            _goldSystem.OnChangeOwnedGold += UpdateGoldUI;
            UpdateHealthUi();
            UpdateGoldUI();
        }

        private void UpdateGoldUI()
        {
            coinText.text = _goldSystem.OwnedGold.ToString("N0");
        }


        private void UpdateShieldUi()
        {
        }

        private void UpdateHealthUi()
        {
            float healthPercent = _healthSystem.CurrentHealth / _healthSystem.MaxHealth;
            healthBar.transform.localScale = new Vector3(1, healthPercent);
        }

        public void UpdateDelayUI(float percent)
        {
            atkDelayBar.transform.localScale = new Vector3(1, percent);
        }

        private void Update()
        {
            _playTime += Time.deltaTime;
            elapsedTimeText.text = $"{(int)_playTime / 60:D2}:{(int)_playTime % 60:D2}";
        }
    }
}