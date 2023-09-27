using Components;
using Components.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParts : MonoBehaviour
{
    [SerializeField] private CharacterStats stats;
    [SerializeField] private string InteractTag = "Player";
    [SerializeField] private string PartsName;
    [SerializeField] private string PartsDescription;
    [SerializeField] private bool isEquiped = false;
    private GameObject _notifyCanvas;
    private GameObject _curCollider;
    private StatsHandler _statsHandler;

    private void Start()
    {
        _notifyCanvas = transform.GetChild(0).gameObject;
        _notifyCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(InteractTag) && !isEquiped)
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
