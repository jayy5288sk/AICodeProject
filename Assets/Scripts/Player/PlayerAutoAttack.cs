using UnityEngine;
using UnityEngine.Pool;

// 사거리 안의 Enemy Layer 적 중 가장 가까운 적을 타겟으로 정하고, 일정 간격으로 탄환을 발사한다.
// 탄환은 ObjectPool로 재사용한다.
public class PlayerAutoAttack : MonoBehaviour
{
    // 한 번에 감지할 수 있는 최대 적 수 (탐색 결과를 담는 배열 크기)
    private const int MAX_DETECT_COUNT = 16;

    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private Bullet _bulletPrefab;
    // 탄환이 사라질 때 물감 자국을 찍어 줄 스포너 (탄환 생성 시 전달)
    [SerializeField] private PaintSplatSpawner _paintSplatSpawner;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRange = 6f;
    [SerializeField] private float _fireInterval = 1f;

    [Header("탄환 풀")]
    [SerializeField] private int _poolDefaultCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;

    private readonly Collider[] _detectedEnemies = new Collider[MAX_DETECT_COUNT];
    private ObjectPool<Bullet> _bulletPool;
    private float _fireTimer;

    // 현재 조준 중인 적. 없으면 null
    public Collider CurrentTarget { get; private set; }

    private void Awake()
    {
        _bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet,
            true, _poolDefaultCapacity, _poolMaxSize);
    }

    private void Update()
    {
        FindNearestEnemy();
        UpdateFireTimer();
        TryFire();
    }

    private void UpdateFireTimer()
    {
        // 발사 간격까지만 시간을 쌓아 두어, 타겟이 없다가 생기면 바로 발사한다.
        if (_fireTimer < _fireInterval)
        {
            _fireTimer += Time.deltaTime;
        }
    }

    // 타겟이 있고 발사 간격이 찼을 때만 발사하고 타이머를 초기화한다.
    private void TryFire()
    {
        if (CurrentTarget == null || _fireTimer < _fireInterval)
        {
            return;
        }

        Fire();
        _fireTimer = 0f;
    }

    private void FindNearestEnemy()
    {
        int detectedCount = Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _detectedEnemies, _enemyLayer);

        Collider nearestEnemy = null;
        float nearestSqrDistance = float.MaxValue;

        for (int i = 0; i < detectedCount; i++)
        {
            // 거리 비교만 하므로 제곱근 계산이 없는 sqrMagnitude를 사용한다.
            float sqrDistance = (_detectedEnemies[i].transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearestEnemy = _detectedEnemies[i];
            }
        }

        CurrentTarget = nearestEnemy;
    }

    private void Fire()
    {
        bool isPiercing = _playerWeapon.IsPiercing;
        Vector3 targetCenter = CurrentTarget.bounds.center;
        Vector3 startPosition = _playerWeapon.MuzzlePoint.position;

        // 관통 탄환은 타겟 몸통 높이에서 출발시켜, 뒤에 있는 적까지 같은 높이로 수평 비행하게 한다.
        if (isPiercing)
        {
            startPosition.y = targetCenter.y;
        }

        // 방향은 발사 시점에 한 번만 정하고, 탄환은 그 방향으로 직선 이동한다.
        Vector3 direction = (targetCenter - startPosition).normalized;

        Bullet bullet = _bulletPool.Get();
        bullet.Launch(startPosition, direction, isPiercing);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(_bulletPrefab);
        bullet.Initialize(_bulletPool.Release, _paintSplatSpawner);
        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
