using System;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Stats
{
    public class StatsHandler : MonoBehaviour
    {
        public CharacterStats CurrentStats { get; private set; }

        [SerializeField] private CharacterStats baseStats;
        private List<CharacterStats> _statsModifiers = new List<CharacterStats>();

        private void Start()
        {
            CurrentStats = baseStats;
        }

        public void UpdateStats(CharacterStats statsModifier)
        {
            _statsModifiers.Add(statsModifier);
        }
    }
}