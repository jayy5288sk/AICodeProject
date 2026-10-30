using UnityEngine;
using UnityEngine.InputSystem;

// 카메라 기준 WASD 입력으로 Rigidbody를 이동시키고, 이동 방향으로 부드럽게 회전시킨다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;

    private void Awake()
    {
        CacheComponents();
    }

    private void OnEnable()
    {
        _moveAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
    }

    private void Update()
    {
        // 입력은 Update에서 읽어 FixedUpdate로 전달한다.
        Vector2 moveInput = _moveAction.action.ReadValue<Vector2>();
        _moveDirection = GetCameraRelativeDirection(moveInput);
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // 카메라의 앞/오른쪽 방향을 수평면에 투영해 입력을 월드 방향으로 변환한다.
    private Vector3 GetCameraRelativeDirection(Vector2 moveInput)
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;

        return cameraForward * moveInput.y + cameraRight * moveInput.x;
    }

    private void Move()
    {
        // 중력에 의한 낙하 속도를 유지하기 위해 Y축 속도는 보존한다.
        Vector3 velocity = _moveDirection * _moveSpeed;
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }

    private void Rotate()
    {
        if (_moveDirection == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
    }
}
