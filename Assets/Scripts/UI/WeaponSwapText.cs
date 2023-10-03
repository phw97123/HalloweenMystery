using System;
using UnityEngine;

namespace UI
{
    public class WeaponSwapText : UIPopup
    {
        private Vector2 _startPosition;
        [SerializeField] private float time = 0.5f;
        [SerializeField] private float speed = 0.5f;
        private float _lastTime = 0f;
        private float _dy;
        private int _dir = 1;

        private void Update()
        {
            _dy = Time.deltaTime * speed * _dir;
            _lastTime += Time.deltaTime;
            if (_lastTime >= time)
            {
                _dir *= -1;
                _lastTime = 0f;
            }

            transform.position += new Vector3(0, _dy);
        }
    }
}