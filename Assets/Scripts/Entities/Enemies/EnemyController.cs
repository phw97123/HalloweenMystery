using Components.Stats;
using Entites;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    protected bool IsAttacking { get; set; }

    protected StatsHandler Stats { get; private set; }

    GameManager gameManager;
    protected Transform Target { get; private set; }

    protected virtual void Start()
    {
        Stats = GetComponent<StatsHandler>();

        //ClosestTarget = gameManager.Player;
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void FixedUpdate()
    {
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, Target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (Target.position - transform.position).normalized;
    }
}
