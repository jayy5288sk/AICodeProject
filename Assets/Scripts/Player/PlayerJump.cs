using UnityEngine;
using UnityEngine.InputSystem;

// Space 입력으로 Rigidbody를 점프시킨다. 땅에 닿아 있을 때만 점프할 수 있다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private float _jumpVelocity = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody _rigidbody;

    // Update에서 받은 점프 입력을 FixedUpdate로 전달하기 위한 플래그
    private bool _isJumpRequested;

    private void Awake()
    {
        CacheComponents();
    }

    private void OnEnable()
    {
        _jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        _jumpAction.action.Disable();
    }

    private void Update()
    {
        // 입력은 Update에서 읽고, 물리 점프는 FixedUpdate에서 처리한다.
        ReadJumpInput();
    }

    private void FixedUpdate()
    {
        ApplyJump();
    }

    private void ReadJumpInput()
    {
        if (_jumpAction.action.WasPressedThisFrame())
        {
            _isJumpRequested = true;
        }
    }

    private void ApplyJump()
    {
        if (!_isJumpRequested)
        {
            return;
        }

        // 공중에서 누른 입력은 저장하지 않고 버린다. (착지 후 자동 점프 방지)
        _isJumpRequested = false;

        if (!IsGrounded())
        {
            return;
        }

        // 수평 속도(x, z)는 유지하고 위쪽 속도(y)만 지정한다.
        Vector3 currentVelocity = _rigidbody.linearVelocity;
        _rigidbody.linearVelocity = new Vector3(currentVelocity.x, _jumpVelocity, currentVelocity.z);
    }

    private bool IsGrounded()
    {
        // 발밑 위치에 구를 만들어 Ground 레이어와 겹치는지 검사한다.
        return Physics.CheckSphere(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
