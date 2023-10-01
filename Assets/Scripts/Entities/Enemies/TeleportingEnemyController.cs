using Components.Stats;
using UnityEngine;

public class TeleportingEnemyController : EnemyController
{
    [SerializeField]
    private float moveCooldown = 2f;

    private Vector2 _playerPosition;
    private float _lastMoveTime;

    private StatsHandler statsHandler;

    [SerializeField]
    private GameObject teleportParticlePrefab; // 파티클 프리팹을 Inspector에서 할당합니다.

    protected override void Start()
    {
        base.Start();
        _lastMoveTime = Time.time;
        statsHandler = GetComponent<StatsHandler>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        UpdatePlayerPosition();

        MoveTowardPlayer();
    }

    private void UpdatePlayerPosition()
    {
        if (Target != null)
        {
            _playerPosition = Target.position;
        }
    }

    private void MoveTowardPlayer()
    {
        float speed = statsHandler.CurrentStats.speed * 30;

        if (Time.time - _lastMoveTime >= moveCooldown)
        {
            Vector2 directionToPlayer = _playerPosition - (Vector2)transform.position;
            directionToPlayer.Normalize();

            // 순간이동하기 전에 파티클 효과 생성
            if (teleportParticlePrefab != null)
            {
                GameObject teleportParticle = Instantiate(teleportParticlePrefab, transform.position, Quaternion.identity);
                // 일정 시간 후에 파티클 효과 삭제
                Destroy(teleportParticle, 2f); // 2초 후에 파티클 삭제 (원하는 시간으로 수정 가능)
            }

            transform.Translate(directionToPlayer * speed * Time.deltaTime);

            _lastMoveTime = Time.time;
        }
    }
}
