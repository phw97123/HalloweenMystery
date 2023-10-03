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
    [SerializeField] private int price;
    [SerializeField] private bool isEquiped = false;

    [SerializeField] private TextMeshProUGUI descriptionText;
    private GameObject _notifyCanvas;
    private GameObject _curCollider;

    private StatsHandler _statsHandler;
    private PlayerCharacterController _controller;
    private GoldSystem _goldSystem;

    public AudioClip partsClip; 

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
            descriptionText.text = $"이름 : {partsName}\n설명 : {partsDescription}\n가격 : {price}";

            _controller = other.gameObject.GetComponent<PlayerCharacterController>();
            _statsHandler = other.gameObject.GetComponentInChildren<BaseAttack>().GetComponent<StatsHandler>();
            _goldSystem = other.gameObject.GetComponent<GoldSystem>();

            _curCollider = other.gameObject;
            _notifyCanvas.SetActive(true);

            _controller.OnInteractionItemPartsEvent += InteractItemParts;

           
        }
    }

    private void InteractItemParts()
    {
        if (_curCollider && !isEquiped && _goldSystem.OwnedGold >= price)
        {
            CharacterStats weaponPartsStats= new CharacterStats { attackData = stats.attackData , changeType = StatsChangeType.Add }; //
            _statsHandler.AddStatModifier(weaponPartsStats);

            isEquiped = true;
            GameManager.Instance.BuyParts(weaponPartsStats);
            _notifyCanvas.SetActive(false);
            _goldSystem.ChangeOwnedGold(-price);

            if (partsClip)
                SoundManager.PlayClip(partsClip);

            Destroy(gameObject);
        }
        else if(_goldSystem.OwnedGold < price)
        {
            descriptionText.text = $"골드가 부족해 구매할 수 없습니다";
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
