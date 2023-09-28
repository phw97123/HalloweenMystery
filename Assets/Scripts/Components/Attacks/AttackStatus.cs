using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace Components.Attacks
{
    public enum AttackType
    {
        Melee,
        Range,
    }

    [Serializable]
    public struct AttackStatus
    {
        public Vector2 startPosition;
        public AttackType attackType;
        public int currentAttackCount;
        public bool isCritical;
        public Vector2 direction;
        public float degree;

        public AttackDataSO attackData;

        public AttackStatus(int currentAttackCount, bool isCritical, Vector2 startPosition, Vector2 direction,
            float degree,
            AttackDataSO attackData)
        {
            this.currentAttackCount = currentAttackCount;
            this.isCritical = isCritical;
            this.direction = direction;
            this.degree = degree;
            this.attackData = attackData;
            this.startPosition = startPosition;
            
            if (attackData is MeleeAttackDataSO)
            {
                attackType = AttackType.Melee;
            }
            else if (attackData is RangeAttackDataSO)
            {
                attackType = AttackType.Range;
            }
            else
            {
                throw new NotImplementedException($"Not handle case : {attackData.GetType()}");
            }
        }
    }
}