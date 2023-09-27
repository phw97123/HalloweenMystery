using UnityEngine;

public class TeleportingEnemyController : EnemyController
{
    [SerializeField]
    private float teleportDistance = 5f;
    [SerializeField]
    private float teleportCooldown = 2f;

    private Vector2 playerPosition;
    private float lastTeleportTime;
    private float minX, maxX, minY, maxY; // 카메라 경계 좌표

    protected override void Start()
    {
        base.Start();

        // 카메라 경계 좌표 계산
        CalculateCameraBounds();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 플레이어의 위치 업데이트
        UpdatePlayerPosition();

        // 일정 시간마다 플레이어 근처로 순간이동하기
        TeleportEnemy();
    }

    private void UpdatePlayerPosition()
    {
        // 플레이어의 현재 위치 가져오기
        playerPosition = ClosestTarget.position;
    }

    private void TeleportEnemy()
    {
        // 일정 시간마다 순간이동
        if (Time.time - lastTeleportTime >= teleportCooldown)
        {
            // 플레이어와의 거리가 일정 거리보다 가까우면 순간이동
            float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);
            if (distanceToPlayer < teleportDistance)
            {
                // 순간이동 위치 설정
                Vector2 teleportPosition = GetRandomTeleportPosition();

                // 적을 순간이동 위치로 이동
                transform.position = teleportPosition;

                // 순간이동 시간 업데이트
                lastTeleportTime = Time.time;
            }
        }
    }

    private Vector2 GetRandomTeleportPosition()
    {
        // 카메라 경계를 벗어나지 않는 무작위 위치 반환
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector2(x, y);
    }

    private void CalculateCameraBounds()
    {
        // 카메라 경계 좌표 계산
        Camera mainCamera = Camera.main;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        minX = mainCamera.transform.position.x - cameraWidth / 2f;
        maxX = mainCamera.transform.position.x + cameraWidth / 2f;
        minY = mainCamera.transform.position.y - cameraHeight / 2f;
        maxY = mainCamera.transform.position.y + cameraHeight / 2f;
    }
}
