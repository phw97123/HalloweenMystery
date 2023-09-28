using Entites;
using Entities;
using Managers;
using System;
using UnityEngine;

namespace Components.Action
{
    public class Interaction : MonoBehaviour
    {
        private PlayerCharacterController _controller;
        private WeaponManager _weaponManager;
        public GameObject currentInteract;

        private void Awake()
        {
            _weaponManager = WeaponManager.Singleton;
            _controller = GetComponent<PlayerCharacterController>();
        }

        private void Start()
        {
            _controller.OnInteractionEvent += Interact;
        }

        private void Interact()
        {
            if (currentInteract == null) { return; }
            
            //todo compare tag to switch interact logic
            Debug.Log($"interact with {currentInteract.name}");
            _weaponManager.EquipWeapon(currentInteract, gameObject);
        }
    }
}