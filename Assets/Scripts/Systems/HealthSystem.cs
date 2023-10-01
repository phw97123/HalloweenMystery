using Components;
using Components.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;
    [SerializeField] private int MaxShieldCount = 3;

    private StatsHandler _statsHandler;

    private float _timeSinceLastChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => _statsHandler.CurrentStats.maxHealth;
    public int ShieldCount = 0;

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;
    public event Action OnChangeShieldCount;

    public AudioClip damageClip; 

    private void Awake()
    {
        _statsHandler = GetComponent<StatsHandler>();

        if (_statsHandler == null)
            Debug.Log("StatsHandler not found");
    }

    private void Start()
    {
        CurrentHealth = _statsHandler.CurrentStats.maxHealth;
    }

    private void Update()
    {
        if (_timeSinceLastChange < healthChangeDelay)
        {
            _timeSinceLastChange += Time.deltaTime;
            if (_timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || _timeSinceLastChange < healthChangeDelay)
            return false;

        _timeSinceLastChange = 0f;

        if (change < 0 && ShieldCount > 0)
        {
            ShieldCount--;
            OnChangeShieldCount?.Invoke();
            return true;
        }

        CurrentHealth += change;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (change > 0)
            OnHeal?.Invoke();
        else
        {
            OnDamage?.Invoke();
            if (damageClip)
                SoundManager.PlayClip(damageClip); 
        }

        if (CurrentHealth <= 0f)
            CallDeath();

        return true;
    }

    public void AddShieldCount(int amount)
    {
        ShieldCount = Mathf.Min(ShieldCount + amount, MaxShieldCount);
        OnChangeShieldCount?.Invoke();
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}
