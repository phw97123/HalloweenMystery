using UnityEngine;

public class CircularEnemyController : EnemyController
{
    [SerializeField]
    private float rotationSpeed = 60f;
    [SerializeField]
    private float movementSpeed = 2f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 적을 이동하면서 동그랗게 회전하게 합니다.
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        // 현재 위치를 기준으로 회전합니다.
        transform.Rotate(Vector3.forward, rotationSpeed * Time.fixedDeltaTime);

        // 이동 방향 설정
        Vector2 moveDirection = transform.up;

        // 이동하면서 회전합니다.
        CallMove(moveDirection * movementSpeed);
    }
}
