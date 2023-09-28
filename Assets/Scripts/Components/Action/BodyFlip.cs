using Entites;
using System;
using UnityEngine;

namespace Components.Action
{
    public class BodyFlip : MonoBehaviour
    {
        private EntityController _controller;
        [SerializeField] private SpriteRenderer renderer;

        private void Awake()
        {
            _controller = GetComponent<EntityController>();
            if (renderer == null)
            {
                renderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        private void Start()
        {
            _controller.OnLookEvent += FlipBody;
        }

        private void FlipBody(Vector2 dir)
        {
            float angleRadian = Mathf.Atan2(dir.y, dir.x);
            renderer.flipX = Mathf.Abs(angleRadian) >= Mathf.PI * 0.5f;
        }
    }
}