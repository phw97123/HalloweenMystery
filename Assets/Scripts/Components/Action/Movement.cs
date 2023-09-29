using Components.Stats;
using Entites;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Components.Action
{
    public class Movement : MonoBehaviour
    {
        private EntityController _controller;
        private StatsHandler _stats;

        private Vector2 _movementDirection = Vector2.zero; 
        private Rigidbody2D _rigidbody;

        private Vector2 _knockback = Vector2.zero;
        private float knockbackDuration = 0.0f; 

        private void Awake()
        {
            _controller = GetComponent<EntityController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _stats = GetComponent<StatsHandler>();
        }

        private void Start()
        {
            _controller.OnMoveEvent += Move;
        }

        private void FixedUpdate()
        {
            ApplyMovement(_movementDirection);
            if (knockbackDuration > 0.0f)
            {
                knockbackDuration -= Time.fixedDeltaTime; 
            }
        }

        private void Move(Vector2 dir)
        {
            _movementDirection = dir; 
        }

        public void ApplyKnockback(Transform other, float power, float duration)
        {
            knockbackDuration = duration;
            _knockback = -(other.position - transform.position).normalized * power; 
        }

        private void ApplyMovement(Vector2 direction)
        {
            direction = direction * _stats.CurrentStats.speed;

            if (knockbackDuration > 0.0f)
                direction += _knockback;

            _rigidbody.velocity = direction; 
        }
    }
}