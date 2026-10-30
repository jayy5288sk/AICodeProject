using UnityEngine;
using UnityEngine.InputSystem;

// WASD 입력을 받아 Rigidbody로 플레이어를 월드 기준 이동·회전시킨다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private float _moveSpeed = 5f;
    // 초당 회전 각도(도)
    [SerializeField] private float _rotationSpeed = 720f;

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
        // 입력은 Update에서 읽고, 물리 처리는 FixedUpdate에서 사용한다.
        Vector2 input = _moveAction.action.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, 0f, input.y);
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
}
