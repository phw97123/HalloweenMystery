using Entities;
using Managers;
using System;
using UI;
using UnityEngine;

namespace Components.Weapon
{
    public class EquipWeapon : MonoBehaviour
    {
        private UIManager _uiManager;
        private InteractController _controller;

        private float _currentTime = 0f;

        private void Awake()
        {
            _controller = GetComponent<InteractController>();
            _uiManager = UIManager.Singleton;
        }

        private void Start()
        {
            _controller.OnInteractionChangeEvent += Interact;
        }
        
        private void Interact(bool isEnter)
        {
            if (isEnter)
            {
                UIPopup popup = _uiManager.ShowUIPopupByName(nameof(WeaponSwapText));
                popup.gameObject.GetComponentInChildren<RectTransform>().position = transform.position + new Vector3(0, 1);
            }
            else
            {
                _uiManager.ClosePopup(nameof(WeaponSwapText));
            }
        }
    }
}