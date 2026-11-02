## 문서명: `261102FeatPlayerWeapon`

## 1. 구현 일자
- 일자: 2026-11-02 (월요일)

## 2. Core Contents
- 구현 방향: PlayerMovement(MoveDirection) → PlayerWeapon → WeaponPivot 회전 / WeaponGraphic 반전 / MuzzlePoint
- 목표: 조준 방향으로 회전하는 무기, 좌우 이동 시 위아래 뒤집힘 방지, 교체 가능한 스프라이트와 자동 정렬되는 총구 위치

## 3. F-05 무기 조준 및 반전
- What : 조준 방향(타겟이 있으면 타겟, 없으면 이동 방향, F-07 참조)의 XZ 각도로 Pivot을 회전하고, 왼쪽(x < 0)을 향하면 Graphic의 `localScale.y = -1`로 위아래를 바로 세움
- Where: `Assets/Scripts/Player/PlayerWeapon.cs`, `Assets/New Scene.unity` (Player/WeaponPivot)
- Tune : `_tiltAngle`

## 3. F-06 무기 스프라이트 교체 및 총구
- What : SpriteRenderer의 스프라이트가 바뀌면 MuzzlePoint를 스프라이트 오른쪽 끝 중앙으로 정렬, `MuzzlePoint` 프로퍼티로 제공
- Where: `Assets/Scripts/Player/PlayerWeapon.cs`, `Assets/Sprites/Weapon/`
- Tune : WeaponGraphic 위치(손잡이 오프셋), 스프라이트 PPU

## 4. 선택 이유
- Pivot / Graphic 분리: 회전(Pivot)과 반전·스프라이트(Graphic)의 책임을 나눠 스프라이트 교체에 영향을 주지 않기 위함
- `flipY` 대신 `localScale.y` 반전: 자식인 MuzzlePoint까지 함께 반전되어 총구 위치를 따로 계산하지 않기 위함
- Euler(기울기, 0, 조준각): 플레이어 스프라이트와 같은 45° 평면 안에서 회전시켜 비스듬한 시점에서도 자연스럽게 보이도록 하기 위함
- 임시 조준을 이동 방향으로: 적이 없는 상태에서 추가 입력 없이 확인하고, 이후 타겟 방향으로 쉽게 교체하기 위함
- 총구 자동 정렬: 스프라이트 교체 시 추가 설정을 없애기 위함 (규칙: 총구가 오른쪽 끝 중앙인 스프라이트)

## 5. Features 목록
| ID   | 기능 | 사용처 | 핵심 위치 |
|------|------|--------|-----------|
| F-01 | 터치 가상 조이스틱 | 터치 드래그 방향을 이동 입력으로 전달 | `Assets/Scripts/Player/` |
| F-02 | 플레이어 이동 | 조이스틱/WASD 입력으로 플레이어 이동 | `Assets/Scripts/Player/` |
| F-03 | 플레이어 애니메이션 | 이동 상태별 애니메이션, 좌우 반전 | `Assets/Scripts/Player/` |
| F-04 | 카메라 추적 | 비스듬한 탑다운 시점에서 플레이어 추적 | `Assets/Scripts/Camera/` |
| F-05 | 무기 조준 및 반전 | 조준 방향 회전, 좌우 시 위아래 뒤집힘 방지 | `Assets/Scripts/Player/` |
| F-06 | 무기 교체 및 총구 | 스프라이트 교체 시 총구 위치 자동 정렬 | `Assets/Scripts/Player/` |
