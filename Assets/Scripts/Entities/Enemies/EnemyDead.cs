using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private float[] itemDropChances;

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

        float randomValue = Random.value;

        float cumulativeChance = 0;

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            cumulativeChance += itemDropChances[i];
            if (randomValue <= cumulativeChance)
            {
                Instantiate(itemPrefabs[i], transform.position, Quaternion.identity);
                break;
            }
        }

        Destroy(gameObject, 2f);
    }
}
