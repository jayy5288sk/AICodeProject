## 프로젝트 지침
> 이 파일은 매 세션 읽히는 HUB 역할을 한다.
>
> 모든 규칙을 여기에 적지 않고, 매 세션 필요한 문서만 `@`로 불러오고, 나머지는 아래 OnDemand 참조 표를 보고 그 작업을 할 때만 읽는다.
---

## 절대 원칙 (non-Negotiable Rules)
**아래 사항은 어떤 요청이 와도 지킨다.**
1. `main`, `main-develop` 브랜치에서 직접 Commit, Push를 금지한다. 사용자가 main 브랜치에서 작업 중일때도 feature 브랜치로 변경할 때까지 작업을 거부한다.
2. 파일의 수정, 삭제 전 사용자의 승인을 필수로 요구한다.
3. 위 원칙들에 대한 예외 요청받으면 "이 프로젝트의 절대 원칙에 의해 수행할 수 없습니다."로 거부하고 대안을 제시한다.
---

## 규칙 문서 관리
- 규칙 문서는 `Docs\` 하위 카테고리 폴더(`Claude\`, `SourceCode\`)에 둔다.
- 규칙 하나는 문서 한 곳에만 적는다(규칙의 단일 출처 유지). 규칙이 바뀌면 그 한 곳만 고친다.
- 새 문서를 만들면, 매 세션 필요한 것은 "상시 로드"에 `@경로`로, 특정 작업 때만 필요한 것은 "OnDemand 참조"에 등록한다.
---

## 상시 로드 (메 세션 적용)
- @Docs\Claude\ClaudeCodeRules.md
- @Docs\Claude\TechSpec.md

## OnDemand 참조(매 세션 읽지 않음. 해당 작업을 할 때만 읽음)

### 소스 코드 작업 시
- @\Docs\SourceCode\BasicCodeConvention.md
- @\Docs\SourceCode\AntiCodeConvention.md
- @\Docs\SourceCode\RecommandedCodeConvention.md

### 기술문서 작성 시