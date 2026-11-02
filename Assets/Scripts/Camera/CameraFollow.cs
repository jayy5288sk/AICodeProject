using UnityEngine;

// 대상을 일정한 거리(오프셋)를 유지하며 따라간다.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 10f, -6f);

    private void LateUpdate()
    {
        // 대상의 이동이 끝난 뒤에 카메라 위치를 갱신한다.
        transform.position = _target.position + _offset;
    }
}
