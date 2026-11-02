using UnityEngine;
using UnityEngine.UI;

// 적의 체력이 바뀔 때만 빨간 게이지(Filled Image)의 채움 비율을 갱신한다.
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private Image _gaugeImage;

    private void OnEnable()
    {
        _enemyHealth.OnHealthChanged += UpdateGauge;
    }

    private void OnDisable()
    {
        _enemyHealth.OnHealthChanged -= UpdateGauge;
    }

    private void UpdateGauge(float healthRatio)
    {
        _gaugeImage.fillAmount = healthRatio;
    }
}
