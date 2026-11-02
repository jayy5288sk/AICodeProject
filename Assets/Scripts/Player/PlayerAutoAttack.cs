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

        // 발사 간격까지만 시간을 쌓아 두어, 타겟이 없다가 생기면 바로 발사한다.
        if (_fireTimer < _fireInterval)
        {
            _fireTimer += Time.deltaTime;
        }

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
        Bullet bullet = _bulletPool.Get();
        bullet.Launch(_playerWeapon.MuzzlePoint.position, CurrentTarget);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(_bulletPrefab);
        bullet.Initialize(_bulletPool.Release);
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
