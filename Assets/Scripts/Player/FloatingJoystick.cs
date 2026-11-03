using UnityEngine;
using UnityEngine.InputSystem;

// 화면을 누른 지점을 중심으로 하는 보이지 않는 조이스틱. 드래그 방향을 계산한다.
public class FloatingJoystick : MonoBehaviour
{
    [SerializeField] private InputActionReference _touchPressAction;
    [SerializeField] private InputActionReference _touchPositionAction;
    // 이 거리(픽셀)만큼 드래그하면 최대 입력(길이 1)이 된다.
    [SerializeField] private float _maxDragDistance = 100f;
    // 이 거리(픽셀) 미만의 드래그는 손떨림으로 보고 무시한다.
    [SerializeField] private float _deadZoneDistance = 10f;

    private Vector2 _touchStartPosition;

    // 드래그 방향 (화면 기준, 길이 0~1). 누르지 않으면 Vector2.zero
    public Vector2 Direction { get; private set; }

    private void OnEnable()
    {
        _touchPressAction.action.Enable();
        _touchPositionAction.action.Enable();
        _touchPressAction.action.started += OnTouchStarted;
        _touchPressAction.action.canceled += OnTouchCanceled;
    }

    private void OnDisable()
    {
        _touchPressAction.action.started -= OnTouchStarted;
        _touchPressAction.action.canceled -= OnTouchCanceled;
        _touchPressAction.action.Disable();
        _touchPositionAction.action.Disable();
        Direction = Vector2.zero;
    }

    private void Update()
    {
        UpdateDirection();
    }

    // 누르고 있는 동안 시작 지점 기준 드래그 방향을 계산한다.
    private void UpdateDirection()
    {
        if (!_touchPressAction.action.IsPressed())
        {
            return;
        }

        Vector2 dragOffset = _touchPositionAction.action.ReadValue<Vector2>() - _touchStartPosition;
        if (dragOffset.magnitude < _deadZoneDistance)
        {
            Direction = Vector2.zero;
            return;
        }

        Direction = Vector2.ClampMagnitude(dragOffset / _maxDragDistance, 1f);
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        // 누른 순간의 위치를 조이스틱 중심으로 삼는다.
        _touchStartPosition = _touchPositionAction.action.ReadValue<Vector2>();
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        Direction = Vector2.zero;
    }
}
