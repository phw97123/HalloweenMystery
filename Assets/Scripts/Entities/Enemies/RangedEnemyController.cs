using Components.Stats;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private float attackInterval;
    [SerializeField] private int numProjectiles;

    [SerializeField] private string targetTag = "Player";
    private bool _isCollidingWithTarget;

    [SerializeField] private GameObject spawnParticlePrefab; // 등장 파티클 프리팹을 Inspector에서 할당합니다.

    private Transform _player;
    private float _timeUntilNextAttack;
    private EnemyObjectPool _objectPoolManager;
    private StatsHandler _statsHandler;

    private HealthSystem _collidingTargetHealthSystem;
    private GameObject spawnParticleInstance; // 파티클 오브젝트에 대한 참조를 유지합니다.

    protected override void Start()
    {
        base.Start();
        InitializePlayer();
        InitializeAttackParams();

        // 몬스터 등장 시 파티클 효과 생성
        if (spawnParticlePrefab != null)
        {
            spawnParticleInstance = Instantiate(spawnParticlePrefab, transform.position, Quaternion.identity);
            Destroy(spawnParticleInstance, 10f); // 10초 후에 파티클 삭제
        }
    }

    private void InitializePlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(targetTag);
        _player = playerObject != null ? playerObject.transform : null;
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
                    bullet.transform.position = transform.position;
                    bullet.SetActive(true);
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
