using System;
using UnityEngine;

// 적의 체력을 관리하고, 피격 시 체력 변경을 알리며, 체력이 0 이하가 되면 비활성화한다.
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    private int _currentHealth;

    // 남은 체력 비율(0 ~ 1)을 전달한다.
    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);
        OnHealthChanged?.Invoke((float)_currentHealth / _maxHealth);

        if (_currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 비활성화된 적은 타겟 탐색(OverlapSphere)에서 자동으로 제외된다.
        gameObject.SetActive(false);
    }
}
