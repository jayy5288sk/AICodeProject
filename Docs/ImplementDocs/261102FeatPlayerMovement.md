## 문서명: `261102FeatPlayerMovement`

## 1. 구현 일자
- 일자: 2026-11-02 (월요일)

## 2. Core Contents
- 구현 방향: InputAction(Move) → PlayerMovement → Rigidbody / MoveDirectionArrow, CameraFollow
- 목표: WASD 이동과 이동 방향 표시(연두색 화살표, 1초 주기 페이드), 카메라 추적

## 3. F-01 플레이어 이동
- What : WASD 입력을 Update에서 읽고 FixedUpdate에서 Rigidbody 속도로 이동
- Where: `Assets/Scripts/Player/PlayerMovement.cs`, `Assets/Input/PlayerControls.inputactions`
- Tune : `_moveSpeed`

## 3. F-02 이동 방향 화살표
- What : 이동 중 월드 공간 화살표가 이동 방향을 가리키며 페이드 인/아웃, 정지 시 숨김
- Where: `Assets/Scripts/Player/MoveDirectionArrow.cs`, `Assets/Sprites/Arrow.png`
- Tune : `_arrowColor`, `_fadeCycleSeconds`, `_distanceFromPlayer`, `_heightFromGround`

## 3. F-03 카메라 추적
- What : LateUpdate에서 대상 위치 + 오프셋으로 카메라 이동
- Where: `Assets/Scripts/Camera/CameraFollow.cs`
- Tune : `_offset`

## 4. 선택 이유
- Rigidbody: 벽, 지형 충돌을 물리 엔진에 맡기기 위함
- 월드 공간 화살표: 플레이어를 따라다니며 바닥에서 방향을 직관적으로 보여주기 위함
- 페이드: 단순 켜짐/꺼짐보다 부드럽게 보이도록 하기 위함
- 화살표는 PlayerMovement를 직접 참조(SerializeField): 참조 대상이 하나뿐이라 이벤트 구조 없이 단순하게 구성

## 5. Features 목록
| ID   | 기능 | 사용처 | 핵심 위치 |
|------|------|--------|-----------|
| F-01 | 플레이어 이동 | WASD 입력으로 플레이어 이동 | `Assets/Scripts/Player/` |
| F-02 | 이동 방향 화살표 | 이동 중 플레이어 진행 방향 시각화 | `Assets/Scripts/Player/` |
| F-03 | 카메라 추적 | 탑다운 시점에서 플레이어 추적 | `Assets/Scripts/Camera/` |
