using Scriptable_Objects.Scripts;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace UI
{
    public class CharacterCardUI : UIPopup
    {
        [SerializeField] private Image borderImage;
        [SerializeField] private Text nameText;
        [SerializeField] private Text hpText;
        [SerializeField] private Text spdText;
        [SerializeField] private Text criText;
        [SerializeField] private Text buffText;
        [SerializeField] private Text goldText;
        [SerializeField] private Text dropText;
        [SerializeField] private GameObject imageContainer;
        [SerializeField] private Image lockImage;


        private bool _isSelected = false;
        private int _index;
        public event Action<int> OnItemClicked;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(SelectCard);
        }

        private void SelectCard()
        {
            OnItemClicked?.Invoke(_index);
            UpdateBorderUI();
        }

        public void SetData(CharacterDataSO data, int index)
        {
            _index = index;
            nameText.text = data.characterName;
            hpText.text = data.stats.maxHealth.ToString("N0");
            spdText.text = data.stats.speed.ToString("N1");
            criText.text = (data.stats.criticalPercentage * 100 % 101).ToString("N0");
            buffText.text = data.stats.buffDurationIncrease.ToString("N0");
            goldText.text = (data.stats.goldPercentage * 100 % 101).ToString("N0");
            dropText.text = (data.stats.itemDropPercentage * 100 % 101).ToString("N0");
            lockImage.gameObject.SetActive(!data.canPlay);
            Instantiate(data.imagePrefab, imageContainer.transform);
        }

        public void SubscribeListItemSelectEvent(int index)
        {
            _isSelected = _index == index;
            UpdateBorderUI();
        }

        private void UpdateBorderUI()
        {
            borderImage.gameObject.SetActive(_isSelected);
        }
    }
}