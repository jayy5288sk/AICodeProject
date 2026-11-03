using UnityEngine;

// 이동 상태에 따라 Idle/Move 애니메이션을 전환하고, 좌우 방향에 맞게 스프라이트를 뒤집는다.
public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IS_MOVING_HASH = Animator.StringToHash("IsMoving");

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool _wasMoving;

    private void Update()
    {
        UpdateMoveAnimation();
        UpdateFacing();
    }

    // 이동 상태가 바뀔 때만 Animator 값을 갱신한다.
    private void UpdateMoveAnimation()
    {
        bool isMoving = _playerMovement.IsMoving;
        if (isMoving != _wasMoving)
        {
            _animator.SetBool(IS_MOVING_HASH, isMoving);
            _wasMoving = isMoving;
        }
    }

    private void UpdateFacing()
    {
        // 좌우 입력이 있을 때만 방향을 바꾸고, 위/아래 이동 시에는 마지막 방향을 유지한다.
        float horizontal = _playerMovement.MoveDirection.x;
        if (horizontal != 0f)
        {
            // 원본 스프라이트는 꼬리가 오른쪽에 있어 왼쪽을 향하므로, 오른쪽 이동 시 뒤집는다.
            _spriteRenderer.flipX = horizontal > 0f;
        }
    }
}
