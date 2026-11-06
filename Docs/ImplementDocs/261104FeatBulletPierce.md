## 문서명: `261104FeatBulletPierce`

## 1. 구현 일자
- 일자: 2026-11-04 (수요일)

## 2. Core Contents
- 구현 방향: PlayerWeapon(IsPiercing) → PlayerAutoAttack.Fire(생성 위치·방향 1회 계산) → Bullet.Launch → MoveStraight / OnTriggerEnter
- 목표: 탄환을 유도 대신 발사 방향 직선 비행으로 바꾸고, 관통 무기의 탄환은 적을 맞혀도 사라지지 않고 뒤의 적까지 명중

## 3. F-09 직선 탄환
- What : 기존 유도 탄환에서 변경. 타겟 추적을 없애고, 발사 시 받은 방향으로 FixedUpdate마다 직선 이동. 만료는 수명(`_lifeTime`)만으로 판정해 바닥에 자국을 남기고 풀 반환. 명중 시 데미지 + 몸통 자국, 비관통이면 풀 반환
- Where: `Assets/Scripts/Weapon/Bullet.cs`, `Assets/Scripts/Player/PlayerAutoAttack.cs`
- Tune : `_moveSpeed`, `_damage`, `_lifeTime`, `_blinkSpeed`, `_brightColor`, `_dimColor`, TrailRenderer `time`

## 3. F-16 탄환 관통
- What : 무기에 관통 여부를 두고, 관통이면 생성 위치의 Y를 타겟 콜라이더 중심 높이로 맞춰 수평 발사. 탄환은 명중 후에도 풀로 돌아가지 않고 수명이 끝날 때까지 비행
- Where: `Assets/Scripts/Player/PlayerWeapon.cs`, `Assets/Scripts/Player/PlayerAutoAttack.cs`, `Assets/Scripts/Weapon/Bullet.cs`
- Tune : PlayerWeapon `_isPiercing`(테스트 단계로 인스펙터에서 켜고 끔), 탄환 비행 거리는 관통 여부와 관계없이 Bullet `_moveSpeed` x `_lifeTime`

## 4. 선택 이유
- 유도 대신 직선 비행: 유도를 유지하면 관통 후 다음 타겟 등 탄환이 가져야 할 정보와 처리할 함수가 늘어나 탄환의 역할이 단일화되지 않기 때문 (SRP)
- 관통 여부를 `PlayerWeapon`에 둠: 관통은 무기의 성질이기 때문. `PlayerAutoAttack`에 두면 의미가 약하고, Bullet 프리팹에 두면 관통/비관통 프리팹과 풀을 나눠야 함
- 방향을 쏘는 쪽(`PlayerAutoAttack.Fire`)에서 발사 시 1회 계산: Bullet이 타겟을 몰라도 되어, 비행 중 타겟이 죽거나 비활성화되어도 영향을 받지 않음
- 관통일 때 생성 위치 Y를 타겟 몸통 높이에 맞춤: 총구가 높으면 3D 방향이 아래로 꺾여 뒤에 있는 적의 몸통 높이를 지나지 못하므로, 수평으로 날아 뒤의 적까지 맞히기 위함
- 만료를 수명만으로 판정: 타겟 참조가 없어졌고, 관통 탄환이 사라지는 기준을 하나로 단순화하기 위함

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
| F-09 | 직선 탄환 | 발사 방향 직선 비행, 명중 시 데미지, 페인트볼 연출 | `Assets/Scripts/Weapon/` |
| F-10 | 적 체력 및 사망 | 피격 시 체력 감소, 0이면 비활성화 | `Assets/Scripts/Enemy/` |
| F-11 | 적 체력 UI | 적 머리 위 체력 게이지 표시 | `Assets/Scripts/Enemy/` |
| F-12 | 물감 자국 데칼 | 탄환 소멸 지점에 1초간 퍼지며 사라지는 자국 | `Assets/Scripts/Weapon/` |
| F-13 | 데칼 생성 및 풀링 | 명중 시 적 몸통, 수명 만료 시 바닥에 데칼 배치 | `Assets/Scripts/Weapon/` |
| F-14 | 적 데칼 수신 | 적 스프라이트에 물감 자국이 찍히도록 불투명 처리 | `Assets/Materials/` |
| F-15 | 플레이어 점프 | Space 입력 시 땅에 닿아 있을 때만 점프 | `Assets/Scripts/Player/` |
| F-16 | 탄환 관통 | 관통 무기 탄환이 적을 뚫고 뒤의 적까지 명중 | `Assets/Scripts/Player/`, `Assets/Scripts/Weapon/` |

## TODO
- 비관통 탄환은 총구에서 적 몸통 중심을 향해 아래로 기울어 발사되므로, 빗나가면 바닥 아래로 내려가 바닥 자국 위치가 어긋날 가능성 확인 (미검증)
- 탄환 비행 거리(`_moveSpeed` x `_lifeTime`)가 감지 범위(`_attackRange`)보다 김, 의도 확인 필요
- 관통 탄환이 같은 적을 다시 맞힐 가능성 확인 (적이 콜라이더를 벗어났다 재진입하는 경우) (미검증)
- 풀에서 꺼낸 첫 프레임의 Interpolate/Trail 잔상 가능성 확인 (미검증)
