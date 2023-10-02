using Managers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace UI
{
    public class AlertDialogUI : UIPopup
    {
        [SerializeField] private Text messageText;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button cancelButton;

        public Text MessageText => messageText;
        public Button SubmitButton => submitButton;
        public Button CancelButton => cancelButton;


        private void OnDisable()
        {
            submitButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
        }
    }
}