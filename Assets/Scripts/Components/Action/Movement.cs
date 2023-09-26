using Components.Stats;
using Entites;
using UnityEngine;

namespace Components.Action
{
    public class Movement : MonoBehaviour
    {
        private EntityController _controller;

        private Rigidbody2D _rigidbody;

        private StatsHandler _stats;

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

        private void Move(Vector2 dir)
        {
            _rigidbody.velocity = dir * _stats.CurrentStats.speed;
        }
    }
}