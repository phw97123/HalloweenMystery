using Components.Attacks;
using System;

namespace Entities
{
    public class DaggerAttackController : AttackController
    {
        public event Action SuperLungeEvent;

        private AttackStatusHandler _handler;

        //todo add stats coefficient
        private readonly int _superLungePerAttack = 5;

        protected override void Awake()
        {
            base.Awake();
            _handler = GetComponent<AttackStatusHandler>();
        }

        protected override void CallEvents()
        {
            if (_handler.attackStatus.currentAttackCount == _superLungePerAttack)
            {
                SuperLungeEvent?.Invoke();
            }
            else
            {
                CallMove();
            }
        }
    }
}