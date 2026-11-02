## 문서명: `261102FeatBullet`

## 1. 구현 일자
- 일자: 2026-11-02 (월요일)

## 2. Core Contents
- 구현 방향: Bullet(명중 / 수명 만료) → PaintSplatSpawner(ObjectPool) → PaintSplat(DecalProjector)
- 목표: 빨간 페인트볼 탄환이 사라질 때 명중 시 적 몸통, 수명 만료 시 바닥에 물감 자국을 1초간 남김

## 3. F-12 물감 자국 데칼
- What : 1초 동안 DecalProjector의 size를 키우고 fadeFactor를 낮춰 퍼지듯 사라진 뒤 풀 반환
- Where: `Assets/Scripts/Weapon/PaintSplat.cs`, `Assets/Prefabs/Bullet/PFB_PaintSplat.prefab`, `Assets/Materials/MAT_PaintSplat.mat`
- Tune : `_duration`, `_startSize`, `_endSize`

## 3. F-13 데칼 생성 및 풀링
- What : 바닥(위→아래 투영) / 몸통(카메라 방향 투영) 두 방식으로 데칼을 풀에서 꺼내 배치
- Where: `Assets/Scripts/Weapon/PaintSplatSpawner.cs`, `Assets/Scripts/Weapon/Bullet.cs`
- Tune : `_groundProjectionDepth`, `_bodyProjectionDepth`, `_poolDefaultCapacity`, `_poolMaxSize`

## 3. F-14 적 데칼 수신
- What : 적 스프라이트를 URP/Lit + Alpha Clipping 불투명 머티리얼로 그려 데칼이 찍히도록 함
- Where: `Assets/Materials/MAT_Enemy.mat`, `Assets/Prefabs/Enemy/PFB_Enemy.prefab`
- Tune : `MAT_Enemy`의 Alpha Clipping Threshold

## 4. 선택 이유
- URP Decal: 바닥·적 표면에 이미지를 투영해 별도 메시 없이 자국 표현 (URP 에셋 미연결 상태였으므로 `Assets/Settings/`에 신규 생성·등록)
- ObjectPool<PaintSplat>: 탄환마다 생성되는 데칼의 반복 Instantiate/Destroy 방지 (안티패턴 문서)
- Spawner 분리: 적 탐색·발사(PlayerAutoAttack)와 데칼 관리의 책임 분리 (SRP)
- 몸통 데칼은 카메라 방향 투영: 비스듬한 탑다운 시점에서 항상 정면으로 보이게 하기 위함
- 몸통 투영 깊이를 얕게: 적 뒤쪽 바닥까지 자국이 번지지 않게 하기 위함
- 적 머티리얼 불투명화: URP 데칼은 반투명(Sprites/Default) 표면에 찍히지 않기 때문
- URP/Lit 선택: Unlit의 데칼 수신 여부가 불확실해 지원이 확실한 Lit 사용

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
| F-09 | 유도 탄환 | 타겟 추적, 명중 시 데미지, 빨간 페인트볼 연출 | `Assets/Scripts/Weapon/` |
| F-10 | 적 체력 및 사망 | 피격 시 체력 감소, 0이면 비활성화 | `Assets/Scripts/Enemy/` |
| F-11 | 적 체력 UI | 적 머리 위 체력 게이지 표시 | `Assets/Scripts/Enemy/` |
| F-12 | 물감 자국 데칼 | 탄환 소멸 지점에 1초간 퍼지며 사라지는 자국 | `Assets/Scripts/Weapon/` |
| F-13 | 데칼 생성 및 풀링 | 명중 시 적 몸통, 수명 만료 시 바닥에 데칼 배치 | `Assets/Scripts/Weapon/` |
| F-14 | 적 데칼 수신 | 적 스프라이트에 물감 자국이 찍히도록 불투명 처리 | `Assets/Materials/` |

## TODO
- 적 스프라이트 교체 시 `MAT_Enemy` Base Map 수동 갱신 필요
