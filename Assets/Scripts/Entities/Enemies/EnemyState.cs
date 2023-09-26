using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    protected ContactEnemyController enemyController;

    public EnemyState(ContactEnemyController _enemyController)
    {
        enemyController = _enemyController;
    }

    public virtual void Enter()
    {
        // 상태 진입 시 실행할 코드 작성
    }

    public virtual void Update()
    {
        // 상태 업데이트 시 실행할 코드 작성
    }

    public virtual void Exit()
    {
        // 상태 종료 시 실행할 코드 작성
    }
}
