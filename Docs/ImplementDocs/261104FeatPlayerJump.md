## 문서명: `261104FeatPlayerJump`

## 1. 구현 일자
- 일자: 2026-11-04 (수요일)

## 2. Core Contents
- 구현 방향: InputAction(Jump) → PlayerJump → Rigidbody(linearVelocity.y)
- 목표: Space 입력으로 땅에 닿아 있을 때만 점프

## 3. F-15 플레이어 점프
- What : Update에서 Jump 입력을 플래그로 받고, FixedUpdate에서 땅 판정이 참이면 Rigidbody의 y 속도만 지정
- Where: `Assets/Scripts/Player/PlayerJump.cs`, `Assets/Input/PlayerControls.inputactions`
- Tune : `_jumpVelocity`, `_groundCheckRadius`, `_groundCheckPoint`(Player 자식 `GroundCheck`) 위치

## 4. 선택 이유
- 별도 컴포넌트로 분리: 점프만 담당해 단일 책임 유지, `PlayerMovement`는 수정하지 않음. 두 컴포넌트가 각자 자기 축만 쓰므로(y는 Jump, x/z는 Movement) `linearVelocity`를 함께 써도 충돌하지 않음
- `Physics.CheckSphere`로 땅 판정: 발이 가장자리에 걸쳐도 관대하게 판정하고, 반경으로 범위를 조절하며, `Ground` 레이어 마스크로 땅만 검사하기 위함
- `linearVelocity.y`를 직접 지정: 힘(AddForce) 대신 속도를 정해 질량과 관계없이 항상 같은 높이로 뛰도록 하기 위함
- 공중 입력은 저장하지 않고 버림: 공중에서 누른 입력이 남아 착지 직후 자동으로 점프하는 것을 막기 위함
- 입력은 Update에서 플래그로 받고 FixedUpdate에서 소비: 프레임마다 읽어야 하는 입력과 물리 주기에 맞춰야 하는 속도 변경을 나누기 위함
- `GroundCheck`를 CapsuleCollider 바닥면에 배치: 판정 구가 발끝(캡슐 바닥면)에 오도록 하기 위함. 바닥면 위치는 `Assets/New Scene.unity`의 Player CapsuleCollider(Center, Height) 값을 따른다

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
| F-15 | 플레이어 점프 | Space 입력 시 땅에 닿아 있을 때만 점프 | `Assets/Scripts/Player/` |

## TODO
- 점프 직후 CheckSphere가 아직 땅과 겹쳐 연타 시 2단 점프가 될 가능성 확인 (미검증)
- 입력 버퍼 (착지 직전 입력을 짧게 기억)
- 점프 애니메이션
