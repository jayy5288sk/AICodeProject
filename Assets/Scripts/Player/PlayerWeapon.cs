using UnityEngine;

// 이동 방향으로 무기를 회전시키고, 왼쪽을 향할 때 무기 이미지가 위아래로 뒤집혀 보이지 않도록 그래픽을 반전한다.
// 무기 스프라이트가 바뀌면 MuzzlePoint를 스프라이트의 오른쪽 끝 중앙(총구)으로 맞춘다.
// 이 컴포넌트는 회전을 담당하는 WeaponPivot 오브젝트에 붙인다.
public class PlayerWeapon : MonoBehaviour
{
    private const float NORMAL_SCALE_Y = 1f;
    private const float FLIPPED_SCALE_Y = -1f;

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAutoAttack _playerAutoAttack;
    [SerializeField] private SpriteRenderer _weaponRenderer;
    [SerializeField] private Transform _muzzlePoint;
    // 플레이어 스프라이트와 같은 기울기(카메라 각도)로 무기를 눕힌다.
    [SerializeField] private float _tiltAngle = 45f;

    private Transform _weaponGraphic;
    private Vector3 _lastAimDirection;
    private Sprite _currentSprite;

    // 총알이 생성될 위치 (PlayerAutoAttack에서 사용)
    public Transform MuzzlePoint => _muzzlePoint;

    private void Awake()
    {
        CacheComponents();
    }

    private void Update()
    {
        UpdateMuzzle();
        UpdateAim();
    }

    private void UpdateMuzzle()
    {
        // 스프라이트가 교체되었을 때만 총구 위치를 다시 맞춘다.
        if (_weaponRenderer.sprite != _currentSprite)
        {
            AlignMuzzleToSprite();
        }
    }

    private void UpdateAim()
    {
        // 정지 중(타겟 없음)이거나 방향이 같으면 마지막 조준을 유지한다.
        Vector3 aimDirection = GetAimDirection();
        if (aimDirection == Vector3.zero || aimDirection == _lastAimDirection)
        {
            return;
        }

        AimAt(aimDirection);
        _lastAimDirection = aimDirection;
    }

    private void CacheComponents()
    {
        _weaponGraphic = _weaponRenderer.transform;
    }

    // 타겟이 있으면 타겟 방향, 없으면 이동 방향을 조준한다.
    private Vector3 GetAimDirection()
    {
        Collider target = _playerAutoAttack.CurrentTarget;
        
        if (target == null)
        {
            return _playerMovement.MoveDirection;
        }

        // 높이 차이는 무시하고 XZ 평면의 방향만 사용한다.
        Vector3 toTarget = target.bounds.center - transform.position;
        toTarget.y = 0f;
        return toTarget.normalized;
    }

    private void AimAt(Vector3 direction)
    {
        // XZ 평면의 방향을 각도로 바꾼다. (오른쪽 0°, 앞쪽 90°)
        float aimAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        // Euler 적용 순서(Z → X)에 따라, 평면 안에서 조준 각도만큼 돌린 뒤 카메라 쪽으로 기울인다.
        transform.localRotation = Quaternion.Euler(_tiltAngle, 0f, aimAngle);

        // 좌우 입력이 있을 때만 반전 여부를 바꾸고, 위/아래 조준 시에는 마지막 상태를 유지한다.
        if (direction.x != 0f)
        {
            // 왼쪽을 향하면 회전 때문에 위아래가 뒤집히므로 Y 스케일을 반전해 바로 세운다. (MuzzlePoint도 함께 반전)
            Vector3 graphicScale = _weaponGraphic.localScale;
            graphicScale.y = direction.x < 0f ? FLIPPED_SCALE_Y : NORMAL_SCALE_Y;
            _weaponGraphic.localScale = graphicScale;
        }
    }

    private void AlignMuzzleToSprite()
    {
        _currentSprite = _weaponRenderer.sprite;

        // 스프라이트의 오른쪽 끝 중앙을 총구로 본다. (스프라이트는 총구가 오른쪽을 향하도록 그린다)
        Bounds spriteBounds = _currentSprite.bounds;
        _muzzlePoint.localPosition = new Vector3(spriteBounds.max.x, spriteBounds.center.y, 0f);
    }
}
