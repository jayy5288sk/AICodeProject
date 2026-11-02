using UnityEngine;
using UnityEngine.Pool;

// 물감 자국 데칼을 ObjectPool로 재사용해 바닥 또는 적 몸통에 찍어 준다.
public class PaintSplatSpawner : MonoBehaviour
{
    // 바닥에 찍을 때는 위에서 아래로 투영한다.
    private static readonly Quaternion GROUND_ROTATION = Quaternion.Euler(90f, 0f, 0f);

    [SerializeField] private PaintSplat _paintSplatPrefab;
    // 몸통에 찍을 때는 카메라가 바라보는 방향으로 투영해 화면 정면으로 보이게 한다.
    [SerializeField] private Transform _cameraTransform;
    // 투영 깊이: 바닥은 공중 위치에서도 닿도록 깊게, 몸통은 뒤쪽 바닥까지 번지지 않도록 얕게
    [SerializeField] private float _groundProjectionDepth = 2f;
    [SerializeField] private float _bodyProjectionDepth = 0.5f;

    [Header("데칼 풀")]
    [SerializeField] private int _poolDefaultCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;

    private ObjectPool<PaintSplat> _paintSplatPool;

    private void Awake()
    {
        _paintSplatPool = new ObjectPool<PaintSplat>(CreatePaintSplat, OnGetPaintSplat, OnReleasePaintSplat, OnDestroyPaintSplat,
            true, _poolDefaultCapacity, _poolMaxSize);
    }

    public void SpawnOnGround(Vector3 position)
    {
        Spawn(position, GROUND_ROTATION, _groundProjectionDepth);
    }

    public void SpawnOnBody(Vector3 position)
    {
        Spawn(position, _cameraTransform.rotation, _bodyProjectionDepth);
    }

    private void Spawn(Vector3 position, Quaternion rotation, float projectionDepth)
    {
        PaintSplat paintSplat = _paintSplatPool.Get();
        paintSplat.Play(position, rotation, projectionDepth);
    }

    private PaintSplat CreatePaintSplat()
    {
        PaintSplat paintSplat = Instantiate(_paintSplatPrefab);
        paintSplat.Initialize(_paintSplatPool.Release);
        return paintSplat;
    }

    private void OnGetPaintSplat(PaintSplat paintSplat)
    {
        paintSplat.gameObject.SetActive(true);
    }

    private void OnReleasePaintSplat(PaintSplat paintSplat)
    {
        paintSplat.gameObject.SetActive(false);
    }

    private void OnDestroyPaintSplat(PaintSplat paintSplat)
    {
        Destroy(paintSplat.gameObject);
    }
}
