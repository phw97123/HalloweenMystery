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
        private float _maxDistance;

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
            _maxDistance = _handler.attackStatus.attackData.range;
        }

        private void Move()
        {
            AttackDataSO attackData = _handler.attackStatus.attackData;
            if (_distance >= _maxDistance)
            {
                _controller.Inactivate(attackData.prefabTag, false);
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