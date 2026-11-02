using System;
using UnityEngine;

// 타겟을 따라가는 유도 탄환. 적에게 닿으면 데미지를 주고 풀로 돌아간다.
// 색과 투명도를 주기적으로 바꿔 반짝이는 느낌을 내고, 잔상은 TrailRenderer가 담당한다.
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private int _damage = 20;
    // 타겟에 닿지 못했을 때 풀로 돌아가기까지의 시간(초)
    [SerializeField] private float _lifeTime = 3f;

    [Header("반짝임")]
    [SerializeField] private float _blinkSpeed = 20f;
    [SerializeField] private Color _brightColor = Color.white;
    [SerializeField] private Color _dimColor = new Color(1f, 0.8f, 0.2f, 0.5f);

    private Collider _target;
    private float _elapsedTime;
    private bool _isReleased;
    private Action<Bullet> _releaseToPool;

    // 풀에서 처음 생성될 때 한 번만 호출한다.
    public void Initialize(Action<Bullet> releaseToPool)
    {
        _releaseToPool = releaseToPool;
    }

    public void Launch(Vector3 startPosition, Collider target)
    {
        transform.position = startPosition;
        _rigidbody.position = startPosition;
        _target = target;
        _elapsedTime = 0f;
        _isReleased = false;

        // 이전 발사 때 남은 잔상이 새 위치까지 이어지지 않도록 지운다.
        _trailRenderer.Clear();
    }

    private void Update()
    {
        // Sin 값(-1 ~ 1)을 0 ~ 1로 바꿔 두 색 사이를 오간다.
        float blinkRatio = (Mathf.Sin(Time.time * _blinkSpeed) + 1f) * 0.5f;
        _spriteRenderer.color = Color.Lerp(_dimColor, _brightColor, blinkRatio);
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.fixedDeltaTime;

        // 타겟이 사망(비활성화)했거나 수명이 지나면 풀로 돌아간다.
        if (!_target.gameObject.activeInHierarchy || _elapsedTime >= _lifeTime)
        {
            ReleaseToPool();
            return;
        }

        Vector3 direction = (_target.bounds.center - _rigidbody.position).normalized;
        _rigidbody.MovePosition(_rigidbody.position + direction * (_moveSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            return;
        }

        // 같은 물리 프레임에 여러 적과 닿아도 데미지는 한 번만 준다.
        if (_isReleased)
        {
            return;
        }

        enemyHealth.TakeDamage(_damage);
        ReleaseToPool();
    }

    private void ReleaseToPool()
    {
        // 같은 프레임에 명중과 수명 만료가 겹쳐도 풀에 두 번 반환되지 않도록 막는다.
        if (_isReleased)
        {
            return;
        }

        _isReleased = true;
        _releaseToPool(this);
    }
}
