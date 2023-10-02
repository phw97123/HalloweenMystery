using Entites;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
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

        private Vector2 _screenPoint;
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
            _screenPoint = screenPoint;
            Vector3 worldPoint = _camera.ScreenToWorldPoint(screenPoint);
            Vector2 dir = (worldPoint - transform.position);
            CallLook(dir);
        }

        public void OnFire(InputValue value)
        {
            if (value.isPressed && !IsMouseOverUIWithIgnores())
            {
                CallAttack();
            }
        }

        private bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool IsMouseOverUIWithIgnores()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = _screenPoint;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.GetComponent<IgnoreEventSystemPointerOverUI>() != null)
                {
                    raycastResults.RemoveAt(i);
                    i--;
                }
            }

            return raycastResults.Count > 0;
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