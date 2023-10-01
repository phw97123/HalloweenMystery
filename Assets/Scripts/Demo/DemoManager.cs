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
        private Camera _camera;

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
            gameManager = GameManager.Instance;
        }

        private void Start()
        {
            _camera = Camera.main;
            LoadDemoUI();
            CreatePlayer();
            EnterDungeon();
            IsPlayerExists = true;
        }

        private void Update()
        {
            Vector3 position = gameManager.Player.position;
            _camera.transform.position =
                new Vector3(position.x, position.y, -10f);
        }

        private void LoadDemoUI()
        {
            //todo
            uiManager.ShowUIPopupByName("Demo/DemoUi");
        }


        public void CreatePlayer()
        {
            if (IsPlayerExists) { return; }

            gameManager.CreatePlayer();
            IsPlayerExists = true;
        }

        public void EnterDungeon()
        {
            gameManager.ShowDungeonUI();
            GameObject.Find("Container").transform.localScale = new Vector3(0.5f, 1f);
        }

        public void CreateWeaponParts()
        {
        }
    }
}
#endif