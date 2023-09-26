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
            weaponRenderer.flipY = Mathf.Abs(degree) > 90f;
            characterRenderer.flipX = weaponRenderer.flipY;
            armPivot.rotation = Quaternion.Euler(0, 0, degree);
        }
    }
}