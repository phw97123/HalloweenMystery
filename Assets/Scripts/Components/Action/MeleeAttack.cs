using Components.Stats;
using Entites;
using Entities;
using Managers;
using System;
using UnityEngine;
using Utils;

namespace Components.Action
{
    public class MeleeAttack : MonoBehaviour
    {
        private EntityController _controller;
        private StatsHandler _stats;
        [SerializeField] private Transform attackPosition;

        //todo animator
        private Vector2 _direction = Vector2.zero;
        private float _timeSinceLastAttack = float.MaxValue;
        private bool _isAttacking = false;
        private int _currentAttackCount = 0;

        private AttackManager _attackManager;

        private void Awake()
        {
            _attackManager = AttackManager.Instance;
            _controller = GetComponentInParent<EntityController>();
            _stats = GetComponentInParent<StatsHandler>();
        }

        private void Start()
        {
            if (_controller == null) { return; }

            _controller.OnAttackEvent += Attack;
            _controller.OnLookEvent += Aim;
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_isAttacking)
            {
                MeleeAttackDataSO meleeAttack = _stats.CurrentStats.attackData as MeleeAttackDataSO;
                if (meleeAttack == null)
                {
                    return;
                }

                _timeSinceLastAttack = 0f;
                _isAttacking = false;
                HandleAttack(meleeAttack, attackPosition.position, _direction);
            }
        }

        private void Attack()
        {
            MeleeAttackDataSO meleeAttack = _stats.CurrentStats.attackData as MeleeAttackDataSO;
            if (meleeAttack == null) return;


            if (_timeSinceLastAttack >= meleeAttack.delay)
            {
                _currentAttackCount = 1;
                _isAttacking = true;
            }
            else if (_currentAttackCount < meleeAttack.attackCount)
            {
                _isAttacking = true;
                _currentAttackCount++;
            }
        }

        private void Aim(Vector2 direction) => _direction = direction;

        private void HandleAttack(MeleeAttackDataSO meleeAttack, Vector2 spawnPosition, Vector2 direction)
        {
            //todo attackManager
            float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            string prefabTag = GetCurrentAttackTag(meleeAttack);
            _attackManager.MeleeAttack(
                prefabTag,
                spawnPosition,
                direction,
                _currentAttackCount,
                degree,
                meleeAttack);
        }

        private string GetCurrentAttackTag(MeleeAttackDataSO meleeAttack)
        {
            if ((int)meleeAttack.attackCount <= _currentAttackCount)
            {
                return meleeAttack.prefabTag;
            }

            if (meleeAttack.arc <= 0)
            {
                return Constants.LUNGE_TAG;
            }


            return Constants.SWING_TAG;
        }
    }
}