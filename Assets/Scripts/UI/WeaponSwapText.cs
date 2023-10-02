using System;
using UnityEngine;

namespace UI
{
    public class WeaponSwapText : UIPopup
    {
        private Vector2 _startPosition;
        private float _currentY = 0f;
        private int _dir = 1;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            _currentY += Time.deltaTime * _dir;
            if (_currentY >= 0.25f)
            {
                _dir = -1;
            }
            else if (_currentY <= -0.25f)
            {
                _dir = 1;
            }

            transform.position = new Vector3(_startPosition.x, _startPosition.y + _currentY);
        }
    }
}