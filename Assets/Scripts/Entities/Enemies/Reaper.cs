using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float attackInterval;
    public int numProjectiles;

    private Transform player;
    private float timeUntilNextAttack;
    private ReaperObjectPool objectPoolManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeUntilNextAttack = attackInterval;
        objectPoolManager = FindObjectOfType<ReaperObjectPool>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceToPlayer > retreatDistance)
        {
            transform.position = transform.position;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
    }

    private void HandleAttack()
    {
        if (timeUntilNextAttack <= 0)
        {
            Attack();
            timeUntilNextAttack = attackInterval;
        }
        else
        {
            timeUntilNextAttack -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        for (int i = 0; i < numProjectiles; i++)
        {
            GameObject bullet = objectPoolManager.GetPooledProjectile();

            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.SetActive(true);
            }
        }
    }
}
