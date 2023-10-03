using Components.Stats;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private float attackInterval;
    [SerializeField] private int numProjectiles;

    private bool _isCollidingWithTarget;
    private Transform _player;
    private float _timeUntilNextAttack;
    private EnemyObjectPool _objectPoolManager;
    private StatsHandler _statsHandler;
    private HealthSystem _collidingTargetHealthSystem;

    [SerializeField] private AudioClip shootingClip;
    [SerializeField] private AudioClip appearingClip;
    [SerializeField] private GameObject spawnParticlePrefab;
    private GameObject spawnParticleInstance;

    protected override void Start()
    {
        base.Start();
        InitializePlayer();
        InitializeAttackParams();

        if (spawnParticlePrefab != null)
        {
            spawnParticleInstance = Instantiate(spawnParticlePrefab, transform.position, Quaternion.identity);
            if (appearingClip != null)
            {
                SoundManager.PlayClip(appearingClip);
            }
            Destroy(spawnParticleInstance, 10f);
        }
    }

    private void InitializePlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
        else
        {
            _player = null;
        }
    }

    private void InitializeAttackParams()
    {
        _timeUntilNextAttack = attackInterval;
        _objectPoolManager = FindObjectOfType<EnemyObjectPool>();
        _statsHandler = GetComponent<StatsHandler>();
    }

    private void Update()
    {
        InitializePlayer();
        HandleMovement();
        HandleAttack();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isCollidingWithTarget)
        {
            ApplyHealthChange();
        }
    }

    private void HandleMovement()
    {
        if (_player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
            float speed = _statsHandler.CurrentStats.speed;

            if (distanceToPlayer > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, _player.position, -speed * Time.deltaTime);
            }
        }
    }

    private void HandleAttack()
    {
        if (_timeUntilNextAttack <= 0)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                GameObject bullet = _objectPoolManager.GetPooledProjectile();

                if (bullet != null)
                {
                    Vector3 playerDirection = (_player.position - transform.position).normalized;
                    bullet.transform.position = transform.position;
                    bullet.GetComponent<EnemyBullet>().SetDirection(playerDirection);
                    bullet.SetActive(true);

                    if (shootingClip != null)
                    {
                        SoundManager.PlayClip(shootingClip);
                    }
                }
            }
            _timeUntilNextAttack = attackInterval;
        }
        else
        {
            _timeUntilNextAttack -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;

        if (!receiver.CompareTag("Player"))
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
        if (!collision.CompareTag("Player"))
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
