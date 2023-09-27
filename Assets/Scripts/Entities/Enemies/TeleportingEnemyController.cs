using UnityEngine;

public class TeleportingEnemyController : EnemyController
{
    [SerializeField]
    private float teleportDistance = 5f;
    [SerializeField]
    private float teleportCooldown = 2f;

    private Vector2 _playerPosition;
    private float _lastTeleportTime;
    private float _minX, _maxX, _minY, _maxY;

    protected override void Start()
    {
        base.Start();

        CalculateCameraBounds();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        UpdatePlayerPosition();

        TeleportEnemy();
    }

    private void UpdatePlayerPosition()
    {
        _playerPosition = Target.position;
    }

    private void TeleportEnemy()
    {
        if (Time.time - _lastTeleportTime >= teleportCooldown)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _playerPosition);
            if (distanceToPlayer < teleportDistance)
            {
                Vector2 teleportPosition = GetRandomTeleportPosition();
                transform.position = teleportPosition;

                _lastTeleportTime = Time.time;
            }
        }
    }

    private Vector2 GetRandomTeleportPosition()
    {
        float x = Random.Range(_minX, _maxX);
        float y = Random.Range(_minY, _maxY);
        return new Vector2(x, y);
    }

    private void CalculateCameraBounds()
    {
        Camera mainCamera = Camera.main;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        _minX = mainCamera.transform.position.x - cameraWidth / 2f;
        _maxX = mainCamera.transform.position.x + cameraWidth / 2f;
        _minY = mainCamera.transform.position.y - cameraHeight / 2f;
        _maxY = mainCamera.transform.position.y + cameraHeight / 2f;
    }
}
