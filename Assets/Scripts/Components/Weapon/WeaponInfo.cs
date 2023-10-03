using Managers;
using System.Collections.Generic;

namespace Components.Weapon
{
    public struct WeaponInfo
    {
        public WeaponType type;
        public AttackDataSO attackData;
        public List<CharacterStats> PartsDataList;

        public WeaponInfo(List<CharacterStats> partsDataList, AttackDataSO attackData, WeaponType type)
        {
            PartsDataList = partsDataList;
            this.attackData = attackData;
            this.type = type;
        }
    }
}