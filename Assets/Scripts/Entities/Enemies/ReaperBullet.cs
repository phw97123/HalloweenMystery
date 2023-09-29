using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperBullet : MonoBehaviour
{
    public float speed;

    private Vector2 moveDirection;

    private void Start()
    {
        float randomAngle = Random.Range(0f, 360f);
        moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
    }

    private void Update()
    {
        // 이전 위치 저장
        Vector3 previousPosition = transform.position;

        // 이동
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 현재 위치 확인
        Vector3 currentPosition = transform.position;

        // 카메라 뷰포트 밖으로 나갔는지 확인
        if (!IsInCameraView(currentPosition))
        {
            // 비활성화하고 오브젝트 풀로 반환
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 충돌 시 오브젝트 풀로 반환
            ReturnToPool();
        }
    }

    // 카메라 뷰포트 내에 있는지 확인하는 함수
    private bool IsInCameraView(Vector3 position)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }

    // 오브젝트 풀로 반환하는 메서드
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
