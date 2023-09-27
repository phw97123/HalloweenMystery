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
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.fixedDeltaTime);

        Vector2 moveDirection = transform.up;
        CallMove(moveDirection * movementSpeed);
    }
}
