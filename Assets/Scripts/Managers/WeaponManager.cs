using Components;
using Components.Action;
using Components.Attacks;
using Components.Stats;
using Components.Weapon;
using Entities;
using System;
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

    /// <summary>
    /// WeaponManager responsible for do equip, createWeapons,
    /// has status of currentEquippedWeapon
    /// 상태 : 현재 장착중인 무기 정보
    /// 동작 : 무기 장착, 무기 생성
    /// </summary>
    public class WeaponManager : MonoBehaviour
    {
        private ResourceManager _resourceManager;
        private static WeaponManager _singleton;

        public WeaponInfo? CurrentEquippedWeapon { get; private set; } = null;
        public event Action<WeaponInfo?> OnWeaponEquipped;

        public AudioClip equipClip;

        private void Start()
        {
            equipClip = Resources.Load<AudioClip>("Sound/WeaponEquip");
        }

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

        private void Awake()
        {
            if (_singleton != null && _singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            _singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        //todo refactor name -> createInteractableWeaponByName
        public void CreateInteractableWeapon(WeaponType weaponType, Vector2 position)
        {
            GameObject obj = ResourceManager.Instance.LoadPrefab(weaponType.ToString());
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
                //todo refactor resourcesManager
                Debug.Log($"Prefabs/Weapons/{availWeapons[i].ToString()}");
                GameObject go = Resources.Load<GameObject>($"Prefabs/Weapons/{availWeapons[i].ToString()}");
                CreateInteractableWeaponByPrefab(go, startPosition + spacing * i);
            }
        }

        public void EquipWeapon(GameObject weapon, GameObject character)
        {
            if (CurrentEquippedWeapon != null)
            {
                //todo 장착중인 무기 원상복구 
            }

            //todo remove to 
            StatsHandler playerStats = character.GetComponent<StatsHandler>();
            BaseAttack prevAttack = character.GetComponentInChildren<BaseAttack>();

            if (prevAttack != null)
            {
                playerStats.RemoveTopModifier();
                DestroyImmediate(prevAttack.gameObject);
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
            BaseAttack newAttack = weapon.GetComponent<BaseAttack>();
            CharacterStats weaponStats = weapon.GetComponent<StatsHandler>().CurrentStats;
            playerStats.AddStatModifier(weaponStats);

            CurrentEquippedWeapon = new WeaponInfo
            {
                Type = newAttack.WeaponType, AttackData = weaponStats.attackData,
            };


            if (equipClip)
                SoundManager.PlayClip(equipClip);

            //destroy equipped item
            Destroy(weapon.gameObject);
            OnWeaponEquipped?.Invoke(CurrentEquippedWeapon);
        }
    }
}