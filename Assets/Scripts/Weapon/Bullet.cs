using System;
using UnityEngine;

// 타겟을 따라가는 페인트볼 유도 탄환. 적에게 닿으면 데미지를 주고 풀로 돌아간다.
// 색과 투명도를 주기적으로 바꿔 반짝이는 느낌을 내고, 잔상은 TrailRenderer가 담당한다.
// 사라질 때 명중했으면 적 몸통에, 아니면 마지막 위치의 바닥에 물감 자국을 남긴다.
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private int _damage = 20;
    // 타겟에 닿지 못했을 때 풀로 돌아가기까지의 시간(초)
    [SerializeField] private float _lifeTime = 2f;

    [Header("반짝임")]
    [SerializeField] private float _blinkSpeed = 20f;
    [SerializeField] private Color _brightColor = new Color(1f, 0.15f, 0.15f, 1f);
    [SerializeField] private Color _dimColor = new Color(0.7f, 0f, 0f, 0.6f);

    private Collider _target;
    private float _elapsedTime;
    private bool _isReleased;
    private Action<Bullet> _releaseToPool;
    private PaintSplatSpawner _paintSplatSpawner;

    // 풀에서 처음 생성될 때 한 번만 호출한다.
    public void Initialize(Action<Bullet> releaseToPool, PaintSplatSpawner paintSplatSpawner)
    {
        _releaseToPool = releaseToPool;
        _paintSplatSpawner = paintSplatSpawner;
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
        UpdateBlink();
    }

    private void FixedUpdate()
    {
        if (TryExpire())
        {
            return;
        }

        MoveTowardTarget();
    }

    private void UpdateBlink()
    {
        // Sin 값(-1 ~ 1)을 0 ~ 1로 바꿔 두 색 사이를 오간다.
        float blinkRatio = (Mathf.Sin(Time.time * _blinkSpeed) + 1f) * 0.5f;
        _spriteRenderer.color = Color.Lerp(_dimColor, _brightColor, blinkRatio);
    }

    // 수명을 누적하고, 만료되었으면 바닥에 자국을 남기고 풀로 돌아간 뒤 true를 반환한다.
    private bool TryExpire()
    {
        _elapsedTime += Time.fixedDeltaTime;

        // 타겟이 사망(비활성화)했거나 수명이 지나면 풀로 돌아간다.
        if (!_target.gameObject.activeInHierarchy || _elapsedTime >= _lifeTime)
        {
            _paintSplatSpawner.SpawnOnGround(_rigidbody.position);
            ReleaseToPool();
            return true;
        }

        return false;
    }

    private void MoveTowardTarget()
    {
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
        // 적 콜라이더 중심(몸통)에 물감 자국을 남긴다.
        _paintSplatSpawner.SpawnOnBody(other.bounds.center);
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
