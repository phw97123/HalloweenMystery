using Entites;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    public class PlayerCharacterController : EntityController
    {
        public void OnMove(InputValue input)
        {
            Vector2 dir = input.Get<Vector2>();
            CallMove(dir);
        }

        public void OnLook(InputValue input)
        {
            Vector2 dir = input.Get<Vector2>();
            CallMove(dir);
        }

        public void OnAttack(InputValue value)
        {
            //todo 
        }
    }
}