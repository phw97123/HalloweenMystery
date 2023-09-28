using Entities;
using System;
using UnityEngine;

namespace Components.Attacks
{
    public class AttackRotation : MonoBehaviour
    {
        //todo attack object rotate to direction for trail renderer
        private AttackController _controller;

        private void Awake()
        {
            _controller = GetComponent<AttackController>();
        }

        private void Start()
        {
            _controller.OnRotationChanged += RotateTransform;
        }

        private void RotateTransform(float degree)
        {
            transform.rotation = Quaternion.Euler(0, 0, degree);
        }
    }
}