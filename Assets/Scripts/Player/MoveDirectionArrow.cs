using UnityEngine;
using UnityEngine.UI;

// 플레이어의 이동 방향을 화살표로 표시하고, 이동 중에는 페이드 인/아웃을 반복한다.
public class MoveDirectionArrow : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Image _arrowImage;
    [SerializeField] private Color _arrowColor = new Color(0.6f, 1f, 0.2f);
    [SerializeField] private float _fadeCycleSeconds = 1f;
    [SerializeField] private float _distanceFromPlayer = 1f;
    [SerializeField] private float _heightFromGround = -0.95f;

    // 바닥에 눕히기 위한 X축 회전값 (UI의 위쪽이 진행 방향을 향하도록 함)
    private static readonly Quaternion LAY_FLAT_ROTATION = Quaternion.Euler(90f, 0f, 0f);

    private float _fadeTimer;

    private void Update()
    {
        if (!_playerMovement.IsMoving)
        {
            _arrowImage.enabled = false;
            _fadeTimer = 0f;
            return;
        }

        _arrowImage.enabled = true;
        UpdateDirection(_playerMovement.MoveDirection);
        UpdateFade();
    }

    private void UpdateDirection(Vector3 moveDirection)
    {
        transform.localPosition = moveDirection * _distanceFromPlayer + Vector3.up * _heightFromGround;
        transform.rotation = Quaternion.LookRotation(moveDirection) * LAY_FLAT_ROTATION;
    }

    private void UpdateFade()
    {
        // 한 주기의 절반 동안 나타나고, 나머지 절반 동안 사라진다.
        _fadeTimer += Time.deltaTime;
        float alpha = Mathf.PingPong(_fadeTimer * 2f / _fadeCycleSeconds, 1f);

        Color color = _arrowColor;
        color.a = alpha;
        _arrowImage.color = color;
    }
}
