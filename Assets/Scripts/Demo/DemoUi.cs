#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public enum DemoState
    {
        Basic,
        CreateWeapon,
        CreateMonsters,
        Dungeon,
        CreateParts
    }

    public class DemoUi : UIPopup
    {
        [SerializeField] private Button createPlayerButton;
        [SerializeField] private Button createWeaponsButton;
        [SerializeField] private Button createMonsterButton;
        [SerializeField] private Button enterDungeonButton;
        [SerializeField] private Button partsButton;


        private bool _isReady;
        private DemoState _currentState;
        public DemoState CurrentState => _currentState;
        private DemoState NewState => _stack.Peek();
        private readonly Stack<DemoState> _stack = new Stack<DemoState>();

        public event Action<DemoState> OnStateChanged;

        private void Start()
        {
            _stack.Push(DemoState.Basic);
            UpdateUi();
        }


        private void Update()
        {
            if (!_isReady)
            {
                _isReady = true;
                UpdateUi();
                return;
            }

            if (CurrentState == NewState) { return; }

            UpdateUi();
        }


        private void OnEnable()
        {
            createPlayerButton.onClick.AddListener(DemoManager.Singleton.CreatePlayer);
            createWeaponsButton.onClick.AddListener(() => { _stack.Push(DemoState.CreateWeapon); });
            createMonsterButton.onClick.AddListener(DemoManager.Singleton.CreateMonsters);
            enterDungeonButton.onClick.AddListener(DemoManager.Singleton.EnterDungeon);
            partsButton.onClick.AddListener((() => { _stack.Push(DemoState.CreateParts); }));
        }

        private void OnDisable()
        {
            createPlayerButton.onClick.RemoveAllListeners();
            createWeaponsButton.onClick.RemoveAllListeners();
            createMonsterButton.onClick.RemoveAllListeners();
            enterDungeonButton.onClick.RemoveAllListeners();
        }

        private void UpdateUi()
        {
            if (_stack.Count <= 0) { return; }

            _currentState = NewState;  
            OnStateChanged?.Invoke(_currentState);
        }

        public void BackPressed()
        {
            _stack.Pop();
        }
    }
}
#endif