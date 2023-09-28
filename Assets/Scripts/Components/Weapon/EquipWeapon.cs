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
        private const float ANIMATION_TIME = 5f;
        private InteractController _controller;
        private Transform _weaponPivot;

        private float _currentTime = 0f;

        private void Awake()
        {
            _controller = GetComponent<InteractController>();
            _uiManager = UIManager.Singleton;
            _weaponPivot = transform;
        }

        private void Start()
        {
            _controller.OnInteractionChangeEvent += Interact;
        }

        private void Update()
        {
            RotateWeapon();
        }

        private void RotateWeapon()
        {
            _currentTime += Time.deltaTime;
            float angle = Mathf.Lerp(0, 360, _currentTime / ANIMATION_TIME);
            _weaponPivot.rotation = Quaternion.Euler(0, 0, angle);
            if (_currentTime >= ANIMATION_TIME) { _currentTime = 0f; }
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