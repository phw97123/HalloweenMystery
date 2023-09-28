using Entites;
using System;
using UnityEngine;

namespace Components.Action
{
    public class WeaponRotation : MonoBehaviour
    {
        private EntityController _controller;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _controller = GetComponentInParent<EntityController>();
            _renderer = GetComponentInParent<SpriteRenderer>();
        }
    }
}