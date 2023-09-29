using Components;
using Components.Action;
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

        public void CreateInteractableWeapons(bool[] isAble, Vector2 startPosition, Vector2 spacing)
        {
            //todo Refactor bool -> achievement 
            GameObject[] objects = Resources.LoadAll<GameObject>("Prefabs/Weapons");

            for (int i = 0; i < objects.Length; i++)
            {
                if (!isAble[i]) { continue; }

                Instantiate(objects[i], startPosition + spacing * i, Quaternion.identity);
            }
        }

        public void EquipWeapon(GameObject weapon, GameObject character)
        {
            //todo remove to 
            StatsHandler playerStats = character.GetComponent<StatsHandler>();
            BaseAttack prevAttack = character.GetComponentInChildren<BaseAttack>();
            if (prevAttack != null)
            {
                StatsHandler prevWeaponStats = prevAttack.GetComponent<StatsHandler>();
                playerStats.RemoveStatModifier(prevWeaponStats.CurrentStats);
                Destroy(prevAttack.gameObject);
            }

            InteractController controller = weapon.GetComponent<InteractController>();
            EquipWeapon equip = weapon.GetComponent<EquipWeapon>();
            Destroy(controller);
            Destroy(equip);

            //add component to character
            Transform pivot = character.GetComponentsInChildren<Transform>()
                .First(t => t.name == Constants.ARM_PIVOT);
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, pivot.transform, false);

            //update information
            CharacterStats stats = weapon.GetComponent<StatsHandler>().CurrentStats;
            playerStats.AddStatModifier(stats);

            //destroy equipped item
            Destroy(weapon.gameObject);
        }
    }
}