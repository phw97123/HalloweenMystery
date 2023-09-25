using Entites;
using UnityEngine;

namespace Components
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
        }

        private void Start()
        {
            _controller.OnLookEvent += Look;
            Debug.Assert(armPivot != null);
            Debug.Assert(characterRenderer != null);
            Debug.Assert(weaponRenderer != null);
        }


        private void Look(Vector2 dir)
        {
            float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            armPivot.transform.rotation = Quaternion.Euler(0, 0, degree);
            characterRenderer.flipX = Mathf.Abs(degree) >= 90f;
            weaponRenderer.flipY = characterRenderer.flipX;
        }
    }
}