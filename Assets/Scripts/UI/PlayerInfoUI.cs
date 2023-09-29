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

        ///if atk is ranged => projectiles count, else if melee atkCount 
        [SerializeField] private Text atkCountText;

        [SerializeField] private Text arcText;
        [SerializeField] private Text targetsText;
        [SerializeField] private Text knockBackPowerText;
        [SerializeField] private Text atkAngleText;
        [SerializeField] private Text goldPercentText;
        [SerializeField] private Text dropPercentText;

        private void Awake()
        {
            
        }

        private void Start()
        {
        }
    }
}