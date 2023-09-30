using System;

namespace Entities
{
    public class SwingAttackController : AttackController
    {
        public event Action OnSwing;
        public event Action OnReverseSwing;
        
        protected override void CallEvents()
        {
            MeleeAttackDataSO meleeAttack = Handler.attackStatus.attackData as MeleeAttackDataSO;
            if (meleeAttack == null) { return; }
            
            TrailRenderer.startWidth = Handler.attackStatus.attackData.range;
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