using Entities;
using Scriptable_Objects.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterListUI : UIPopup
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private GameObject cardItemPrefab;
        [SerializeField] private List<CharacterDataSO> characterDataList;

        public event Action<CharacterDataSO,int> OnCharacterItemSelected;

        private void Start()
        {
            for (int i = 0; i < characterDataList.Count; i++)
            {
                CreateCardItem(characterDataList[i], i).transform.SetParent(content);
            }
        }

        private GameObject CreateCardItem(CharacterDataSO item, int index)
        {
            GameObject go = Instantiate(cardItemPrefab, content.transform);
            CharacterCardUI card = go.GetComponent<CharacterCardUI>();
            OnCharacterItemSelected += card.SubscribeListCharacterItemSelectEvent;
            card.OnItemClicked += SelectItem;
            card.SetData(item, index);
            return go;
        }

        private void SelectItem(int index)
        {
            OnCharacterItemSelected?.Invoke(characterDataList[index],index);
        }
    }
}