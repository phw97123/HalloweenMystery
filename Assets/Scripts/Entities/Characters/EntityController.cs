using System;
using UnityEngine;

namespace Entites
{
    public class EntityController : MonoBehaviour
    {
        public event Action<Vector2> OnMoveEvent;
        public event Action<Vector2> OnLookEvent;
        public event Action OnAttackEvent;

        protected void CallMove(Vector2 dir)
        {
            OnMoveEvent?.Invoke(dir);
        }

        protected void CallLook(Vector2 dir)
        {
            OnLookEvent?.Invoke(dir);
        }

        protected void CallAttack()
        {
            OnAttackEvent?.Invoke();
        }
    }
}