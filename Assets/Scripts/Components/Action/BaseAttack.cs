using Managers;
using UnityEngine;

namespace Components.Action
{
    public class BaseAttack : MonoBehaviour
    {
        [SerializeField] private WeaponType weaponType;
        public WeaponType WeaponType => weaponType;
    }
}