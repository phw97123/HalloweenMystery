using System;
using UnityEngine;

namespace Entities
{
    public class MeleeAttackController : MonoBehaviour
    {
        private MeleeAttackDataSO _meleeAttackData;
        private Vector2 _direction;
        private bool _isReady = false;
        private Rigidbody2D _rigidbody;
        private float _distance;

        public event Action<Vector2> OnDestroyEvent;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!_isReady) { return; }

            _rigidbody.velocity = _meleeAttackData.speed * _direction;
            _distance += _rigidbody.velocity.magnitude * Time.deltaTime;

            if (_meleeAttackData.range < _distance)
            {
                Inactivate();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //todo
        }


        public void Initialize(MeleeAttackDataSO meleeAttack, Vector2 direction)
        {
            _isReady = true;
            _distance = 0f;
            _meleeAttackData = meleeAttack;
            _direction = direction;
        }

        private void Inactivate()
        {
            OnDestroyEvent?.Invoke(transform.position);
            Destroy(gameObject);
        }
    }
}