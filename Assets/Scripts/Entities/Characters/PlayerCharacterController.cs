using Entites;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    public enum CharacterType
    {
        BlueMan,
        BlueGirl,
        RedMan,
        RedGirl,
        Ghost,
    }

    public class PlayerCharacterController : EntityController
    {
        private Camera _camera;

        public event Action OnInteractionEvent;
        public event Action OnInteractionItemPartsEvent;

        protected void CallInteraction()
        {
            OnInteractionEvent?.Invoke();
        }

        protected void CallInteractionItemParts()
        {
            OnInteractionItemPartsEvent?.Invoke();
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void OnMove(InputValue input)
        {
            Vector2 dir = input.Get<Vector2>();
            CallMove(dir.normalized);
        }

        public void OnLook(InputValue input)
        {
            Vector2 screenPoint = input.Get<Vector2>();
            Vector3 worldPoint = _camera.ScreenToWorldPoint(screenPoint);
            Vector2 dir = (worldPoint - transform.position).normalized;
            CallLook(dir.normalized);
        }

        public void OnFire(InputValue value)
        {
            if (value.isPressed)
            {
                CallAttack();
            }
        }

        public void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                CallInteraction();
            }
        }

        public void OnInteractItemParts(InputValue value)
        {
            if (value.isPressed)
            {
                CallInteractionItemParts();
            }
        }
    }
}