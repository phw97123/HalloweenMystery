using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : CharacterAnimations
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int IsHit = Animator.StringToHash("IsHit");

    private HealthSystem _healthSystem;

    protected override void Awake()
    {
        base.Awake();
        _healthSystem = GetComponent<HealthSystem>(); 
    }

    private void Start()
    {
        controller.OnMoveEvent += Move;

        if (_healthSystem != null)
        {
            _healthSystem.OnDamage += Hit;
            _healthSystem.OnInvincibilityEnd += InvincibilityEnd;
            _healthSystem.OnDeath += Death; 
        }
        else
            Debug.Log("_healthSystem not found"); 
    }

    private void Death()
    {
        animator.SetBool(Dead, true);
        Debug.Log("Dead"); 
    }

    private void Hit()
    {
        animator.SetBool(IsHit, true);
        Debug.Log("Hit");
    }

    private void InvincibilityEnd()
    {
        animator.SetBool(IsHit, false);
        Debug.Log("InvincibilityEnd");
    }

    private void Move(Vector2 obj)
    {
        animator.SetBool(IsWalking, obj.magnitude > .5f);
        Debug.Log("Move");
    }
}
