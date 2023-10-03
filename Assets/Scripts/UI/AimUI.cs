using Components;
using Components.Stats;
using Entites;
using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace UI
{
    public class AimUI : UIPopup
    {
        [SerializeField] private float maxLerpTime = 1.5f;
        [SerializeField] private Vector3 maxScale = new Vector3(0.5f, 0.5f);

        private Camera _camera;
        private EntityController _aimController;
        private StatsHandler _statsHandler;
        private float _range;
        private Vector3 _startScale;
        private float _timeForLerp;

        private void Awake()
        {
            Cursor.visible = false;
            _camera = Camera.main;
        }

        private void Start()
        {
            _startScale = transform.localScale;
            _aimController = GameManager.Instance.Player.GetComponent<EntityController>();
            _statsHandler = GameManager.Instance.Player.GetComponent<StatsHandler>();
            _range = _statsHandler.CurrentStats.attackData.range;
            _aimController.OnLookEvent += Aim;
        }

        private void Update()
        {
            if (_range <= 0)
            {
                gameObject.SetActive(false);
                return;
            }

            _timeForLerp += Time.deltaTime;
            if (_timeForLerp >= maxLerpTime) { _timeForLerp = 0f; }

            Vector3 currentScale = Vector3.Lerp(_startScale, _startScale + maxScale, _timeForLerp / maxLerpTime);
            transform.localScale = currentScale;
        }

        private void OnDestroy()
        {
            _aimController.OnLookEvent -= Aim;
        }

        private void Aim(Vector2 dir)
        {
            Vector3 newAim = _aimController.transform.position + (Vector3)dir;
            Vector3 screenPos = _camera.WorldToScreenPoint(newAim);
            transform.position = screenPos;
        }
    }
}