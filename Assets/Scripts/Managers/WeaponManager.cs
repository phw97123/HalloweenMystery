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
    public enum WeaponType
    {
        Sword,
        Dagger,
        Axe,
        Pistol,
        Rifle,
        Shotgun,
    }

    public class WeaponManager : MonoBehaviour
    {
        private ResourceManager _resourceManager;
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


        //todo refactor name -> createInteractableWeaponByName
        public void CreateInteractableWeapon(string weaponName, Vector2 position)
        {
            //todo by ResourcesManager
            GameObject obj = Resources.Load<GameObject>($"Prefabs/Weapons/{weaponName}");
            GameObject go = Instantiate(obj, position, Quaternion.identity);

            go.AddComponent<InteractController>();
            go.AddComponent<EquipWeapon>();
        }

        public void CreateInteractableWeaponByPrefab(GameObject prefab, Vector2 position)
        {
            //todo by ResourcesManager
            GameObject go = Instantiate(prefab, position, Quaternion.identity);

            go.AddComponent<InteractController>();
            go.AddComponent<EquipWeapon>();
        }

        public void CreateInteractableWeapons(WeaponType[] availWeapons, Vector2 startPosition, Vector2 spacing)
        {
            //todo Refactor bool -> achievement 

            for (int i = 0; i < availWeapons.Length; i++)
            {
                GameObject go = _resourceManager.LoadPrefab(availWeapons[i].ToString());
                Instantiate(go, (startPosition + spacing * i), Quaternion.identity);
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