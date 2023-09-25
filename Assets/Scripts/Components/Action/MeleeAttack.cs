using Components.Stats;
using Entites;
using System;
using UnityEngine;

namespace Components.Action
{
    public class MeleeAttack : MonoBehaviour
    {
        private EntityController _controller;
        private StatsHandler _stats;
        
        private void Awake()
        {
            _controller = GetComponent<EntityController>();
            _stats = GetComponent<StatsHandler>();
            Debug.Assert(_controller != null);
            Debug.Assert(_stats != null);
        }

        private void Start()
        {
            _controller.OnAttackEvent += Attack;
        }

        private void Attack()
        {
        }
    }
}