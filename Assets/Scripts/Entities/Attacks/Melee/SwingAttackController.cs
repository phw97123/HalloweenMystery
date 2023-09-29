using Components.Attacks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class SwingAttackController : AttackController
    {
        public event Action OnSwing;
        public event Action OnReverseSwing;
        private TrailRenderer _trailRenderer;

        protected override void Awake()
        {
            base.Awake();
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        
        protected override void CallEvents()
        {
            MeleeAttackDataSO meleeAttack = Handler.attackStatus.attackData as MeleeAttackDataSO;
            if (meleeAttack == null) { return; }
            
            
            _trailRenderer.startWidth = Handler.attackStatus.attackData.range;
            CallRotateAttack(Handler.attackStatus.degree);
            if ((Handler.attackStatus.currentAttackCount & 1) == 1)
            {
                //odd
                OnSwing?.Invoke();
            }
            else
            {
                //even
                OnReverseSwing?.Invoke();
            }
        }
    }
}