using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : UIPopup
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text[] achieves;

    private AchiveManager _achiveManager;

    private void Awake()
    {
        _achiveManager = AchiveManager.Instance;
    }

    private void Start()
    {
        closeButton.onClick.AddListener(() => { gameObject.SetActive(false); });
        int n = 0;
        for(int i = 0; i < achieves.Length; i++)
        {
            if(_achiveManager.achievementDataArray[i].isAchive == true)
            {
                achieves[i].text = _achiveManager.AchievementsString[i];
            }
            else
            {
                achieves[i].text = "???";
            }
        }
    }
}
