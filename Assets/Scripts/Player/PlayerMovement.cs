using UnityEngine;
using UnityEngine.InputSystem;

// WASD 입력을 받아 Rigidbody로 플레이어를 X/Z 평면에서 이동시킨다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody _rigidbody;
    private Vector2 _moveInput;

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
        // 입력은 매 프레임 받아 두고, 실제 이동은 FixedUpdate에서 처리한다.
        _moveInput = _moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Move()
    {
        // 입력의 y값은 3D 공간의 z축(앞뒤)에 대응한다. 중력이 유지되도록 y 속도는 그대로 둔다.
        Vector3 velocity = new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveSpeed;
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }
}
