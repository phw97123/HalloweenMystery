using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public int damage;

    private Vector2 moveDirection;

    private void Start()
    {
        InitializeMoveDirection();
    }

    private void InitializeMoveDirection()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            moveDirection = (playerObject.transform.position - transform.position).normalized;
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

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
    }
}
