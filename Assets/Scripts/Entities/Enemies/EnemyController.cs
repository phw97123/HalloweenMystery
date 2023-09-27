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
    protected Transform ClosestTarget { get; private set; }

    protected virtual void Start()
    {
        //gameManager = GameManager.instance;
        //ClosestTarget = gameManager.Player;
        ClosestTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void FixedUpdate()
    {
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, ClosestTarget.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (ClosestTarget.position - transform.position).normalized;
    }
}
