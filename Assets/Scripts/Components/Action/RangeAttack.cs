using Components.Stats;
using Entites;
using Entities;
using System;
using UnityEngine;

namespace Components.Action
{
    public class RangeAttack : MonoBehaviour
    {
        private StatsHandler _stats;
        private EntityController _controller;

        [SerializeField] private Transform spawnPosition;
        [SerializeField] private GameObject testPrefab;
        private Vector2 _direction = Vector2.zero;
        private float _timeSinceLastAttack = float.MaxValue;
        private bool _isAttacking;

        //todo animation 
        private void Awake()
        {
            _stats = GetComponentInParent<StatsHandler>();
            _controller = GetComponentInParent<EntityController>();

            Debug.Assert(_stats != null);
            Debug.Assert(_controller != null);
        }

        private void Start()
        {
            _controller.OnAttackEvent += Attack;
            _controller.OnLookEvent += Aim;
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_timeSinceLastAttack <= _stats.CurrentStats.attackData.delay)
            {
                _isAttacking = false;
                return;
            }


            if (_isAttacking)
            {
                RangeAttackDataSO rangeAttack = _stats.CurrentStats.attackData as RangeAttackDataSO;
                if (rangeAttack == null)
                {
                    Debug.LogWarning("StatHandler's attack data is not type of RangeAttackDataSO");
                    return;
                }

                _isAttacking = false;
                _timeSinceLastAttack = 0f;

                //todo attackManager
                CreateProjectile(
                    startPosition: spawnPosition.position,
                    direction: _direction,
                    rangeAttack
                );
            }
        }

        private void Aim(Vector2 direction) => _direction = direction;

        private void Attack()
        {
            _isAttacking = true;
        }

        private void CreateProjectile(Vector2 startPosition, Vector2 direction, RangeAttackDataSO rangeAttackData)
        {
            float startDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            startDegree -= (rangeAttackData.projectilesPerAttack * rangeAttackData.anglePerShot * 0.5f);

            for (int i = 0; i < rangeAttackData.projectilesPerAttack; i++)
            {
                float currentDegree = startDegree + i * rangeAttackData.anglePerShot;
                float currentRad = currentDegree * Mathf.Deg2Rad;

                Vector2 currentDirection = new Vector2(
                    x: direction.x + Mathf.Cos(currentRad),
                    y: direction.y + Mathf.Sin(currentRad)).normalized;

                GameObject go = Instantiate(
                    testPrefab,
                    startPosition,
                    Quaternion.Euler(0, 0, currentDegree));

                ProjectileController controller = go.GetComponent<ProjectileController>();
                controller.Initialize(startPosition, currentDirection, rangeAttackData);
            }
        }
    }
}