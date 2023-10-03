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
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Stats = GetComponent<StatsHandler>();
    }

    protected virtual void FixedUpdate()
    {
        if (Target != null)
        {
            Vector2 direction = DirectionToTarget();
            CallMove(direction);
        }
    }

    protected float DistanceToTarget()
    {
        if (Target != null)
        {
            return Vector3.Distance(transform.position, Target.position);
        }
        else
        {
            return float.MaxValue;
        }
    }

    protected Vector2 DirectionToTarget()
    {
        if (Target != null)
        {
            return (Target.position - transform.position).normalized;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
