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
        public event Action OnLastAttack;

        protected override void CallEvents()
        {
            MeleeAttackDataSO meleeAttack = Handler.attackStatus.attackData as MeleeAttackDataSO;
            if (meleeAttack == null) { return; }

            CallRotateAttack(Handler.attackStatus.degree);
            if (Handler.attackStatus.currentAttackCount == (int)meleeAttack.attackCount)
            {
                OnLastAttack?.Invoke();
            }
            else if ((Handler.attackStatus.currentAttackCount & 1) == 1)
            {
                //odd
                OnSwing?.Invoke();
            }
            else
            {
                OnReverseSwing?.Invoke();
            }
        }
    }
}