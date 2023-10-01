using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs; // 아이템 프리팹 배열
    [SerializeField] private float[] itemDropChances; // 아이템 드랍 확률 배열
    [SerializeField] private float noDropChance = 0.2f; // 아무것도 드랍하지 않을 확률

    private HealthSystem _healthSystem;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthSystem.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        _rigidbody.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 무작위로 아이템을 선택하고 드랍 확률을 고려하여 생성
        float randomValue = Random.value;
        if (randomValue <= noDropChance)
        {
            // 아무것도 드랍하지 않음
        }
        else
        {
            for (int i = 0; i < itemPrefabs.Length; i++)
            {
                if (randomValue <= noDropChance + itemDropChances[i])
                {
                    Instantiate(itemPrefabs[i], transform.position, Quaternion.identity);
                    break; // 아이템을 한 번만 드랍하도록 처리
                }
            }
        }

        Destroy(gameObject, 2f);
    }
}
