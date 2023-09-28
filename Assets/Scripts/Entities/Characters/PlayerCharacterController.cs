using Entites;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    public class PlayerCharacterController : EntityController
    {
        private Camera _camera;

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
            Debug.Log($"dir is {dir}");
            CallLook(dir.normalized);
        }

        public void OnFire(InputValue value)
        {
            if (value.isPressed)
            {
                CallAttack();
            }
        }
    }
}