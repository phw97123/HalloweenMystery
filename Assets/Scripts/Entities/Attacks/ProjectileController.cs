using Managers;
using System;
using UnityEngine;

namespace Entities
{
    public class ProjectileController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private TrailRenderer _trailRenderer;
        private RangeAttackDataSO _rangeAttack;
        private Vector2 _direction;
        private bool _isReady;
        private Vector2 _startPosition;
        private float _distance;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void Start()
        {
            _trailRenderer.Clear();
        }

        private void Update()
        {
            if (!_isReady) { return; }

            _rigidbody.velocity = _direction * _rangeAttack.speed;
            //todo lifecycle system
            _distance += _rigidbody.velocity.magnitude * Time.deltaTime;

            if (_distance >= _rangeAttack.range)
            {
                FxOnDestroy(true, AttackManager.Instance);
            }
        }

        public void Initialize(Vector2 startPosition, Vector2 direction, float degree, RangeAttackDataSO rangeAttack)
        {
            transform.position = startPosition;
            transform.rotation = Quaternion.Euler(0, 0, degree);
            _startPosition = startPosition;
            _isReady = true;
            _direction = direction;
            _rangeAttack = rangeAttack;
            _distance = 0;
        }

        private void FxOnDestroy(bool showFx, AttackManager manager)
        {
            _trailRenderer.Clear();
            AttackManager.Instance.InactivateGameObject(gameObject, _rangeAttack.bulletTag, showFx);
        }
    }
}