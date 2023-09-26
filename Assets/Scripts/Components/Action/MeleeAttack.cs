using Components.Stats;
using Entites;
using Entities;
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

        //todo animator
        private Vector2 _direction = Vector2.zero;
        private float _timeSinceLastAttack = float.MaxValue;
        private bool _isAttacking = false;
        private int _currentAttackCount = 0;

        private void Awake()
        {
            _controller = GetComponentInParent<EntityController>();
            _stats = GetComponentInParent<StatsHandler>();
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
            //todo divide SwingAttack, LungeAttack
            GameObject go = Instantiate(testPrefab, spawnPosition, Quaternion.identity);
            MeleeAttackController controller = go.GetComponent<MeleeAttackController>();
            if (meleeAttack.arc > 0)
            {
                if (_currentAttackCount % 3 == 0)
                {
                    //Lunge Attack
                    controller.Initialize(
                        meleeAttack: meleeAttack,
                        startPosition: spawnPosition,
                        direction: direction,
                        isReversed: true,
                        isLunge: true);
                }
                else if ((_currentAttackCount & 1) == 1)
                {
                    //reversed direction swing
                    float rad = meleeAttack.arc * Mathf.Deg2Rad;
                    float x = Mathf.Cos(rad);
                    float y = Mathf.Sin(rad);
                    controller.Initialize(
                        meleeAttack: meleeAttack,
                        startPosition: spawnPosition,
                        direction: direction + new Vector2(x, y),
                        isReversed: true);
                }
                else
                {
                    controller.Initialize(meleeAttack, spawnPosition, direction);
                }
            }
            else
            {
                controller.Initialize(meleeAttack, spawnPosition, direction);
            }
        }
    }
}