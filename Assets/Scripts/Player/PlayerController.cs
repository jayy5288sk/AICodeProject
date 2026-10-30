using UnityEngine;
using UnityEngine.InputSystem;

// WASD 입력을 받아 Rigidbody로 플레이어를 월드 기준 이동·회전시키고, Space 입력으로 점프시킨다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private float _moveSpeed = 5f;
    // 초당 회전 각도(도)
    [SerializeField] private float _rotationSpeed = 720f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Ground Check")]
    // 발밑 위치에 둔 지면 판정 기준점
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;
    private bool _isJumpRequested;

    private void Awake()
    {
        CacheComponents();
    }

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
        _jumpAction.action.Disable();
    }

    private void Update()
    {
        // 입력은 Update에서 읽고, 물리 처리는 FixedUpdate에서 사용한다.
        Vector2 input = _moveAction.action.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, 0f, input.y);

        // 눌린 프레임을 FixedUpdate에서 놓치지 않도록 요청 상태로 저장한다.
        if (_jumpAction.action.WasPressedThisFrame())
        {
            _isJumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        Jump();
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Move()
    {
        Vector3 nextPosition = _rigidbody.position + _moveDirection * (_moveSpeed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(nextPosition);
    }

    private void Rotate()
    {
        // 입력이 없으면 마지막으로 바라보던 방향을 유지한다.
        if (_moveDirection == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
        Quaternion nextRotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(nextRotation);
    }

    private void Jump()
    {
        if (!_isJumpRequested)
        {
            return;
        }

        _isJumpRequested = false;

        // 공중에서 들어온 입력은 버린다.
        if (!IsGrounded())
        {
            return;
        }

        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
    }
}
