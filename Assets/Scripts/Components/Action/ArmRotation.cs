using Entites;
using UnityEngine;

namespace Components.Action
{
    public class ArmRotation : MonoBehaviour
    {
        private EntityController _controller;
        [SerializeField] private Transform armPivot;
        [SerializeField] private SpriteRenderer characterRenderer;
        [SerializeField] private SpriteRenderer weaponRenderer;
        [SerializeField] private bool weaponFlipY = true;


        private void Awake()
        {
            _controller = GetComponent<EntityController>();
            Debug.Assert(armPivot != null);
            Debug.Assert(characterRenderer != null);
            Debug.Assert(weaponRenderer != null);
        }

        private void Start()
        {
            _controller.OnLookEvent += Look;
        }


        private void Look(Vector2 dir)
        {
            float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (weaponFlipY)
            {
                weaponRenderer.flipY = Mathf.Abs(degree) > 90f;
            }

            characterRenderer.flipX = Mathf.Abs(degree) > 90f;
            armPivot.rotation = Quaternion.Euler(0, 0, degree);
        }
    }
}