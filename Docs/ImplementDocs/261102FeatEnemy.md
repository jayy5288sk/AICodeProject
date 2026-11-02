## 문서명: `261102FeatEnemy`

## 1. 구현 일자
- 일자: 2026-11-02 (월요일)

## 2. Core Contents
- 구현 방향: PlayerAutoAttack(OverlapSphere, Enemy Layer) → CurrentTarget → PlayerWeapon 조준 / ObjectPool<Bullet> → Bullet → EnemyHealth → EnemyHealthBar
- 목표: 사거리 내 가장 가까운 적 자동 조준·발사, 유도 탄환 명중 시 체력 감소, 체력 0에서 적 비활성화

## 3. F-07 오토 타겟팅
- What : 매 프레임 사거리 내 Enemy Layer 콜라이더 중 가장 가까운 적을 `CurrentTarget`으로 지정, PlayerWeapon은 타겟이 있으면 타겟 방향(XZ)을 조준
- Where: `Assets/Scripts/Player/PlayerAutoAttack.cs`, `Assets/Scripts/Player/PlayerWeapon.cs`
- Tune : `_attackRange`, `_enemyLayer`

## 3. F-08 자동 발사
- What : 타겟이 있을 때 발사 간격마다 풀에서 탄환을 꺼내 MuzzlePoint에서 발사
- Where: `Assets/Scripts/Player/PlayerAutoAttack.cs`, `Assets/Prefabs/Bullet/PFB_Bullet.prefab`
- Tune : `_fireInterval`, `_poolDefaultCapacity`, `_poolMaxSize`

## 3. F-09 유도 탄환
- What : FixedUpdate에서 타겟 콜라이더 중심으로 이동, Trigger로 명중 판정 후 풀 반환. 색·투명도 깜빡임 + Trail 잔상
- Where: `Assets/Scripts/Weapon/Bullet.cs`, `Assets/Sprites/Bullet/`
- Tune : `_moveSpeed`, `_damage`, `_lifeTime`, `_blinkSpeed`, `_brightColor`, `_dimColor`, TrailRenderer `time`

## 3. F-10 적 체력 및 사망
- What : 피격 시 체력 감소와 `OnHealthChanged`(비율) 알림, 0이 되면 `SetActive(false)`
- Where: `Assets/Scripts/Enemy/EnemyHealth.cs`, `Assets/Prefabs/Enemy/PFB_Enemy.prefab`
- Tune : `_maxHealth`

## 3. F-11 적 체력 UI
- What : World Space Canvas의 Filled Image를 `OnHealthChanged` 이벤트가 올 때만 갱신
- Where: `Assets/Scripts/Enemy/EnemyHealthBar.cs`, `PFB_Enemy/HealthBarCanvas`
- Tune : HealthBarCanvas 위치·크기

## 4. 선택 이유
- OverlapSphereNonAlloc: 결과 배열을 재사용해 매 프레임 탐색에도 GC 할당 없음
- ObjectPool<Bullet>: 반복 Instantiate/Destroy를 피하기 위함 (안티패턴 문서)
- 유도탄 + 콜라이더 중심 추적: 스프라이트 피벗이 발밑이라 피벗 대신 몸통 중앙을 맞추기 위함
- Kinematic Rigidbody + Trigger: 물리 반동 없이 명중 이벤트만 받기 위함
- 사망 시 SetActive(false): 비활성 콜라이더는 탐색에서 자동 제외되고, 이후 적 풀링·재생성에 활용하기 위함
- 이벤트 기반 체력 UI: 값이 바뀔 때만 갱신, 체력과 UI의 결합을 느슨하게 유지
- `_isReleased` 가드: 명중과 수명 만료가 같은 프레임에 겹쳐 풀에 중복 반환되는 것을 막기 위함

## 5. Features 목록
| ID   | 기능 | 사용처 | 핵심 위치 |
|------|------|--------|-----------|
| F-01 | 터치 가상 조이스틱 | 터치 드래그 방향을 이동 입력으로 전달 | `Assets/Scripts/Player/` |
| F-02 | 플레이어 이동 | 조이스틱/WASD 입력으로 플레이어 이동 | `Assets/Scripts/Player/` |
| F-03 | 플레이어 애니메이션 | 이동 상태별 애니메이션, 좌우 반전 | `Assets/Scripts/Player/` |
| F-04 | 카메라 추적 | 비스듬한 탑다운 시점에서 플레이어 추적 | `Assets/Scripts/Camera/` |
| F-05 | 무기 조준 및 반전 | 조준 방향 회전, 좌우 시 위아래 뒤집힘 방지 | `Assets/Scripts/Player/` |
| F-06 | 무기 교체 및 총구 | 스프라이트 교체 시 총구 위치 자동 정렬 | `Assets/Scripts/Player/` |
| F-07 | 오토 타겟팅 | 사거리 내 가장 가까운 적 선택 및 조준 | `Assets/Scripts/Player/` |
| F-08 | 자동 발사 | 발사 간격마다 풀의 탄환을 총구에서 발사 | `Assets/Scripts/Player/` |
| F-09 | 유도 탄환 | 타겟 추적, 명중 시 데미지, 반짝임 연출 | `Assets/Scripts/Weapon/` |
| F-10 | 적 체력 및 사망 | 피격 시 체력 감소, 0이면 비활성화 | `Assets/Scripts/Enemy/` |
| F-11 | 적 체력 UI | 적 머리 위 체력 게이지 표시 | `Assets/Scripts/Enemy/` |

## TODO
- 적 이동 / 적 스폰(풀링) 미구현
