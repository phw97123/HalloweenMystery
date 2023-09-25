using Entites;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private EntityController _controller;

    private Rigidbody2D _rigidbody;

    //todo stats handler
    private float _speed = 5f;

    private void Awake()
    {
        _controller = GetComponent<EntityController>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 dir)
    {
        _rigidbody.velocity = dir * _speed;
    }
}