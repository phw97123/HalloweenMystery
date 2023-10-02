using Managers;
using System;
using UI;
using UnityEngine;

namespace Components.Action
{
    public class BaseAttack : MonoBehaviour
    {
        [SerializeField] private WeaponType weaponType;
        public WeaponType WeaponType => weaponType;

        public event Action<float> OnAttackDelayChanged;


        protected void CallAttackDelayChange(float percent)
        {
            OnAttackDelayChanged?.Invoke(percent);
        }
    }
}