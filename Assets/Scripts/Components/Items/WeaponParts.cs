using Components;
using Components.Action;
using Components.Stats;
using Entities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class WeaponParts : MonoBehaviour
{
    [SerializeField] private CharacterStats stats;
    [SerializeField] private string interactTag = "Player";
    [SerializeField] private string partsName;
    [SerializeField] private string partsDescription;
    [SerializeField] private float price;
    [SerializeField] private bool isEquiped = false;

    [SerializeField] private TextMeshProUGUI descriptionText;
    private GameObject _notifyCanvas;
    private GameObject _curCollider;

    private StatsHandler _statsHandler;
    private PlayerCharacterController _controller;

    private void Start()
    {
        _notifyCanvas = transform.GetChild(0).gameObject;
        descriptionText.text = $"이름 : {partsName}\n설명 : {partsDescription}\n가격 : {price}";
        _notifyCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(interactTag) && !isEquiped)
        {
            _controller = other.gameObject.GetComponent<PlayerCharacterController>();
            _statsHandler = other.gameObject.GetComponentInChildren<BaseAttack>().GetComponent<StatsHandler>();

            _curCollider = other.gameObject;
            _notifyCanvas.SetActive(true);

            _controller.OnInteractionItemPartsEvent += InteractItemParts;
        }
    }

    private void InteractItemParts()
    {
        if (_curCollider && !isEquiped)
        {
            CharacterStats weaponPartsStats= new CharacterStats { attackData = stats.attackData , changeType = StatsChangeType.Add }; //
            _statsHandler.AddStatModifier(weaponPartsStats);

            isEquiped = true;
            _notifyCanvas.SetActive(false);

            Debug.Log("딜레이 "+_statsHandler.CurrentStats.attackData.delay);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_curCollider)
        {
            _curCollider = null;
            _controller.OnInteractionItemPartsEvent -= InteractItemParts;
            _notifyCanvas.SetActive(false);
        }
    }


}
