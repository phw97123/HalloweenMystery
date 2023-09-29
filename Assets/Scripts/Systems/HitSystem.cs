using Components.Attacks;
using Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class HitSystem : MonoBehaviour
    {
        [SerializeField] private LayerMask levelCollision;
        private readonly HashSet<int> _hitInstanceIdSet = new HashSet<int>();
        private AttackStatusHandler _handler;
        private AttackController _controller;

        private int _maxTargetCount = 1;

        private void Awake()
        {
            _handler = GetComponent<AttackStatusHandler>();
            _controller = GetComponent<AttackController>();
        }

        private void Start()
        {
            if (_handler.attackStatus.attackData == null)
            {
                return;
            }

            switch (_handler.attackStatus.attackType)
            {
                case AttackType.Melee:
                    _maxTargetCount = ((MeleeAttackDataSO)_handler.attackStatus.attackData).targets;
                    break;
                case AttackType.Range:
                    _maxTargetCount = (int)((RangeAttackDataSO)_handler.attackStatus.attackData).piercingCount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (levelCollision.value == (levelCollision.value | (1 << other.gameObject.layer)))
            {
                Debug.Log("trigger enter to level collision");
                _controller.Inactivate(_handler.attackStatus.attackData.prefabTag, false);
            }

            if (_hitInstanceIdSet.Contains(other.GetInstanceID()) ||
                _hitInstanceIdSet.Count >= _maxTargetCount ||
                _handler.attackStatus.attackData.target.value !=
                (_handler.attackStatus.attackData.target.value | (1 << other.gameObject.layer))
               ) { return; }

            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem == null) { return; }

            if (healthSystem.ChangeHealth(-_handler.attackStatus.attackData.damage))
            {
                _hitInstanceIdSet.Add(other.GetInstanceID());
                if (_handler.attackStatus.attackType == AttackType.Range && _maxTargetCount <= _hitInstanceIdSet.Count)
                {
                    _controller.Inactivate(_handler.attackStatus.attackData.prefabTag, false);
                }
            }
        }
    }
}