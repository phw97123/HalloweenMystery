using Entities;
using System;
using UnityEngine;

namespace Components.Attacks
{
    public class SwingAttackMove : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        private SwingAttackController _controller;
        private AttackStatusHandler _handler;
        private MeleeAttackDataSO _meleeAttack;


        private bool _isReversed = false;
        private bool _isStarted = false;
        private Vector2 _startPosition;
        private float _startDegree;
        private float _endDegree;
        private float _currentDegree;
        private float _endTime;
        private float _elapsedTime;

        private void Awake()
        {
            if (pivot == null)
            {
                pivot = GetComponent<Transform>();
            }

            _controller = GetComponent<SwingAttackController>();
            _handler = GetComponent<AttackStatusHandler>();
        }

        private void Start()
        {
            _startPosition = pivot.position;
            _controller.OnSwing += Swing;
            _controller.OnReverseSwing += SwingReversed;
        }

        private void SetupState(bool isReversed)
        {
            _isReversed = isReversed;
            _meleeAttack = _handler.attackStatus.attackData as MeleeAttackDataSO;
            if (_meleeAttack == null) return;
            _elapsedTime = 0f;
            pivot.localScale = new Vector3(pivot.localScale.x * _meleeAttack.range, 0);
            _endTime = _meleeAttack.range * _meleeAttack.arc * Mathf.Deg2Rad / _meleeAttack.speed;
            _startPosition = _handler.attackStatus.startPosition;

            float arc = isReversed ? _meleeAttack.arc : _meleeAttack.arc * -1;

            _startDegree = (_handler.attackStatus.degree + 360 - arc * 0.5f) % 360;
            _endDegree = (_handler.attackStatus.degree + 360 + arc) % 360;
            _currentDegree = _startDegree;
            _isStarted = true;
        }

        private void ApplyMovement()
        {
            _elapsedTime += Time.deltaTime;
            _currentDegree = Mathf.LerpAngle(_startDegree, _endDegree, _elapsedTime / _endTime);
            _handler.attackStatus.degree = _currentDegree;
            float rad = _currentDegree * Mathf.Deg2Rad;
            pivot.transform.position = _startPosition + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            if (_elapsedTime >= _endTime)
            {
                _controller.Inactivate(_handler.attackStatus.attackTag, false);
            }
        }

        private void Swing()
        {
            if (!_isStarted)
            {
                SetupState(false);
            }

            ApplyMovement();
        }

        private void SwingReversed()
        {
            if (!_isStarted)
            {
                SetupState(true);
            }

            ApplyMovement();
        }

        private void OnDisable()
        {
            _isStarted = false;
        }
    }
}