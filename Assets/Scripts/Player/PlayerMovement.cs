using UnityEngine;
using UnityEngine.InputSystem;

// 터치 조이스틱(우선) 또는 WASD 입력으로 Rigidbody를 XZ 평면에서 이동시킨다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody _rigidbody;

    // 현재 이동 방향 (월드 XZ 평면, 정지 시 Vector3.zero)
    public Vector3 MoveDirection { get; private set; }
    public bool IsMoving => MoveDirection != Vector3.zero;

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
        // 입력은 Update에서 읽고, 물리 이동은 FixedUpdate에서 처리한다.
        ReadMoveInput();
    }

    private void FixedUpdate()
    {
        ApplyMoveVelocity();
    }

    private void ReadMoveInput()
    {
        Vector2 input = _joystick.Direction;
        if (input == Vector2.zero)
        {
            input = _moveAction.action.ReadValue<Vector2>();
        }

        // 화면 위쪽(y)을 월드 앞쪽(z)으로 대응시킨다.
        MoveDirection = new Vector3(input.x, 0f, input.y);
    }

    private void ApplyMoveVelocity()
    {
        // 낙하 속도(y)는 유지하고 수평 속도만 입력에 맞게 설정한다.
        Vector3 horizontalVelocity = MoveDirection * _moveSpeed;
        _rigidbody.linearVelocity = new Vector3(horizontalVelocity.x, _rigidbody.linearVelocity.y, horizontalVelocity.z);
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
