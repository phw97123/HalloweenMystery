using Components.Action;
using Components.Stats;
using UnityEngine;

public class RangeEnemyController : EnemyController
{
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float shootRange = 10f;

    private RangeAttack rangeAttack;
    private StatsHandler Stats;

    protected override void Start()
    {
        base.Start();
        rangeAttack = GetComponent<RangeAttack>();
        Stats = GetComponent<StatsHandler>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        if (distance <= followRange)
        {
            if (distance <= shootRange)
            {
                int layerMaskTarget = Stats.CurrentStats.attackData.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 11f, (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                if (hit.collider != null && (layerMaskTarget & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    CallLook(direction);
                    CallMove(Vector2.zero);
                    rangeAttack.enabled = true; // RangeAttack 스크립트 활성화
                    CallAttack(); // 공격 호출
                }
            }
            else
            {
                CallMove(direction);
            }
        }
        else
        {
            CallMove(direction);
        }
    }
}
