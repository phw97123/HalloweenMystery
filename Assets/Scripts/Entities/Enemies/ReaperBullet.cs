using UnityEngine;

public class ReaperBullet : MonoBehaviour
{
    public float speed;
    public int damage;
    private Vector2 moveDirection;
    private float randomAngle;

    private void Start()
    {
        InitializeMoveDirection();
    }

    private void InitializeMoveDirection()
    {
        randomAngle = Random.Range(0f, 360f);
        moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
    }

    // 발사 방향을 설정하는 메서드
    public void SetDirection(Vector2 direction)
    {
        // 방향이 설정되었을 경우 랜덤 각도 대신에 주어진 방향을 사용합니다.
        if (direction != Vector2.zero)
        {
            moveDirection = direction.normalized;
        }
        else
        {
            // 랜덤 각도를 유지합니다.
            moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
        }
    }

    private void Update()
    {
        MoveBullet();
    }

    private void MoveBullet()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        if (!IsInCameraView(transform.position))
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem playerHealth = collision.GetComponent<HealthSystem>();
            playerHealth?.ChangeHealth(-damage);
            ReturnToPool();
        }
    }

    private bool IsInCameraView(Vector3 position)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
