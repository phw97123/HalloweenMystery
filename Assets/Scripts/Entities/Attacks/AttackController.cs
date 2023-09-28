using Components.Attacks;
using Managers;
using System;
using UnityEngine;

namespace Entities
{
    public class AttackController : MonoBehaviour
    {
        [SerializeField] private LayerMask levelCollision;
        [SerializeField] protected bool isReady = false;

        private AttackManager _attackManager;
        private TrailRenderer _trailRenderer;
        private string _prefabTag;

        protected AttackStatusHandler Handler;
        public event Action OnMoveEvent;
        public event Action<float> OnRotationChanged;
        public event Action<Collider2D> OnTriggerEnterEvent;

        protected virtual void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
            _attackManager = AttackManager.Instance;
            Handler = GetComponent<AttackStatusHandler>();
        }

        protected virtual void Update()
        {
            if (!isReady) { return; }

            CallEvents();
        }

        protected virtual void CallEvents()
        {
            CallMove();
        }

        protected void CallMove()
        {
            OnMoveEvent?.Invoke();
        }

        public virtual void Initialize(
            Vector2 startPosition,
            Vector2 direction,
            float degree,
            int currentAttackCount,
            bool isCritical,
            AttackDataSO attackData)
        {
            CallRotateAttack(degree);
            _prefabTag = attackData.prefabTag;
            transform.position = startPosition;
            AttackStatus status = new AttackStatus(
                startPosition: startPosition,
                currentAttackCount: currentAttackCount,
                isCritical: isCritical,
                direction: direction,
                degree: degree,
                attackData: attackData
            );
            Handler.SetStatus(status, this);
            isReady = true;
        }

        protected void CallRotateAttack(float degree)
        {
            OnRotationChanged?.Invoke(degree);
        }

        public virtual void Inactivate(string prefabTag, bool showFx)
        {
            isReady = false;
            gameObject.SetActive(false);
            _trailRenderer.Clear();
            _attackManager.InactivateGameObject(gameObject, prefabTag, showFx);
        }
    }
}