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
            Debug.Log($"screenPosition is {screenPoint}");
            Vector3 worldPoint = _camera.ScreenToWorldPoint(screenPoint).normalized;
            Debug.Log($"worldPoint is {worldPoint}");
            Vector2 dir = worldPoint - transform.position;
            Debug.Log($"dir is {dir}");
            CallLook(dir);
        }

        public void OnAttack(InputValue value)
        {
            //todo 
        }
    }
}