## 문서명: `261102FeatPlayerMovement`

## 1. 구현 일자
- 일자: 2026-11-02 (월요일)

## 2. Core Contents
- 구현 방향: InputAction(TouchPress/TouchPosition) → FloatingJoystick → PlayerMovement ← InputAction(Move) → Rigidbody / PlayerAnimation, CameraFollow
- 목표: 화면 어디든 터치하면 생기는 투명 가상 조이스틱으로 이동, WASD 보조 이동, Idle/Move 애니메이션과 좌우 반전, 카메라 추적

## 3. F-01 터치 가상 조이스틱
- What : 누른 지점을 중심으로 드래그 방향(길이 0~1)을 계산, 손을 떼면 0
- Where: `Assets/Scripts/Player/FloatingJoystick.cs`, `Assets/Input/PlayerControls.inputactions`
- Tune : `_maxDragDistance`, `_deadZoneDistance`

## 3. F-02 플레이어 이동
- What : 조이스틱 입력(우선) 또는 WASD를 Update에서 읽고, FixedUpdate에서 Rigidbody 속도로 XZ 평면 이동
- Where: `Assets/Scripts/Player/PlayerMovement.cs`
- Tune : `_moveSpeed`

## 3. F-03 플레이어 애니메이션
- What : `IsMoving` 값이 바뀔 때만 Animator 갱신, 오른쪽 이동 시 `flipX`
- Where: `Assets/Scripts/Player/PlayerAnimation.cs`, `Assets/Animations/Player/`
- Tune : 클립 frameRate(`Player_Idle.anim`, `Player_Move.anim`)

## 3. F-04 카메라 추적
- What : LateUpdate에서 대상 위치 + 오프셋으로 카메라 이동
- Where: `Assets/Scripts/Camera/CameraFollow.cs`
- Tune : `_offset`

## 4. 선택 이유
- XZ 평면 + Rigidbody(3D): 2.5D 장르에 맞추고 지형 충돌을 물리 엔진에 맡기기 위함
- 투명 조이스틱(UI 없음): 화면을 가리지 않고 어디서든 조작하기 위함, `<Pointer>` 바인딩으로 에디터 마우스 테스트 가능
- 조이스틱을 별도 컴포넌트로 분리: 입력 계산과 이동 처리를 나눠 단일 책임 유지
- 원본 스프라이트가 왼쪽을 향하므로 오른쪽 이동 시 반전
- 스프라이트를 카메라 각도(45°)만큼 기울임: 비스듬한 시점에서 납작해 보이지 않도록 하기 위함

## 5. Features 목록
| ID   | 기능 | 사용처 | 핵심 위치 |
|------|------|--------|-----------|
| F-01 | 터치 가상 조이스틱 | 터치 드래그 방향을 이동 입력으로 전달 | `Assets/Scripts/Player/` |
| F-02 | 플레이어 이동 | 조이스틱/WASD 입력으로 플레이어 이동 | `Assets/Scripts/Player/` |
| F-03 | 플레이어 애니메이션 | 이동 상태별 애니메이션, 좌우 반전 | `Assets/Scripts/Player/` |
| F-04 | 카메라 추적 | 비스듬한 탑다운 시점에서 플레이어 추적 | `Assets/Scripts/Camera/` |
