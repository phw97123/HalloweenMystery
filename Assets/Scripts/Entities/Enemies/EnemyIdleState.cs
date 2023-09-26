using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(ContactEnemyController _enemyController) : base(_enemyController)
    {
    }

    public override void Enter()
    {
        // Idle 상태 진입 시 실행할 코드 작성
    }

    public override void Update()
    {
        // Idle 상태 업데이트 시 실행할 코드 작성
    }

    public override void Exit()
    {
        // Idle 상태 종료 시 실행할 코드 작성
    }
}
