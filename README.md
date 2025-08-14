# CollectingRPG

## ■ 사용한 외부 라이브러리

UniTask : https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask


## ■ 네이밍 규칙
| 대상 | 규칙 | 예시 |
|------|------|------|
| 클래스명 | PascalCase | `PlayerController`, `GameManager` |
| public 변수 | PascalCase | `MaxHealth`, `PlayerCount` |
| private 변수 | `_camelCase` (접두사 `_`) | `_currentHealth`, `_moveSpeed` |
| 매개변수 | `camelCase` | `currentHealth`, `moveSpeed` |
| 상수 | UPPER_CASE | `MAX_LEVEL`, `DEFAULT_SPEED` |
| async 메서드 | PascalCase + `Async` | `LoadDataAsync()`, `SaveGameAsync()` |

### Branch 네이밍
- 개발 시 : feat/unit
- 수정 시 : fix/unit 
처럼 작성한다.

## ■ 커밋 규칙

|유형|내용|
|-|-|
|Feat|새로운 기능 추가를 한 경우|
|Fix|버그를 수정을 한 경우|
|Build|빌드 관련내용을 수정한 경우 (Project Setting)|
|Test|테스트 Scene또는 코드를 추가한 경우|
|Refactor|코드를 리펙토링한 경우|
|Docs|주석이나 문서를 수정한 경우 (README 등)|
|Release|버전을 릴리즈한 경우|
|Create|프로젝트를 생성한 경우|
|Chore|간단한 수정이 일어난 경우|

## ■ 기타 규칙

- Unity LifeCycle은 별도로 모아둔다.
- Workspace 내의 자신의 이니셜 폴더에서 작업한다.
- 서로 다른 씬에서 작업한다.
- 클래스 하나가 너무 많은 기능을 가지지 않도록 하는 것을 지향한다.
  - 예 : PlayerController → 입력 처리와 이동 로직만
- ScriptableObject를 사용하여 게임 밸런스 수치, 설정값, 리소스 정보를 관리한다.
  - ```cs
    public class Stats : ScriptableObject
    {
      public int MaxHealth;
      public float MoveSpeed;
    }
    ```
- 역할은 인터페이스로 정의, 구현은 클래스에서 한다.
- 데이터와 로직의 분리를 지향한다.
