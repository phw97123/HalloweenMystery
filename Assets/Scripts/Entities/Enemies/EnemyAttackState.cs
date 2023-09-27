using Entites;
using UnityEngine;

public class AttackState : IEnemyState
{
    public void Enter(MeleeEnemyController enemy, EntityController entityController)
    {
        //entityController.CallMove(enemy.DirectionToTarget());
        //entityController.Rotate(enemy.DirectionToTarget());
    }

    public void Update(MeleeEnemyController enemy, EntityController entityController)
    {

    }

    public void Exit(MeleeEnemyController enemy, EntityController entityController)
    {

    }
}