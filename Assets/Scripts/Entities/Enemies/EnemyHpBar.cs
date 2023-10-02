using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private HealthSystem enemyHealthSystem;
    [SerializeField] private Slider hpGaugeSlider;

    private void Awake()
    {
        enemyHealthSystem = GetComponent<HealthSystem>();

        hpGaugeSlider.maxValue = enemyHealthSystem.MaxHealth;
        hpGaugeSlider.value = enemyHealthSystem.CurrentHealth;

        enemyHealthSystem.OnDamage += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = enemyHealthSystem.CurrentHealth;
    }
}
