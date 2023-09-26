using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactEnemyController
{
    private EnemyStateMachine stateMachine;
    private EnemyState idleState;
    private EnemyState chaseState;

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

}
