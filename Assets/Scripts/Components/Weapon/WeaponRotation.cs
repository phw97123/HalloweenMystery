using Entites;
using System;
using System.Linq;
using UnityEngine;
using Utils;

namespace Components.Action
{
    public class WeaponRotation : MonoBehaviour
    {
        private EntityController _controller;
        private Transform _armPivot;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _armPivot = transform;
            _controller = GetComponentInParent<EntityController>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            if (_controller == null) { return; }

            _controller.OnLookEvent += RotateWeapon;
        }

        private void OnDestroy()
        {
            if (_controller != null)
            {
                _controller.OnLookEvent -= RotateWeapon;                
            }
            
        }

        private void RotateWeapon(Vector2 dir)
        {
            float angleDegree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _armPivot.rotation = Quaternion.Euler(0, 0, angleDegree);
            _renderer.flipY = Mathf.Abs(angleDegree) >= 90f;
        }
    }
}