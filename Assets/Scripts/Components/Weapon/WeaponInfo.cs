using Managers;
using System.Collections.Generic;

namespace Components.Weapon
{
    public struct WeaponInfo
    {
        public WeaponType Type;
        public AttackDataSO AttackData;
        public List<CharacterStats> PartsDataList;
    }
}