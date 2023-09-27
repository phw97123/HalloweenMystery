using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyController : EnemyController
{
    [SerializeField] private float specialAttackRange = 10f;

    [SerializeField] private float summonInterval = 10f;
    [SerializeField] private GameObject[] minionPrefabs;
    [SerializeField] private Transform summonPosition;

    private float _lastSummonTime;
    private List<GameObject> _minions = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        _lastSummonTime = summonInterval;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();

        if (distance <= specialAttackRange)
        {
            PerformSpecialAttack();
        }

        if (Time.time - _lastSummonTime >= summonInterval)
        {
            SummonMinion();
            _lastSummonTime = Time.time;
        }
    }

    private void PerformSpecialAttack()
    {
        // 공격 로직
    }

    private void SummonMinion()
    {
        if (minionPrefabs != null && minionPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, minionPrefabs.Length);
            GameObject selectedMinionPrefab = minionPrefabs[randomIndex];

            GameObject minion = Instantiate(selectedMinionPrefab, summonPosition.position, Quaternion.identity, summonPosition);

            _minions.Add(minion);
        }
    }
}
