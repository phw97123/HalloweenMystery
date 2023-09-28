using Components;
using Components.Attacks;
using Components.Stats;
using Components.Weapon;
using Entities;
using System.Linq;
using UnityEngine;
using Utils;

namespace Managers
{
    public class WeaponManager : MonoBehaviour
    {
        private static WeaponManager _singleton;

        public static WeaponManager Singleton
        {
            get
            {
                if (_singleton != null) { return _singleton; }

                _singleton = FindObjectOfType<WeaponManager>();
                if (_singleton != null) { return _singleton; }

                GameObject go = new GameObject(nameof(WeaponManager) + " - Singleton");
                _singleton = go.AddComponent<WeaponManager>();
                return _singleton;
            }
        }


        public void CreateInteractableWeapon(string weaponName, Vector2 position)
        {
            //todo by ResourcesManager
            GameObject obj = Resources.Load<GameObject>($"Prefabs/Weapons/{weaponName}");
            GameObject go = Instantiate(obj, position, Quaternion.identity);

            go.AddComponent<InteractController>();
            go.AddComponent<EquipWeapon>();
        }

        public void EquipWeapon(GameObject weapon, GameObject character)
        {
            //todo remove current equipped weapon
            InteractController controller = weapon.GetComponent<InteractController>();
            EquipWeapon equip = weapon.GetComponent<EquipWeapon>();
            Destroy(controller);
            Destroy(equip);

            //add component to character
            Transform pivot = character.GetComponentsInChildren<Transform>()
                .First(t => t.name == Constants.ARM_PIVOT);
            Instantiate(weapon, pivot.transform, false);

            //update information
            StatsHandler statsHandler = character.GetComponent<StatsHandler>();
            CharacterStats stats = weapon.GetComponent<StatsHandler>().CurrentStats;
            statsHandler.AddStatModifier(stats);

            //destroy equipped item
            Destroy(weapon);
        }
    }
}