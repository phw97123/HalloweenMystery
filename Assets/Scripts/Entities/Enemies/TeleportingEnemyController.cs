using Components.Stats;
using UnityEngine;

public class TeleportingEnemyController : EnemyController
{
    [SerializeField]
    private float moveCooldown = 2f;

    private Vector2 _playerPosition;
    private float _lastMoveTime;

    private StatsHandler statsHandler;

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
        _playerPosition = Target.position;

    }

    private void MoveTowardPlayer()
    {
        float speed = statsHandler.CurrentStats.speed * 30;

        if (Time.time - _lastMoveTime >= moveCooldown)
        {
            Vector2 directionToPlayer = _playerPosition - (Vector2)transform.position;
            directionToPlayer.Normalize();

            transform.Translate(directionToPlayer * speed * Time.deltaTime);

            _lastMoveTime = Time.time;
        }
    }
}
