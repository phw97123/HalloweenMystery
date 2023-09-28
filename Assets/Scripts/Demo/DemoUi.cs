using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class DemoUi : UIPopup
    {
        [SerializeField] private Button createPlayerButton;
        [SerializeField] private Button createWeaponsButton;
        [SerializeField] private Button createMonsterButton;
        [SerializeField] private Button enterDungeonButton;

        private void OnEnable()
        {
            createPlayerButton.onClick.AddListener( DemoManager.Singleton.CreatePlayer);
            createWeaponsButton.onClick.AddListener(DemoManager.Singleton.CreateWeapons);
            createMonsterButton.onClick.AddListener(DemoManager.Singleton.CreateMonsters);
            enterDungeonButton.onClick.AddListener(DemoManager.Singleton.EnterDungeon);
        }

        private void OnDisable()
        {
            createPlayerButton.onClick.RemoveAllListeners();
            createWeaponsButton.onClick.RemoveAllListeners();
            createMonsterButton.onClick.RemoveAllListeners();
            enterDungeonButton.onClick.RemoveAllListeners();
        }
    }
}