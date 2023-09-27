using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class MeleeAttackController : MonoBehaviour
    {
        [SerializeField] private LayerMask levelCollision;
        private MeleeAttackDataSO _meleeAttackData;
        private Vector2 _startPosition;
        private Vector2 _direction;
        private bool _isReady = false;
        private Rigidbody2D _rigidbody;
        private TrailRenderer _trailRenderer;

        private float _distance;

        //todo hit once per attack
        private Dictionary<int, bool> _hitTargets = new Dictionary<int, bool>();

        //todo refactor swing, lunge to encapsulate  ??? 
        private float _currentDegree;
        private float _movedDegree;
        private bool _isReversed;
        private bool _isLunge;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void Update()
        {
            if (!_isReady) { return; }

            HandleMovement();
        }

        private void HandleMovement()
        {
            if (_meleeAttackData.arc <= 0f || _isLunge)
            {
                Lunge();
            }
            else
            {
                Swing();
            }
        }

        private void Lunge()
        {
            _rigidbody.velocity = _meleeAttackData.speed * _direction;
            _distance += _rigidbody.velocity.magnitude * Time.deltaTime;

            if (_meleeAttackData.range < _distance)
            {
                Inactivate();
            }
        }

        private void Swing()
        {
            float rad = _currentDegree * Mathf.Deg2Rad;
            Transform attackTransform = transform;
            attackTransform.position = new Vector3(
                x: _startPosition.x + Mathf.Cos(rad),
                y: _startPosition.y + Mathf.Sin(rad));
            _currentDegree += _meleeAttackData.arc * _meleeAttackData.speed * Time.deltaTime * (_isReversed ? -1 : 1);
            _movedDegree += _meleeAttackData.arc * _meleeAttackData.speed * Time.deltaTime;
            attackTransform.rotation = Quaternion.Euler(0, 0, _currentDegree);

            if (_movedDegree >= _meleeAttackData.arc)
            {
                Inactivate();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_meleeAttackData.target.value == (_meleeAttackData.target.value & 1 << other.gameObject.layer))
            {
                _hitTargets.TryGetValue(other.GetInstanceID(), out bool hasBeenHit);
                if (hasBeenHit)
                {
                    return;
                }
            }
            else if (levelCollision.value == (_meleeAttackData.target.value & 1 << other.gameObject.layer))
            {
            }
        }


        public void Initialize(
            MeleeAttackDataSO meleeAttack,
            Vector2 startPosition,
            Vector2 direction,
            bool isReversed = false,
            bool isLunge = false)
        {
            _isReady = true;
            _distance = 0f;
            _meleeAttackData = meleeAttack;
            _direction = direction.normalized;
            _startPosition = startPosition;
            _movedDegree = 0f;
            _isLunge = isLunge;
            _isReversed = isReversed;
            
            if (meleeAttack.arc > 0 && !isLunge)
            {
                float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _currentDegree = degree + -meleeAttack.arc * 0.5f;
                transform.rotation = Quaternion.Euler(0, 0, _currentDegree);
            }

            gameObject.SetActive(true);
        }

        private void Inactivate()
        {
            _isReady = false;
            gameObject.SetActive(false);
            _trailRenderer.Clear();
        }
    }
}