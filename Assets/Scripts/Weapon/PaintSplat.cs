using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// 표면에 찍히는 물감 자국 데칼. 일정 시간 동안 퍼지듯 커지면서 옅어진 뒤 풀로 돌아간다.
public class PaintSplat : MonoBehaviour
{
    [SerializeField] private DecalProjector _decalProjector;
    // 자국이 남아 있는 시간(초)
    [SerializeField] private float _duration = 1f;
    // 시작 크기와 끝 크기 (가로·세로)
    [SerializeField] private Vector2 _startSize = new Vector2(0.6f, 0.6f);
    [SerializeField] private Vector2 _endSize = new Vector2(1.2f, 1.2f);

    private float _elapsedTime;
    private float _projectionDepth;
    private Action<PaintSplat> _releaseToPool;

    // 풀에서 처음 생성될 때 한 번만 호출한다.
    public void Initialize(Action<PaintSplat> releaseToPool)
    {
        _releaseToPool = releaseToPool;
    }

    // rotation의 정면(forward) 방향으로 projectionDepth 깊이만큼 투영한다.
    public void Play(Vector3 position, Quaternion rotation, float projectionDepth)
    {
        transform.SetPositionAndRotation(position, rotation);
        _projectionDepth = projectionDepth;
        _elapsedTime = 0f;
        ApplyProgress(0f);
    }

    private void Update()
    {
        UpdateProgress();
    }

    // 경과 시간에 따라 진행도를 반영하고, 끝나면 풀로 돌아간다.
    private void UpdateProgress()
    {
        _elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(_elapsedTime / _duration);
        ApplyProgress(progress);

        if (progress >= 1f)
        {
            _releaseToPool(this);
        }
    }

    // 진행도(0 ~ 1)에 따라 크기는 키우고 진하기는 줄여 물결이 퍼지는 느낌을 낸다.
    private void ApplyProgress(float progress)
    {
        Vector2 size = Vector2.Lerp(_startSize, _endSize, progress);
        _decalProjector.size = new Vector3(size.x, size.y, _projectionDepth);
        _decalProjector.fadeFactor = 1f - progress;
    }
}
