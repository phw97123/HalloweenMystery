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
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Target = playerObject.transform;

            float distance = DistanceToTarget();
            Vector2 direction = DirectionToTarget();
        }
        else
        {
            Target = null;
        }

        Stats = GetComponent<StatsHandler>();      
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
