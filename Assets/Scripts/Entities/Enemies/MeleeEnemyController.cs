using Components.Stats;
using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    [SerializeField]
    [Range(0f, 100f)] private float followRange;
    [SerializeField]
    private string targetTag = "Player";
    private bool _isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRendere;

    [SerializeField]
    private GameObject spawnParticlePrefab;

    private HealthSystem healthSystem;
    private HealthSystem _collidingTargetHealthSystem;

    private GameObject spawnParticleInstance;
    [SerializeField] private AudioClip appearingClip;

    protected override void Start()
    {
        base.Start();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;
        followRange = 10f;

        if (spawnParticlePrefab != null)
        {
            spawnParticleInstance = Instantiate(spawnParticlePrefab, transform.position, Quaternion.identity);
            if (appearingClip != null)
            {
                SoundManager.PlayClip(appearingClip);
            }

            Destroy(spawnParticleInstance, 5f);
        }
    }

    private void OnDamage()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            SetTarget(playerObject.transform);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isCollidingWithTarget)
        {
            ApplyHealthChange();
        }

        Vector2 direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }

        CallMove(direction);
        Rotate(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Deg2Rad;
        characterRendere.flipX = Mathf.Abs(rotZ) > 90f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;

        if (!receiver.CompareTag(targetTag))
        {
            return;
        }

        _collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        if (_collidingTargetHealthSystem != null)
        {
            _isCollidingWithTarget = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag))
        {
            return;
        }
        _isCollidingWithTarget = false;
    }

    private void ApplyHealthChange()
    {
        AttackDataSO attackSo = Stats.CurrentStats.attackData;
        bool hasBeenChanged = _collidingTargetHealthSystem.ChangeHealth(-attackSo.damage);
    }
}
