#if (UNITY_EDITOR)
using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Managers
{
    public class DemoManager : MonoBehaviour
    {
        private static DemoManager _singleton;
        public UIManager uiManager;
        public GameManager gameManager;
        public bool IsPlayerExists { get; private set; } = false;

        public static DemoManager Singleton
        {
            get
            {
                if (_singleton != null)
                {
                    return _singleton;
                }

                _singleton = FindObjectOfType<DemoManager>();
                if (_singleton != null) { return _singleton; }

                GameObject go = new GameObject(nameof(DemoManager));
                _singleton = go.AddComponent<DemoManager>();
                return _singleton;
            }
        }


        private void Awake()
        {
            if (_singleton != null && _singleton != this)
            {
                Destroy(gameObject);
            }

            _singleton = this;
            uiManager = UIManager.Singleton;
        }

        private void Start()
        {
            LoadDemoUI();
        }

        private void LoadDemoUI()
        {
            //todo
            uiManager.ShowUIPopupByName("DemoUi");
        }


        public void CreatePlayer()
        {
            if (IsPlayerExists) { return; }

            Object go = Resources.Load("Prefabs/Character/BlueMan");
            Instantiate(go);
            IsPlayerExists = true;
        }

        public void CreateWeapons(bool[] isAble)
        {
            Vector2 position = new Vector2(-3, -3);
            Vector2 spacing = new Vector2(2, 0);
            WeaponManager.Singleton.CreateInteractableWeapons(isAble, position, spacing);
        }

        public void CreateMonsters()
        {
            Object go = Resources.Load("Prefabs/Enemies/Ghost_White");
            Instantiate(go);
        }

        public void EnterDungeon()
        {
            //todo 콜로세움
        }
    }
}
#endif