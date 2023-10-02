using Components.Stats;
using UnityEngine;

public class TeleportingEnemyController : EnemyController
{
    [SerializeField]
    private float moveCooldown = 2f;

    private Transform _playerTransform;
    private float _lastMoveTime;

    private StatsHandler statsHandler;

    [SerializeField]
    private GameObject teleportParticlePrefab;

    protected override void Start()
    {
        base.Start();
        _lastMoveTime = Time.time;
        statsHandler = GetComponent<StatsHandler>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        MoveTowardPlayer();
    }

    private void MoveTowardPlayer()
    {
        if (_playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                _playerTransform = playerObject.transform;
            }
            else
            {
                return;
            }
        }

        float speed = statsHandler.CurrentStats.speed * 30;

        if (Time.time - _lastMoveTime >= moveCooldown)
        {
            Vector2 directionToPlayer = _playerTransform.position - transform.position;
            directionToPlayer.Normalize();

            if (teleportParticlePrefab != null)
            {
                GameObject teleportParticle = Instantiate(teleportParticlePrefab, transform.position, Quaternion.identity);
                Destroy(teleportParticle, 2f);
            }

            transform.position += (Vector3)(directionToPlayer * speed * Time.deltaTime);

            _lastMoveTime = Time.time;
        }
    }
}
