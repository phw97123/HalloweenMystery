using Components.Stats;
using Entites;
using System;
using UnityEngine;

namespace Components.Action
{
    public class MeleeAttack : MonoBehaviour
    {
        private EntityController _controller;
        private StatsHandler _stats;
        [SerializeField] private GameObject testPrefab;

        [SerializeField] private Transform attackPosition;

        //todo animatior
        private Vector2 _direction = Vector2.zero;
        private float _timeSinceLastAttack = float.MaxValue;
        private bool _isAttacking = false;

        private void Awake()
        {
            _controller = GetComponent<EntityController>();
            _stats = GetComponent<StatsHandler>();
            Debug.Assert(_controller != null);
            Debug.Assert(_stats != null);
        }

        private void Start()
        {
            _controller.OnAttackEvent += Attack;
            _controller.OnLookEvent += Aim;
        }

        private void Update()
        {
            if (_timeSinceLastAttack <= _stats.CurrentStats.attackData.delay)
            {
                _isAttacking = false;
                return;
            }

            if (_isAttacking)
            {
                MeleeAttackDataSO meleeAttack = _stats.CurrentStats.attackData as MeleeAttackDataSO;
                if (meleeAttack == null)
                {
                    return;
                }

                HandleAttack(meleeAttack, attackPosition.position, _direction);
            }
        }

        private void Attack()
        {
            _isAttacking = true;
        }

        private void Aim(Vector2 direction) => _direction = direction;

        private void HandleAttack(MeleeAttackDataSO meleeAttack, Vector2 spawnPosition, Vector2 direction)
        {
            //todo apply meleeAttack, direction
            Instantiate(testPrefab, spawnPosition, Quaternion.identity);
        }
    }
}