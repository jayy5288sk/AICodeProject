using UnityEngine;

// 대상과 일정한 오프셋을 유지하며 카메라를 따라 움직인다.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 8f, -8f);

    private void LateUpdate()
    {
        // 대상의 이동이 끝난 뒤 위치를 맞추기 위해 LateUpdate에서 처리한다.
        transform.position = _target.position + _offset;
    }
}
