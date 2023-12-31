using Components.Stats;
using Entites;
using Entities;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Components.Action
{
    [Obsolete("Divide into BodyFlip, WeaponRotation")]
    public class ArmRotation : MonoBehaviour
    {
        private EntityController _controller;
        [SerializeField] private Transform armPivot;
        [SerializeField] private SpriteRenderer characterRenderer;
        [SerializeField] private SpriteRenderer weaponRenderer;


        private void Awake()
        {
            _controller = GetComponent<EntityController>();
            weaponRenderer = GetComponentsInChildren<Transform>()
                .First(go => go.gameObject.CompareTag("Weapon"))
                .GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            _controller.OnLookEvent += Look;
        }


        private void Look(Vector2 dir)
        {
            float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            weaponRenderer.flipY = Mathf.Abs(degree) > 90f;
            characterRenderer.flipX = Mathf.Abs(degree) > 90f;
            armPivot.rotation = Quaternion.Euler(0, 0, degree);
        }
    }
}