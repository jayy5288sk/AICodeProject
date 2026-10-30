# Game Specification Document Rule

> 목적: 코드와 문서의 싱크를 유지하는 누적형 기술 문서이다.
> 
> 기준은 3분 이내로 읽고 구조를 구현할 수 있어야 한다.

---

## 원칙

1. **One-Page**: 문서는 한 화면(약 80줄) 안에 유지한다. 넘치면 요약하거나 `archive/`로 이동한다.
2. **Loop First**: 핵심 루프와 필수 기능만 기록한다. 부가 기능과 아이디어는 `TODO`에 한 줄로만 남긴다.
3. **Why > What**: 코드를 보면 알 수 있는 것(What)은 줄이고, 결정 이유(Why)와 위치(Where)를 남긴다.
4. **Same Commit**: 기능 구현 직후, **같은 커밋**에 문서 변경을 포함한다. 문서만 나중에 몰아서 쓰지 않는다.
5. **Single Source**: 수치는 코드/데이터 파일이 원본이다. 문서에는 값 자체보다 **파일 경로**를 우선 적는다.

---

## 파일 구조

```

```

---

## 3. GAME_SPEC.md 템플릿

```markdown
# [게임명] Spec
Last updated: YYYY-MM-DD | Version: 0.x

## 1. Core Loop
한 줄 요약: (예) 탐색 → 전투 → 보상 → 성장 → 탐색
- 루프 1회 소요 시간 / 플레이어 목표:

## 2. Features
| ID   | 기능          | 상태            | 핵심 위치(코드)            | 커밋  |
|------|---------------|-----------------|----------------------------|-------|
| F-01 | 플레이어 이동 | ✅ Done         | `src/player/Move.cs`       | a1b2c3 |
| F-02 | 기본 공격     | 🚧 In Progress  | `src/combat/Attack.cs`     | d4e5f6 |
| F-03 | 보상 드롭     | ⬜ Planned      | -                          | -     |

## 3. Key Rules
- 데미지 공식: `ATK - DEF` (최소 1) → `Balance.json`
- 핵심 제약/규칙을 1~2줄로

## 4. Decisions (최근 10개만 유지)
- YYYY-MM-DD | F-02 | 결정: ... | 이유: ...

## 5. TODO / Known Issues
- [ ] ...
```

---

## 4. 기능 노트 (기능 완료 시 추가하는 5줄)

기능 구현 직후 `Features` 표를 갱신하고, 필요한 경우에만 `Decisions`에 아래 형식으로 한 줄~다섯 줄을 추가한다.

```markdown
### F-02 기본 공격
- What : 근접 판정 후 데미지 적용
- Why  : 물리 충돌 대신 거리 판정으로 단순화 (성능/디버깅 용이)
- Where: `src/combat/Attack.cs`, `Balance.json`
- Tune : 공격 범위, 쿨타임
- Next : 원거리 공격 추가 시 판정 로직 분리 필요
```

---

## 5. 커밋 메시지 연동

**형식**

```
<type>(<scope>): <요약> [F-xx]

Spec: docs/GAME_SPEC.md#F-xx
```

**type**: `feat` 기능 / `fix` 버그 / `refactor` 구조 개선 / `balance` 수치 조정 / `docs` 문서만 수정

**예시**

```
feat(combat): 근접 기본 공격 구현 [F-02]

Spec: docs/GAME_SPEC.md#F-02
```

**규칙**
- `feat`, `balance`, `refactor` 커밋에는 `GAME_SPEC.md` 변경을 **반드시 같이** 포함한다.
- 문서 갱신이 필요 없는 변경(오타, 포맷)은 커밋 메시지에 `[no-spec]`을 붙인다.
- Features 표의 `커밋` 열에는 해당 기능의 **완료 커밋 해시**를 적는다.

---

## 6. 커밋 전 체크리스트

- [ ] Features 표의 상태와 코드 위치가 현재와 일치하는가?
- [ ] 이번 변경으로 Core Loop나 Key Rules가 바뀌었는가?
- [ ] 중요한 결정이 있었다면 Why를 한 줄로 남겼는가?
- [ ] 문서가 한 화면(약 80줄)을 넘지 않는가?
- [ ] 커밋 메시지에 `[F-xx]`와 `Spec:` 링크가 있는가?

---

## 7. 정리 규칙 (문서 비대화 방지)

| 항목 | 기준 | 처리 |
|------|------|------|
| Decisions | 10개 초과 | 오래된 것부터 `archive/`로 이동 |
| 완료 기능 | 안정화 완료 | 표에서 유지하되 노트는 삭제 |
| 폐기 기능 | 사용 안 함 | 표에서 제거 후 `archive/`에 사유 1줄 |
| TODO | 3개월 이상 미처리 | 삭제 또는 재평가 |
