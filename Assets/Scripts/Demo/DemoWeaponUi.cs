#if UNITY_EDITOR


using Entites;
using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class DemoWeaponUi : UIPopup
    {
        private DemoUi _demoUi;

        [SerializeField] private Button backButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button ableWeaponsButton;
        [SerializeField] private Button shotgunButton;
        [SerializeField] private Button rifleButton;
        [SerializeField] private Button pistolButton;
        [SerializeField] private Button swordButton;
        [SerializeField] private Button daggerButton;
        [SerializeField] private Button axeButton;

        private void Awake()
        {
            _demoUi = GetComponentInParent<DemoUi>();
        }

        private void Start()
        {
            _demoUi.OnStateChanged += UpdateUi;

            backButton.onClick.AddListener(_demoUi.BackPressed);
            deleteButton.onClick.AddListener(DeleteAllWeapons);
            shotgunButton.onClick.AddListener(() => { CreateWeapon(WeaponType.Shotgun); });
            rifleButton.onClick.AddListener(() => { CreateWeapon(WeaponType.Rifle); });
            pistolButton.onClick.AddListener(() => { CreateWeapon(WeaponType.Pistol); });
            swordButton.onClick.AddListener(() => { CreateWeapon(WeaponType.Sword); });
            daggerButton.onClick.AddListener(() => { CreateWeapon(WeaponType.Dagger); });
            axeButton.onClick.AddListener(() => { CreateWeapon(WeaponType.Axe); });

            //todo get bool array
            //ableWeaponsButton.onClick.AddListener();
        }

        private void OnDestroy()
        {
            backButton.onClick.RemoveAllListeners();
            deleteButton.onClick.RemoveAllListeners();
            shotgunButton.onClick.RemoveAllListeners();
            rifleButton.onClick.RemoveAllListeners();
            pistolButton.onClick.RemoveAllListeners();
            swordButton.onClick.RemoveAllListeners();
            daggerButton.onClick.RemoveAllListeners();
            axeButton.onClick.RemoveAllListeners();
        }

        private void DeleteAllWeapons()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Weapon"))
            {
                if (go.GetComponentInParent<EntityController>() == null)
                {
                    Destroy(go);
                }
            }
        }

        private void UpdateUi(DemoState state)
        {
            gameObject.SetActive(state == DemoState.CreateWeapon);
        }

        private void CreateWeapon(WeaponType weaponType)
        {
            WeaponManager.Singleton.CreateInteractableWeapon(weaponType, new Vector2(-1, 1));
        }
    }
}

#endif