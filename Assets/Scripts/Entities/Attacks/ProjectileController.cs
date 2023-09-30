using Components.Attacks;
using Managers;
using System;
using UnityEngine;

namespace Entities
{
    public class ProjectileController : AttackController
    {
        private Rigidbody2D _rigidbody;
        private RangeAttackDataSO _rangeAttack;
        private float _distance;
        
        private void Update()
        {
            if (!isReady) { return; }

            CallEvents();
        }

        public void Initialize(Vector2 startPosition, Vector2 direction, float degree, RangeAttackDataSO rangeAttack)
        {
            transform.position = startPosition;
            transform.rotation = Quaternion.Euler(0, 0, degree);
            _rangeAttack = rangeAttack;
            _distance = 0;
            
            AttackStatus status = new AttackStatus()
            {
                attackData = rangeAttack,
                attackTag = rangeAttack.prefabTag,
                attackType = AttackType.Range,
                currentAttackCount = 0,
                degree = degree,
                direction = direction,
                isCritical = false,
                startPosition = startPosition
            };
            Handler.attackStatus = status;
            isReady = true;
        }

        private void FxOnDestroy(bool showFx, AttackManager manager)
        {
            AttackManager.Instance.InactivateGameObject(gameObject, _rangeAttack.bulletTag, showFx);
        }
    }
}