using Components.Stats;
using Entites;
using Entities;
using Managers;
using System;
using UnityEngine;
using Utils;

namespace Components.Action
{
    public class MeleeAttack : BaseAttack
    {
        private EntityController _controller;
        private StatsHandler _stats;
        [SerializeField] private Transform attackPosition;

        // todo animator
        private Vector2 _direction = Vector2.zero;
        private float _timeSinceLastAttack = float.MaxValue;
        private bool _isAttacking = false;
        private int _currentAttackCount = 0;

        private AttackManager _attackManager;

        public AudioClip meleeClip1;
        public AudioClip meleeClip2;

        public GameObject particleEffectPrefab;

        private GameObject currentParticleEffect;
        public Transform particleTransform;

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
            float percent = GetAttackDelayPercent();
            CallAttackDelayChange(percent);

            if (_isAttacking)
            {
                MeleeAttackDataSO meleeAttack = _stats.CurrentStats.attackData as MeleeAttackDataSO;
                if (meleeAttack == null)
                {
                    return;
                }

                _timeSinceLastAttack = 0f;
                _isAttacking = false;

                Vector3 spawnPosition = particleTransform.position;
                HandleAttack(meleeAttack, attackPosition.position, _direction);

                PlayParticleEffect(spawnPosition);
            }
        }

        private float GetAttackDelayPercent()
        {
            MeleeAttackDataSO meleeAttack = _stats.CurrentStats.attackData as MeleeAttackDataSO;
            if (meleeAttack == null) { return 0f; }

            if (_timeSinceLastAttack > meleeAttack.delay || _currentAttackCount < meleeAttack.attackCount)
            {
                return 1f;
            }

            return _timeSinceLastAttack / meleeAttack.delay;
        }

        private void Attack()
        {
            MeleeAttackDataSO meleeAttack = _stats.CurrentStats.attackData as MeleeAttackDataSO;
            if (meleeAttack == null) return;

            if (_timeSinceLastAttack >= meleeAttack.delay)
            {
                _currentAttackCount = 1;
                _isAttacking = true;

                if (meleeClip1)
                    SoundManager.PlayClip(meleeClip1);
            }
            else if (_currentAttackCount < meleeAttack.attackCount)
            {
                _isAttacking = true;
                _currentAttackCount++;

                if (meleeClip2)
                    SoundManager.PlayClip(meleeClip2);
            }
        }

        private void Aim(Vector2 direction) => _direction = direction.normalized;

        private void HandleAttack(MeleeAttackDataSO meleeAttack, Vector2 spawnPosition, Vector2 direction)
        {
            // todo attackManager
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
            // if ((int)meleeAttack.attackCount <= _currentAttackCount)
            // {
            //     return meleeAttack.prefabTag;
            // }

            if (meleeAttack.arc <= 0)
            {
                return Constants.LUNGE_TAG;
            }

            return Constants.SWING_TAG;
        }


        private void PlayParticleEffect(Vector3 spawnPosition)
        {
            if (particleEffectPrefab != null)
            {
                // 파티클 시스템 Prefab을 Instantiate하고 원하는 위치로 설정
                currentParticleEffect = Instantiate(particleEffectPrefab, spawnPosition, Quaternion.identity);

                // 파티클 시스템을 재생
                ParticleSystem particleSystem = currentParticleEffect.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
        }

        private void LateUpdate()
        {
            if (currentParticleEffect != null)
            {
                ParticleSystem particleSystem = currentParticleEffect.GetComponent<ParticleSystem>();
                if (particleSystem != null && !particleSystem.isPlaying)
                {
                    Destroy(currentParticleEffect);
                }
            }
        }
    }
}
