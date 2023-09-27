using Components;
using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
            _statsHandler = other.gameObject.GetComponent<StatsHandler>();
            if (stats.attackData.GetType() == _statsHandler.CurrentStats.attackData.GetType())
            {
                _curCollider = other.gameObject;
                _notifyCanvas.SetActive(true);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_curCollider && Input.GetKeyDown(KeyCode.Z) && !isEquiped)
        {
            CharacterStats weaponPartsStats= new CharacterStats { attackData = stats.attackData };
            _statsHandler.AddStatModifier(weaponPartsStats);

            isEquiped = true;
            _notifyCanvas.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_curCollider)
        {
            _curCollider = null;
            _notifyCanvas.SetActive(false);
        }
    }


}
