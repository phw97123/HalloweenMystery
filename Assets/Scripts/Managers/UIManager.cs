using JetBrains.Annotations;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    public class UIManager
    {
        private static UIManager _singleton;
        private Dictionary<string, UIPopup> _popUpList = new Dictionary<string, UIPopup>();

        public static UIManager Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new UIManager();
                }

                return _singleton;
            }
        }

        private UIManager()
        {
        }

        public UIPopup ShowUIPopupByName(string name)
        {
            GameObject obj = Resources.Load<GameObject>($"Prefabs/UI/{name}");
            if (obj == null)
            {
                Debug.LogWarning($"Load asset \"{name}\" is not exists");
            }

            return ShowUIPopupWithPrefab(obj, name);
        }

        public UIPopup ShowUIPopupWithPrefab(GameObject prefab, string name)
        {
            if (_popUpList.TryGetValue(name, out UIPopup popup))
            {
                if (popup == null)
                {
                    popup = Object.Instantiate(prefab).GetComponent<UIPopup>();
                    _popUpList[name] = popup;
                }

                popup.gameObject.SetActive(true);
                return popup;
            }

            GameObject go = Object.Instantiate(prefab);
            popup = go.GetComponent<UIPopup>();
            _popUpList[name] = popup;
            return popup;
        }

        public void ClosePopup(string name)
        {
            _popUpList[name]?.gameObject.SetActive(false);
        }

        public void CloseAllPopups()
        {
            foreach ((string _, UIPopup popup) in _popUpList)
            {
                if (popup != null)
                {
                    popup.gameObject.SetActive(false);
                }
            }
        }

        [CanBeNull]
        public UIPopup FindPopup(string name)
        {
            return _popUpList.TryGetValue(name, out UIPopup popup) ? popup : null;
        }
    }
}