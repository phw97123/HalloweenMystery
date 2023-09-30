using Entities;
using System;
using UnityEngine;

namespace Components.Attacks
{
    public class LinearAttackMove : MonoBehaviour
    {
        private AttackController _controller;
        private Rigidbody2D _rigidbody;
        private AttackStatusHandler _handler;

        private float _distance;

        private void Awake()
        {
            _controller = GetComponent<AttackController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _handler = GetComponent<AttackStatusHandler>();
        }

        private void Start()
        {
            _controller.OnMoveEvent += Move;
        }

        private void OnEnable()
        {
            _distance = 0f;
        }

        private void Move()
        {
            AttackDataSO attackData = _handler.attackStatus.attackData;
            float maxDistance = _handler.attackStatus.attackData.range;
            if (_distance > 0 && _distance >= maxDistance)
            {
                _controller.Inactivate(_handler.attackStatus.attackTag, false);
            }

            _rigidbody.velocity = _handler.attackStatus.direction * attackData.speed;
            _distance += _rigidbody.velocity.magnitude * Time.deltaTime;
        }

        private void OnDestroy()
        {
            _controller.OnMoveEvent -= Move;
        }
    }
}