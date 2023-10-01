using Components;
using Components.Weapon;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

namespace UI
{
    public class PlayerInfoUI : MonoBehaviour
    {
        //get model
        private GameManager _gameManager;

        [SerializeField] private Text atkText;
        [SerializeField] private Text hpText;
        [SerializeField] private Text speedText;
        [SerializeField] private Text rangeText;

        [Header("Melee Attack Info")] [SerializeField]
        private List<GameObject> meleeAttackInfo;

        [SerializeField] private Text atkCountText;
        [SerializeField] private Text arcText;
        [SerializeField] private Text targetsText;


        [Header("Range Attack Info")] [SerializeField]
        private List<GameObject> rangeAttackInfo;

        [SerializeField] private Text projectileCountText;
        [SerializeField] private Text atkAngleText;
        [SerializeField] private Text piercingText;


        [SerializeField] private Text knockBackPowerText;

        [SerializeField] private Text goldPercentText;
        [SerializeField] private Text dropPercentText;

        public void UpdateStatsUI(CharacterStats stats)
        {
            hpText.text = $"{stats.maxHealth:N0}";
            speedText.text = $"{stats.speed:N0}";
            
            goldPercentText.text = $"{stats.goldPercentage:N0}";
            dropPercentText.text = $"{stats.itemDropPercentage:N0}";
        }

        public void UpdateWeaponInfoUI(WeaponInfo weaponInfo)
        {
            atkText.text = $"{weaponInfo.AttackData.damage:N0}";
            rangeText.text = $"{weaponInfo.AttackData.range:N0}";
            knockBackPowerText.text = $"{weaponInfo.AttackData.knockBackPower:N0}";
            UpdateMeleeAttackDataUI(weaponInfo.AttackData);
            UpdateRangeAttackDataUI(weaponInfo.AttackData);
        }

        private void UpdateMeleeAttackDataUI(AttackDataSO atkData)
        {
            MeleeAttackDataSO meleeAtk = atkData as MeleeAttackDataSO;
            bool isMeleeAtk = meleeAtk != null;
            foreach (GameObject go in meleeAttackInfo)
            {
                go.SetActive(isMeleeAtk);
            }

            if (!isMeleeAtk) { return; }

            atkCountText.text = meleeAtk.attackCount.ToString("N0");
            arcText.text = meleeAtk.arc.ToString("N0");
            targetsText.text = meleeAtk.targets.ToString("N0");
        }

        private void UpdateRangeAttackDataUI(AttackDataSO atkData)
        {
            RangeAttackDataSO rangeAtk = atkData as RangeAttackDataSO;

            bool isRangeAtk = rangeAtk != null;
            foreach (GameObject go in rangeAttackInfo)
            {
                go.SetActive(isRangeAtk);
            }

            if (!isRangeAtk) { return; }

            projectileCountText.text = rangeAtk.projectilesPerAttack.ToString("N0");
            atkAngleText.text = rangeAtk.anglePerShot.ToString("N0");
            piercingText.text = rangeAtk.piercingCount.ToString("N0");
        }
    }
}