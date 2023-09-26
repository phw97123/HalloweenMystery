using System;
using UnityEngine;

namespace Components
{
    public enum StatsChangeType
    {
        Override,
        Add,
        Multiply
    }

    [Serializable]
    public class CharacterStats
    {
        public StatsChangeType changeType;
        [Range(0, 20_000f)] public float maxHealth;
        [Range(0, 30f)] public float speed;
        [Range(0, 1f)] public float criticalPercentage;
        [Range(0, 50f)] public float goldPercentage;
        [Range(0, 50f)] public float itemDropPercentage;
        [Range(0, 5f)] public float buffDurationIncrease;

        
        public AttackDataSO attackData;
    }
}