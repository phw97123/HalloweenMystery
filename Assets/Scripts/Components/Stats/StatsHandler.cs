using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components.Stats
{
    public class StatsHandler : MonoBehaviour
    {
        private const float MinAttackDelay = 0.03f;
        private const float MinAttackSpeed = .1f;

        private const float MinSpeed = 0.8f;
        private const int MinMaxHealth = 5;
        public CharacterStats CurrentStats { get; private set; }

        [SerializeField] private CharacterStats baseStats;
        private List<CharacterStats> _statsModifiers = new List<CharacterStats>();

        private void Awake()
        {
            UpdateCharacterStats();
        }
        public void AddStatModifier(CharacterStats statModifier)
        {
            _statsModifiers.Add(statModifier);
            UpdateCharacterStats();
        }

        public void RemoveStatModifier(CharacterStats statModifier)
        {
            _statsModifiers.Remove(statModifier);
            UpdateCharacterStats();
        }

        private void UpdateCharacterStats()
        {
            AttackDataSO attackSO = null;
            if (baseStats != null)
            {
                attackSO = Instantiate(baseStats.attackData);
            }

            CurrentStats = new CharacterStats { attackData = attackSO };
            UpdateStats((a, b) => b, baseStats);
            if (CurrentStats.attackData != null)
            {
                CurrentStats.attackData.target = baseStats.attackData.target;
            }

            foreach (CharacterStats modifier in _statsModifiers.OrderBy(o => o.changeType))
            {
                if (modifier.changeType == StatsChangeType.Override)
                {
                    UpdateStats((o, o1) => o1, modifier);
                }
                else if (modifier.changeType == StatsChangeType.Add)
                {
                    UpdateStats((o, o1) => o + o1, modifier);
                }
                else if (modifier.changeType == StatsChangeType.Multiply)
                {
                    UpdateStats((o, o1) => o * (1.0f + o1), modifier);
                }
            }

            LimitAllStats();
        }

        private void UpdateStats(Func<float, float, float> operation, CharacterStats newModifier)
        {
            CurrentStats.maxHealth = Mathf.CeilToInt(operation(CurrentStats.maxHealth, newModifier.maxHealth));
            CurrentStats.speed = operation(CurrentStats.speed, newModifier.speed);
            CurrentStats.criticalPercentage = operation(CurrentStats.criticalPercentage, newModifier.criticalPercentage);
            CurrentStats.goldPercentage = operation(CurrentStats.goldPercentage, newModifier.goldPercentage);
            CurrentStats.itemDropPercentage = operation(CurrentStats.itemDropPercentage, newModifier.itemDropPercentage);
            CurrentStats.buffDurationIncrease = operation(CurrentStats.buffDurationIncrease, newModifier.buffDurationIncrease);

            if (CurrentStats.attackData == null || newModifier.attackData == null)
                return;

            UpdateAttackStats(operation, CurrentStats.attackData, newModifier.attackData);

            if (CurrentStats.attackData.GetType() != newModifier.attackData.GetType())
            {
                return;
            }

            switch (CurrentStats.attackData)
            {
                case MeleeAttackDataSO _:
                    ApplyMeleeStats(operation, newModifier);
                    break;
                case RangeAttackDataSO _:
                    ApplyRangeStats(operation, newModifier);
                    break;
            }
        }

        private void UpdateAttackStats(Func<float, float, float> operation, AttackDataSO currentAttack, AttackDataSO newAttack)
        {
            if (currentAttack == null || newAttack == null)
            {
                return;
            }

            currentAttack.damage = Mathf.CeilToInt(operation(currentAttack.damage, currentAttack.damage));
            currentAttack.delay = operation(currentAttack.delay, newAttack.delay);
            currentAttack.speed = operation(currentAttack.speed, newAttack.speed);
            currentAttack.range = operation(currentAttack.range, newAttack.range);
        }

        private void ApplyMeleeStats(Func<float, float, float> operation, CharacterStats newModifier)
        {
            MeleeAttackDataSO currentRangedAttacks = (MeleeAttackDataSO)CurrentStats.attackData;

            if (!(newModifier.attackData is MeleeAttackDataSO))
            {
                return;
            }

            MeleeAttackDataSO rangedAttacksModifier = (MeleeAttackDataSO)newModifier.attackData;

            currentRangedAttacks.arc = operation(currentRangedAttacks.arc, rangedAttacksModifier.arc);
            currentRangedAttacks.attackCount = operation(currentRangedAttacks.attackCount, rangedAttacksModifier.attackCount);
            currentRangedAttacks.targets = Mathf.CeilToInt(operation(currentRangedAttacks.targets,rangedAttacksModifier.targets));
        }

        private void ApplyRangeStats(Func<float, float, float> operation, CharacterStats newModifier)
        {
            RangeAttackDataSO currentRangedAttacks = (RangeAttackDataSO)CurrentStats.attackData;

            if (!(newModifier.attackData is RangeAttackDataSO))
            {
                return;
            }

            RangeAttackDataSO rangedAttacksModifier = (RangeAttackDataSO)newModifier.attackData;
            
            currentRangedAttacks.projectilesPerAttack = operation(currentRangedAttacks.projectilesPerAttack, rangedAttacksModifier.projectilesPerAttack);
            currentRangedAttacks.anglePerShot = operation(currentRangedAttacks.anglePerShot, rangedAttacksModifier.anglePerShot);
            currentRangedAttacks.piercingCount = operation(currentRangedAttacks.piercingCount, rangedAttacksModifier.piercingCount);
        }

        private void LimitStats(ref float stat, float minVal)
        {
            stat = Mathf.Max(stat, minVal);
        }

        private void LimitAllStats()
        {
            if (CurrentStats == null || CurrentStats.attackData == null)
            {
                return;
            }

            CurrentStats.maxHealth = Mathf.Max(CurrentStats.maxHealth, MinMaxHealth);
            LimitStats(ref CurrentStats.speed, MinSpeed);
            LimitStats(ref CurrentStats.attackData.speed, MinAttackSpeed);
            LimitStats(ref CurrentStats.attackData.delay, MinAttackDelay);
        }
    }
}