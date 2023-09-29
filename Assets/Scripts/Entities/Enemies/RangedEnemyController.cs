using Components.Action;
using Components.Stats;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    public float stoppingDistance;
    public float retreatDistance;
    public float attackInterval;
    public int numProjectiles;

    [SerializeField] private string targetTag = "Player";
    private bool _isCollidingWithTarget;

    private Transform player;
    private float timeUntilNextAttack;
    private ReaperObjectPool objectPoolManager;
    private StatsHandler statsHandler;

    private HealthSystem healthSystem;
    private HealthSystem _collidingTargetHealthSystem;
    private Movement _collidingMovement;

    protected override void Start()
    {
        base.Start();
        InitializePlayer();
        InitializeAttackParams();
    }

    private void InitializePlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(targetTag);
        player = playerObject != null ? playerObject.transform : null;
    }

    private void InitializeAttackParams()
    {
        timeUntilNextAttack = attackInterval;
        objectPoolManager = FindObjectOfType<ReaperObjectPool>();
        statsHandler = GetComponent<StatsHandler>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isCollidingWithTarget)
        {
            ApplyHealthChange();
            Debug.Log("attacking");
        }
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float speed = statsHandler.CurrentStats.speed;

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
    }

    private void HandleAttack()
    {
        if (timeUntilNextAttack <= 0)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                GameObject bullet = objectPoolManager.GetPooledProjectile();

                if (bullet != null)
                {
                    bullet.transform.position = transform.position;
                    bullet.SetActive(true);
                }
            }
            timeUntilNextAttack = attackInterval;
        }
        else
        {
            timeUntilNextAttack -= Time.deltaTime;
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