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

## 프로젝트 설명

**“피플러그”** 기업과 협약으로 제작된 수집형, RPG, 로그라이트, 전략 게임입니다.

---

## 개발 환경

개발기간 : 8월 14일 ~ 10월 16일

개발 인원 : 12인 (기획 6인, 개발 6인)

개발 툴 : Unity Engine 2022.3.61f1, Visual Studio 2022, GitHub

---

## 관련 링크

PPT :  [최종발표 자료](https://docs.google.com/presentation/d/1VNfRTZuF3s_Y4PFqcaacj_iLarDAramcOQRXdKTAVAw/edit?slide=id.g394a073ca7f_2_75#slide=id.g394a073ca7f_2_75)

Notion : [https://github.com/HSD06040/CollectingRPG](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?source=copy_link)

시연영상 : https://www.youtube.com/watch?v=7mxD-0bo5eE

---

## 목차

- **구현**
    1. [**스킬 구현**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    2. [**협업을 위한 커스텀 에디터 구성**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    3. [**Addressable 비동기 로드 시스템**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    4. [**유닛 패시브 관리**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    5. [**시너지 시스템 구현**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    6. [**데이터 관리**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    7. [**단위 테스트 환경 구축**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    8. [**기획을 고려한 유닛 공격, 스킬 데이터 캡슐화**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    9. [**아군, 적 유닛 데이터 공유**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    10. [**파싱 간편화**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    11. [**ComponentProvider 구현**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    12. [**슬롯과 유닛 배치 시스템**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    13. [**스와이프 (UX)**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    14. [**실행 환경 동적 레이아웃 구현 (UX)**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    15. [**DoTween을 이용한 UI**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
- **문제 및 해결**
    1. [**Addressable 순차적 로드 시 시간 문제**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    2. [**CBT 단계 : 기획 딜레이로 인한 개발 지연**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)
    3. [**재활용한 애니메이션의 이벤트 맵핑 문제**](https://www.notion.so/29209522f1c6804a9e61c121edaa681b?pvs=21)

---

# 구현 목록

# 1. 스킬 구현

---

### 구현

- 추상 클래스인 `Skill`을 상속받아 다양한 스킬을 파생
- 이펙트는 EffectAddress로 ResourcesManager에서 Load하여 사용
- `Active()`는 각 클래스에서 독립적으로 구현
    - **SkillData 코드**
        
        ```csharp
        public abstract class UnitSkill : ScriptableObject
        {
            [Header("ID")]
            public int ID;
        
            [Header("Default")]
            public Sprite Icon;
            public string SkillName;
            [TextArea] public string Description;
        
            [Header("Stat")]
            public int MaxCount = 1;
            public float PhysicalPower = 1;
            public float AbilityPower = 1;
            public int ManaCost;
        
            [Header("Type")]
            public Priority Priority;
        
            [Header("Effect")]
            public string EffectAddress;
            public EffectSpawnType EffectSpawnType;
            public Vector2 SpawnPointOffset;    
        
            public virtual void Active(IAttacker attacker)
            {
                
            }
        }
        ```
        
- 필터
    - Enum값인 Priority를 통해 적을 필터링 가능하도록 설정
    - `GetPriorityFilter()` 메서드에서 `Priority` 값(Close, Far, LowHp, Tank 등)에 따라 `System.Func` **형태의 필터 함수**를 반환하여, 유틸리티 함수에 이 필터를 전달
    - **Priority판단**
        
        ```csharp
        protected virtual GameObject GetTargetSingle(IAttacker attacker)
        {
        		// 유틸 클래스에 구성된 Filter로 거른 타켓 찾기 기능
            var target = Utils.GetTargetsNonAllocSingle(attacker, SearchType.Circle, 100, Vector2.zero, 1, attacker.TargetLayer, GetPriorityFilter());
            return target;
        }
        
        protected System.Func<IAttacker, List<GameObject>, GameObject> GetPriorityFilter()
        {
            switch (Priority)
            {
                case Priority.Close:
                    return Close;
                case Priority.Far:
                    return Far;
                case Priority.LowHp:
                    return LowHp;
                case Priority.HightHp:
                    return HighHp;
                case Priority.Tank:
                    return ClassFilter(ClassType.TANK);
                case Priority.Melee:
                    return ClassFilter(ClassType.MELEE);
                case Priority.Ranged:
                    return ClassFilter(ClassType.RANGED);
                case Priority.Support:
                    return ClassFilter(ClassType.SUPPORT);
                default:
                    return null;
            }
        }
        ```
        
- 스킬 종류
    - **체인 공격 스킬**
        - `ChainLineAttacker`라는 별도의 객체를 생성하고, `UniTask`를 사용하여 일정 간격(`_interval`)마다 타겟을 변경하고 공격하는 로직을 구현
        - `CancellationTokenSource`를 활용하여 컴포넌트 비활성화(`OnDisable`) 시 실행 중인 비동기 작업을 안전하게 종료(`Dispose()`)하고 메모리 누수를 방지
            
            ![ChainLineSkill](https://github.com/user-attachments/assets/bbbac512-9ff1-4af9-9166-be2cd4f4b731)

            <가장 먼 적을 기준>
            
        - Chain 스킬 코드
            
            ```csharp
            [CreateAssetMenu(fileName = "LineSkill", menuName = "Data/Unit/Skill/LineSkill")]
            public class PierceLineSkill : AttackSkill
            {
                [Header("Line_Skill")]
                private const string CHAIN_LINE_ATTACKER = "ChainLineAttacker";
                [SerializeField] int _count;
                [SerializeField] float _interval;
                [SerializeField] int _ratio = 10;
                [SerializeField] float _attackThickness = 1;
            
                public override void Active(IAttacker attacker)
                {
                    base.Active(attacker);
            
                    Transform target = Priority == Priority.Target ? attacker.GetTarget() : GetTargetSingle(attacker)?.transform;
            
                    if (target == null)
                        return;
            
                    GameObject effect = Manager.Resources.Load<GameObject>(EffectAddress);
                    ChainLineAttacker chainLineAttacker = Manager.Resources.Instantiate<GameObject>(CHAIN_LINE_ATTACKER, attacker.GetCenter())
                        .GetComponent<ChainLineAttacker>();
            
                    chainLineAttacker.Setup(
                        attacker, effect, target, _count, _interval, attacker.TargetLayer,
                        PhysicalPower, AbilityPower, DamageType, _attackThickness, _ratio
                        );
                }
            
                protected override GameObject GetTargetSingle(IAttacker attacker)
                {
                    return base.GetTargetSingle(attacker);
                }
            
            #if UNITY_EDITOR
                public override void DrawGizmos(IAttacker attacker)
                {
                    Gizmos.color = Color.magenta;
                    Vector2 size = new Vector2(2, _attackThickness);
                    Gizmos.DrawWireCube(attacker.GetCenter(), size);        
                }
            #endif
            }
            ```
            
        - ChainAttacker코드
            
            ```csharp
            public class ChainLineAttacker : MonoBehaviour
            {
            		// 필드 변수 생략
            		
                private void OnDisable()
                {
                    Dispose();
                }
            
                private void Dispose()
                {
                    if (_source != null)
                    {
                        _source.Cancel();
                        _source.Dispose();
                        _source = null;
                    }
                }
            
                public void Setup(IAttacker attacker, GameObject effect, Transform target, int count, float interval, 
                    LayerMask targetLayer, float physicalPower, float abilityPower, DamageType damageType, float attackThickness, int ratio)
                {
                    _count = count;
                    _target = target;
                    _effect = effect;
                    _interval = interval;
                    _damageType = damageType;
                    _physicalPower = physicalPower;
                    _abilityPower = abilityPower;
                    _targetLayer = targetLayer;
                    _attacker = attacker;
                    _ratio = ratio;
                    _attackThickness = attackThickness;
            
                    _targetList.Clear();
                    _currentPos = transform.position;
            
                    if (_source == null)
                        _source = new();
            
                    ChainLineAttack().Forget();
                }
            
                private async UniTask ChainLineAttack()
                {
                    for (int i = 0; i < _count; i++)
                    {
                        if (_target == null)
                        {
                            Manager.Resources.Destroy(gameObject);
                            Dispose();
                            return;
                        }
            
                        SpawnEffect();
                        ChangeTarget();
            
                        await UniTask.WaitForSeconds(_interval, cancellationToken: _source.Token);
                    }
            
                    Manager.Resources.Destroy(gameObject);
                }
                
                private void SpawnEffect()
                {
                    Vector2 spawnPos = GetSpawnPosition(_target.position);
            
                    GameObject effect = Manager.Resources.Instantiate(_effect, spawnPos, true);
            
                    if(effect == null)
                    {
                        Debug.LogWarning("[ChainLineAttacker] 이펙트 프리팹을 찾을 수 없습니다.");
                        return;
                    }
                    if (_target == null)
                    {
                        Debug.LogWarning("[ChainLineAttacker] 타겟이 없습니다.");
                        Manager.Resources.Destroy(effect);
                        return;
                    }
            
                    effect.transform.right = (_target.position - effect.transform.position).normalized;
            
                    Vector3 scale = effect.transform.localScale;
                    float scaleX = Vector2.Distance(_target.position, _currentPos);
                    scale.x = scaleX / _ratio;
                    effect.transform.localScale = scale;
            
                    float boxWidth = scaleX;
                    float boxHeight = _attackThickness;
                    float angle = effect.transform.eulerAngles.z;
            
                    AttackBox(spawnPos, new Vector2(boxWidth, boxHeight), angle);
            
                    Manager.Resources.Destroy(effect, 2);
                }
            
                private void ChangeTarget()
                {
                    _targetList.Add(_target);
                    _currentPos = ComponentProvider.Get<UnitBase>(_target.gameObject).GetCenter();
                    _target = Utils.GetClosestTargetNonAlloc(_currentPos, 100, _targetLayer, Filter);        
                }
            
                private void AttackBox(Vector2 center, Vector2 size, float angleDegrees)
                {
                    int layerMask = _targetLayer;
            
                    int hitCount = Physics2D.OverlapBoxNonAlloc(center, size, angleDegrees, _overlapResults, layerMask);
            
                    for (int i = 0; i < hitCount; i++)
                    {
                        var col = _overlapResults[i];
                        if (col == null) continue;
            
                        _attacker.GetStatusController().CalculateDamage(_physicalPower, _abilityPower, _damageType, ComponentProvider.Get<UnitBase>(col.gameObject).StatusController);
                    }
            
                    for (int i = hitCount; i < _overlapResults.Length; i++)
                        _overlapResults[i] = null;
                }
            
                private Vector2 GetSpawnPosition(Vector2 targetPosition)
                {
                    return (_currentPos + targetPosition) / 2;
                }
            
                private bool Filter(Transform target)
                {
                    return !ComponentProvider.Get<UnitBase>(target.gameObject).StatusController.IsDead && _targetList.Contains(target);
                }
                
            #if UNITY_EDITOR
                private void OnDrawGizmosSelected()
                {
                    if (_target == null) return;
            
                    Vector2 spawn = GetSpawnPosition(_target.position);
                    float width = Vector2.Distance(_target.position, _currentPos);
                    Vector2 size = new Vector2(width, _attackThickness);
            
                    Vector2 dir = (_target.position - (Vector3)_currentPos).normalized;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
                    Gizmos.color = Color.red;
                    Gizmos.matrix = Matrix4x4.TRS(spawn, Quaternion.Euler(0, 0, angle), Vector3.one);
                    Gizmos.DrawWireCube(Vector3.zero, size);
                }
            #endif
            }
            
            ```
            
    - **곡선 멀티 투사체 스킬**
        - `_count` 수 만큼 반복하여 `_interval`마다 곡선 형태로 이동하는 투사체를 생성
            
            ![SkillUseEffect](https://github.com/user-attachments/assets/b467f20c-2197-49d6-9943-ccf35fec1eb0)

        - 곡선 멀티 투사체 Skill코드
            
            ```csharp
            [CreateAssetMenu(fileName = "MultiRangedSkill", menuName = "Data/Unit/Skill/MultiRanged")]
            public class MultiRangedSkill : RangedSkill
            {
                [SerializeField] float _interval;
                [SerializeField] float _count;
                [SerializeField] string _address;
            
                public override void Active(IAttacker attacker)
                {
                    SpawnEffect(attacker);
            
                    MultiRangedAttack(attacker).Forget();
                }
            
                private async UniTask MultiRangedAttack(IAttacker attacker)
                {
                    for (int i = 0; i < _count; i++)
                    {
                        await UniTask.WaitForSeconds(_interval);
                        GameObject obj = Manager.Resources.Instantiate<GameObject>(_address, GetSpawnPoint(attacker), true);
                        Projectile projectile = ComponentProvider.Get<Projectile>(obj);
                        projectile.Init(attacker.GetTarget(), attacker.GetStatusController(), PhysicalPower, AbilityPower, DamageType, attacker.TargetLayer, _projectileSpeed);
                    }
                }
            }
            ```
            
    - **공격, 회복, 버프/디버프를 통합적으로 처리할 수 있는 투사체 스킬**
        - `_isBuff` 플래그에 따라 `BuffEffectData` 또는 `StatEffectModifier`를 투사체에 전달하여, 하나의 스킬 클래스로 다양한 효과를 생성할 수 있도록 **결합도**를 낮춤.
        - 기본 타겟이 없을 경우, 주변에서 유효한 대상을 검색하는 폴백 로직을 적용하여 스킬 발동의 안정성을 높임
        - 터질때 이펙트가 있다면 주소를 설정하여 스폰
      
        ![HealSkill](https://github.com/user-attachments/assets/0cedd3fa-ada7-48cc-80d0-9a8b8ad2f19f)
        ![ExplosionProjcetile](https://github.com/user-attachments/assets/1a1fdfb8-82b3-4dae-a70a-6bc2311c2204)
        
        - BuffRangedSkill 코드
            
            ```csharp
            [CreateAssetMenu(fileName = "BuffRangedSkill", menuName = "Data/Unit/Skill/BuffThrow")]
            public class BuffRangedSkill : RangedSkill
            {
                [Header("Type")]
                [SerializeField] ActivationCondition _activationCondition;
                [SerializeField] ThrowType _throwType;    
                [SerializeField] float _parabolaYOffset;
                [SerializeField] bool _isAttack;
            
                [Header("Buff")]
                [SerializeField] bool _isModifier;
                [SerializeField] bool _isBuff;
                [SerializeField] bool _isAlly;
                [SerializeField] float _radius;
                [SerializeField] BuffEffectData _buffEffectData;
                [SerializeField] StatEffectModifier _statModifier;
                
                public override void Active(IAttacker attacker)
                {
                    GameObject spawnObject = Manager.Resources.Load<GameObject>(EffectAddress);
            
                    if (spawnObject == null)
                    {
                        Debug.Log("SpawnObject is null");
                        return;
                    }
            
                    if (attacker.GetTarget() == null)
                        return;
                    GameObject obj = Manager.Resources.Instantiate<GameObject>(
                            spawnObject,
                            GetSpawnPoint(attacker),
                            true
                            );
            
                    if(obj == null)
                    {
                        Debug.Log("Object가 없습니다");
                        return;
                    }
            
                    SplashBuffProjectile projectile = ComponentProvider.Get<SplashBuffProjectile>(
                        obj
                        );
            
                    Transform target = GetTarget(attacker);
            
                    if (target == null || projectile == null)
                        return;
            
                    projectile.transform.right = (target.position - projectile.transform.position).normalized;
                    projectile.transform.Rotate(0, 0, spawnObject.transform.rotation.eulerAngles.z);
            
                    if (target == null)
                    {
                        target = Utils.GetClosestTargetNonAlloc(GetSpawnPoint(attacker), 10f, GetLayerMask(attacker));
                    }
            
                    if (_isBuff)
                    {
                        projectile.Init(_buffEffectData, _radius, _throwType, _activationCondition, _isAttack, _isModifier,
                        target, attacker.GetStatusController(), PhysicalPower, AbilityPower, DamageType, GetLayerMask(attacker), _projectileSpeed, GetExplosionEffect(),
                        _parabolaYOffset);
                    }
                    else if (!_isBuff)
                    {
                        projectile.Init(_statModifier, _radius, _throwType, _activationCondition, _isAttack, _isModifier,
                        target, attacker.GetStatusController(), PhysicalPower, AbilityPower, DamageType, GetLayerMask(attacker), _projectileSpeed, GetExplosionEffect(),
                        _parabolaYOffset);
                    }
                }
            
                protected override GameObject GetTargetSingle(IAttacker attacker)
                {
                    var target = Utils.GetTargetsNonAllocSingle(attacker, SearchType.Circle, 100, Vector2.zero, 1, GetLayerMask(attacker), GetPriorityFilter());
                    return target;
                }
            
                private LayerMask GetLayerMask(IAttacker attacker)
                {
                    if (_isAlly)
                    {
                        return attacker.GetAllyLayerMask();
                    }
                    else
                        return attacker.TargetLayer;
                }
            
                protected Transform GetTarget(IAttacker attacker)
                {
                    if (Priority == Priority.None)
                        return null;
            
                    if (Priority == Priority.Target)
                    {
                        if (attacker.GetTarget() == null)
                        {
                            Debug.Log("[스킬] 타켓이 없습니다.");
                            
                            return Utils.GetClosestTargetNonAlloc(attacker.GetCenter(), 10, attacker.TargetLayer)?.transform;
                        }
            
                        return attacker.GetTarget();
                    }
                    else
                    {
                        return GetTargetSingle(attacker)?.transform;
                    }
                }
            
                public override string GetCalculateValueString(UnitStatus status)
                {
                    UnitStats stat = status.GetCurrentStat();
                    float value = stat.MagicDamage * (AbilityPower / 100);
            
                    if(_isBuff)
                    {
                        if (_buffEffectData.StatType == StatType.CurHp || _buffEffectData.StatType == StatType.Shield)
                            return Mathf.RoundToInt(value).ToString();
                        else
                            return value.ToString("F1");
                    }        
                    else
                    {
                        if (_statModifier.StatType == StatType.CurHp || _statModifier.StatType == StatType.Shield)
                            return Mathf.RoundToInt(value).ToString();
                        else
                            return value.ToString("F1");
                    }
                }
            
            #if UNITY_EDITOR
                public override void DrawGizmos(IAttacker attacker)
                {
                    base.DrawGizmos(attacker);
            
                    Gizmos.color = Color.yellow;
            
                    Gizmos.DrawWireSphere(attacker.GetCenter(), _radius);
                }
            #endif
            }
            ```
            

### 구현 이유

- 조건이 달라져도 Prioty 변경으로 간단히 변경가능
- 다형성으로 구성하여 확장성을 높이고, 유지 보수성을 높임

### 결과

- 새로운 공격 방식(예: 도트 데미지, 광역 디버프)이 필요할 때, 기존 `UnitSkill`을 상속받는 **새로운 클래스 파일 하나**만 추가하면 되도록 설계하여 개발 시간을 단축
- 타겟팅 조건, 스킬 계수 등 기획 변경 사항이 발생했을 때, 데이터(ScriptableObject) 수정이나 **필터**  변경만으로 유연하게 대처
- `UniTask`와 명시적인 자원 해제를 통해 복잡한 시퀀스 공격 로직에서 발생할 수 있는 잠재적인 런타임 오류 및 메모리 이슈를 사전에 방지

---

# 2. 협업을 위한 커스텀 에디터 구성

---

### 구현

- **유닛 선택**
    ![GridEditorTool](https://github.com/user-attachments/assets/d08ba731-22b7-4cf6-8c3c-4b45dc125e17)
    
    - UI 생성 후 `Unit`데이터 들을 `List`에 담고 List UI로 생성
    - `List` 요소 클릭 시 해당 유닛 데이터를 세팅
    - 일반, 엘리트, 보스 몬스터인지 세팅
    - 현재 세팅된 유닛 미리보기
- **그리드 그리기**
    - 해당 `Unit`이 세팅될 그리드를 그림
    - 그리드 클릭 시 현재 세팅중인 유닛과 그리드 좌표 값으로 데이터를 생성
- **그리드 데이터 수정**
    - 경로에 있는 Grid데이터를 모두 가져옴.
    - 리스트화 후 수정버튼 추가
    - 수정 버튼 클릭 시 해당 데이터를 기반으로 `CustomEditor`상에 데이터를 세팅

### 구현 이유

- `Grid` **좌표 방식**의 **데이터**를 직관적으로 세팅하기 힘들었음
- 보다 직관적인 데이터 생성하고 기획과의 협업을 위하여 커스텀 에디터를 구성
<img width="575" height="453" alt="image (1)" src="https://github.com/user-attachments/assets/f2de81d3-ae1e-4087-9f8d-e404fa922aec" />
### 결과

- 보다 직관적인 데이터 세팅을 통해 휴먼에러 비율을 극도로 낮춤.
- 툴을 제작하여 프로그래밍 없이도 기획자의 의도대로 데이터를 구성하고 추가할 수 있도록 하여 기획자가 모든 스테이지의 데이터를 약 2시간만에 생성하여 업무 효율을 개선.

<img width="456" height="528" alt="image (2)" src="https://github.com/user-attachments/assets/66c15ec6-64a6-49e9-8d42-d0652bc1b256" />

### 관련 코드

- **DrawGrid                                         (그리드 그리기)**
    
    ```csharp
    private void DrawGrid()
    {
        EditorGUILayout.LabelField("그리드 크기 (3x4)", EditorStyles.boldLabel);
    
        if (selectedUnitData != null)
        {
            string rankText = selectedLevel == 0 ? "일반" : selectedLevel == 1 ? "엘리트" : "보스";
            Color rankColor = selectedLevel == 0 ? normalColor : selectedLevel == 1 ? eliteColor : bossColor;
    
            Color originalBg = GUI.backgroundColor;
            GUI.backgroundColor = rankColor;
            EditorGUILayout.LabelField($"배치할 유닛: {selectedUnitData.Name} [{rankText}]", EditorStyles.helpBox);
            GUI.backgroundColor = originalBg;
        }
        else
        {
            EditorGUILayout.LabelField("배치할 유닛을 먼저 선택하세요", EditorStyles.helpBox);
        }
    
        EditorGUILayout.LabelField("좌클릭: UnitStatus 배치 | 우클릭: UnitStatus 삭제");
    
        EditorGUILayout.BeginVertical(GUI.skin.box);
    
        for (int y = GRID_HEIGHT; y > 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
    
            for (int x = GRID_WIDTH; x > 0; x--)
            {
                Vector2Int position = new Vector2Int(x - 1, y - 1);
                UnitStatus unitStatus = currentGridData.GetUnitStatus(position);
    
                Rect slotRect = GUILayoutUtility.GetRect(SLOT_SIZE, SLOT_SIZE);
    
                Color originalColor = GUI.backgroundColor;
                if (unitStatus != null)
                {
                    if (unitStatus.Level == 0)
                        GUI.backgroundColor = normalColor;
                    else if (unitStatus.Level == 1)
                        GUI.backgroundColor = eliteColor;
                    else if (unitStatus.Level == 2)
                        GUI.backgroundColor = bossColor;
                }
    
                GUI.Box(slotRect, "", GUI.skin.button);
                GUI.backgroundColor = originalColor;
    
                if (unitStatus != null && unitStatus.Data != null && unitStatus.Data.Icon != null)
                {
                    Rect iconRect = new Rect(slotRect.x + 2, slotRect.y + 2, slotRect.width - 4, slotRect.height - 20);
                    GUI.DrawTexture(iconRect, unitStatus.Data.Icon.texture, ScaleMode.ScaleToFit);
    
                    Rect textRect = new Rect(slotRect.x, slotRect.y + slotRect.height - 18, slotRect.width, 18);
                    GUIStyle centeredStyle = new GUIStyle(EditorStyles.miniLabel);
                    centeredStyle.alignment = TextAnchor.MiddleCenter;
                    centeredStyle.fontSize = 8;
    
                    string rankText = unitStatus.Level == 0 ? "일반" : unitStatus.Level == 1 ? "엘리트" : "보스";
                    GUI.Label(textRect, $"{unitStatus.Data.Name}\n{rankText}", centeredStyle);
                }
                else
                {
                    Rect posRect = new Rect(slotRect.x, slotRect.y + slotRect.height - 15, slotRect.width, 15);
                    GUIStyle centeredStyle = new GUIStyle(EditorStyles.miniLabel);
                    centeredStyle.alignment = TextAnchor.MiddleCenter;
                    centeredStyle.fontSize = 8;
                    GUI.Label(posRect, $"({x},{y})", centeredStyle);
                }
    
                Event currentEvent = Event.current;
                if (slotRect.Contains(currentEvent.mousePosition))
                {
                    if (currentEvent.type == EventType.MouseDown)
                    {
                        if (currentEvent.button == 0)
                        {
                            if (selectedUnitData != null)
                            {
                                UnitStatus newUnitStatus = new UnitStatus(selectedUnitData, selectedLevel);
                                currentGridData.SetUnitStatus(position, newUnitStatus);
                                EditorUtility.SetDirty(currentGridData);
                                string rankText = selectedLevel == 0 ? "일반" : selectedLevel == 1 ? "엘리트" : "보스";
                                Debug.Log($"배치됨: {selectedUnitData.Name} [{rankText}] at ({x},{y})");
                                Repaint();
                            }
                            else
                            {
                                Debug.LogWarning("배치할 유닛을 먼저 선택하세요!");
                            }
                        }
                        else if (currentEvent.button == 1)
                        {
                            if (unitStatus != null)
                            {
                                string rankText = unitStatus.Level == 0 ? "일반" : unitStatus.Level == 1 ? "엘리트" : "보스";
                                Debug.Log($"삭제됨: {unitStatus.Data.Name} [{rankText}] from ({x},{y})");
                            }
                            currentGridData.RemoveUnitData(position);
                            EditorUtility.SetDirty(currentGridData);
                            Repaint();
                        }
                        currentEvent.Use();
                    }
                }
            }
    
            EditorGUILayout.EndHorizontal();
        }
    
        EditorGUILayout.EndVertical();
    
        int totalUnits = currentGridData.unitDatas.Count;
        EditorGUILayout.LabelField($"배치된 유닛 수: {totalUnits} / {GRID_WIDTH * GRID_HEIGHT}", EditorStyles.helpBox);
    }
    ```
    
- **LoadGridDataForEdit                   (그리드 데이터 로드)**
    
    ```csharp
    private void LoadGridDataForEdit(UnitGridDataSO gridData)
    {
        currentGridData.ClearAllUnitDatas();
    
        foreach (var gridUnitData in gridData.unitDatas)
        {
            UnitStatus loadedStatus = new UnitStatus(gridUnitData.unitStatus.Data, gridUnitData.unitStatus.Level);
            currentGridData.SetUnitStatus(gridUnitData.position, loadedStatus);
        }
    
        if (currentGridData != null)
            EditorUtility.SetDirty(currentGridData);
    
        isEditMode = true;
        selectedGridDataForEdit = gridData;
        newGridName = gridData.gridName;
    
        Repaint();
    
        Debug.Log($"수정 모드로 그리드 구성을 불러왔습니다: {gridData.gridName} (유닛 {gridData.unitDatas.Count}개)");
    }
    ```
    
- **DrawUnitDataSelector                (유닛 데이터 선택)**
    
    ```csharp
    private void DrawUnitDataSelector()
    {
        EditorGUILayout.LabelField("유닛 데이터 설정", EditorStyles.boldLabel);
    
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("UnitData 새로고침", GUILayout.Width(150)))
        {
            RefreshAvailableUnitDatas();
        }
        showUnitDataList = EditorGUILayout.Toggle("유닛 데이터 리스트 보기", showUnitDataList);
        EditorGUILayout.EndHorizontal();
    
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("수동 선택:", GUILayout.Width(100));
        UnitData newUnitData = (UnitData)EditorGUILayout.ObjectField(selectedUnitData, typeof(UnitData), false);
        if (newUnitData != selectedUnitData)
        {
            selectedUnitData = newUnitData;
        }
        EditorGUILayout.EndHorizontal();
    
        if (selectedUnitData != null)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField($"'{selectedUnitData.Name}' 등급 선택", EditorStyles.boldLabel);
    
            EditorGUILayout.BeginHorizontal();
    
            Color originalBg = GUI.backgroundColor;
    
            GUIStyle centeredButtonStyle = new GUIStyle(GUI.skin.button);
            centeredButtonStyle.alignment = TextAnchor.MiddleCenter;
    
            GUI.backgroundColor = normalColor;
            if (GUILayout.Button("일반", selectedLevel == (int)UnitRank.Normal ? centeredButtonStyle : GUI.skin.button, GUILayout.Height(35)))
            {
                selectedLevel = (int)UnitRank.Normal;
                Debug.Log($"등급 선택: {selectedUnitData.Name} - 일반");
            }
    
            GUI.backgroundColor = eliteColor;
            if (GUILayout.Button("엘리트", selectedLevel == (int)UnitRank.Elite ? centeredButtonStyle : GUI.skin.button, GUILayout.Height(35)))
            {
                selectedLevel = (int)UnitRank.Elite;
                Debug.Log($"등급 선택: {selectedUnitData.Name} - 엘리트");
            }
    
            GUI.backgroundColor = bossColor;
            if (GUILayout.Button("보스", selectedLevel == (int)UnitRank.Boss ? centeredButtonStyle : GUI.skin.button, GUILayout.Height(35)))
            {
                selectedLevel = (int)UnitRank.Boss;
                Debug.Log($"등급 선택: {selectedUnitData.Name} - 보스");
            }
    
            GUI.backgroundColor = originalBg;
    
            EditorGUILayout.EndHorizontal();
    
            string rankText = selectedLevel == 0 ? "일반" : selectedLevel == 1 ? "엘리트" : "보스";
            EditorGUILayout.LabelField($"현재 선택: {rankText}", EditorStyles.centeredGreyMiniLabel);
    
            EditorGUILayout.EndVertical();
        }
    
        if (showUnitDataList)
        {
            DrawUnitDataList();
        }
    
        if (selectedUnitData != null)
        {
            DrawSelectedUnitDataPreview();
        }
    }
    ```
    
- **RefreshAvailableUnitDatas         (유닛 데이터 확인)**
    
    ```csharp
    private void RefreshAvailableUnitDatas()
    {
        string[] unitDataGuids = AssetDatabase.FindAssets("t:UnitData", new[] { UNIT_DATA_SEARCH_PATH });
        System.Collections.Generic.List<UnitData> unitDataList = new System.Collections.Generic.List<UnitData>();
    
        foreach (string guid in unitDataGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            UnitData unitData = AssetDatabase.LoadAssetAtPath<UnitData>(path);
    
            if (unitData != null)
            {
                unitDataList.Add(unitData);
            }
        }
    
        availableUnitDatas = unitDataList.ToArray();
    
        System.Array.Sort(availableUnitDatas, (a, b) => a.Name.CompareTo(b.Name));
    }
    ```
    
- **DrawUnitDataList                           (유닛 데이터 선택창 그리기)**
    
    ```csharp
    private void DrawUnitDataList()
    {
        EditorGUILayout.LabelField("UnitData 목록:", EditorStyles.miniLabel);
    
        if (availableUnitDatas == null || availableUnitDatas.Length == 0)
        {
            EditorGUILayout.HelpBox("UnitData ScriptableObject를 찾을 수 없습니다. UnitData SO가 프로젝트에 있는지 확인해주세요.", MessageType.Info);
            return;
        }
    
        unitDataListScrollPosition = EditorGUILayout.BeginScrollView(unitDataListScrollPosition, GUILayout.Height(120));
    
        for (int i = 0; i < availableUnitDatas.Length; i++)
        {
            if (availableUnitDatas[i] == null) continue;
    
            UnitData unitData = availableUnitDatas[i];
    
            bool isSelected = selectedUnitData == unitData;
            Color originalColor = GUI.backgroundColor;
            if (isSelected)
            {
                GUI.backgroundColor = new Color(0.5f, 0.8f, 1f);
            }
    
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
    
            if (unitData.Icon != null)
            {
                Texture2D iconTexture = AssetPreview.GetAssetPreview(unitData.Icon);
                if (iconTexture != null)
                {
                    GUILayout.Label(iconTexture, GUILayout.Width(24), GUILayout.Height(24));
                }
                else
                {
                    GUILayout.Space(28);
                }
            }
            else
            {
                GUILayout.Space(28);
            }
    
            if (GUILayout.Button($"{unitData.Name}", EditorStyles.label))
            {
                selectedUnitData = unitData;
                selectedLevel = 0;
            }
    
            EditorGUILayout.EndHorizontal();
    
            GUI.backgroundColor = originalColor;
        }
    
        EditorGUILayout.EndScrollView();
    }
    ```
    
- **DrawSelectedUnitDataPreview (선택한 유닛 데이터 미리보기)**
    
    ```csharp
    private void DrawSelectedUnitDataPreview()
    {
        if (selectedUnitData != null)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("선택된 유닛 데이터 미리보기:", EditorStyles.miniLabel);
    
            EditorGUILayout.BeginHorizontal();
    
            if (selectedUnitData.Icon != null)
            {
                Texture2D iconTexture = AssetPreview.GetAssetPreview(selectedUnitData.Icon);
                if (iconTexture != null)
                {
                    GUILayout.Label(iconTexture, GUILayout.Width(48), GUILayout.Height(48));
                }
            }
    
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("이름:", selectedUnitData.Name);
    
            string rankText = selectedLevel == 0 ? "일반" : selectedLevel == 1 ? "엘리트" : "보스";
            EditorGUILayout.LabelField("등급:", rankText);
    
            if (selectedUnitData.UnitStats != null && selectedLevel < selectedUnitData.UnitStats.Length && selectedUnitData.UnitStats[selectedLevel] != null)
            {
                var stats = selectedUnitData.UnitStats[selectedLevel];
                EditorGUILayout.LabelField("체력:", stats.MaxHealth.ToString());
                EditorGUILayout.LabelField("물리데미지:", stats.PhysicalDamage.ToString());
                EditorGUILayout.LabelField("마법데미지:", stats.MagicDamage.ToString());
                EditorGUILayout.LabelField("공격속도:", stats.AttackSpeed.ToString("F2"));
                EditorGUILayout.LabelField("이동속도:", stats.MoveSpeed.ToString("F2"));
            }
            else
            {
                EditorGUILayout.LabelField("스탯:", "없음");
            }
            EditorGUILayout.EndVertical();
    
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
    ```
    

---

# 3. Addressable 비동기 로드 시스템

---

### 구현 방법

- `ResourcesManager`를 API구조로 구성하고 전역적인 접근을 허용하여 팀원들이 쉽게 사용할 수 있도록 함.
- 모든 리소스 로드 작업을 `UniTask` 기반의 **비동기 처리(async/await)** 로 전환하여, 메인 스레드 블로킹 없이 UI 및 로직이 자연스럽게 동작하도록 구현.
- `UniTask.WhenAll`을 이용해 **병렬 로드 구조**를 적용함으로써, 다수의 리소스 로드를 동시에 처리.
- 로드된 리소스는 **메모리 풀에 캐싱**하여 중복 로드 방지 및 로드 대기 시간을 단축.

### 구현 이유

- **API구성의 쉬운 사용**
    - 어드레서블의 내부 기능을 모른다고 하더라도 쉽게 사용할 수 있도록 하기 위해서 구성함.
- **호환성 및 유지보수**
    - `ResourcesManager`라는 class에서 생성, 삭제, 로드 등을 관리하여
    - 리소스 관리 툴을 변경해야 하는 상황에도 `ResourcesManager`만을 수정하여 코드 영향 최소화
- **GC 할당 저하**
    - `Coroutine(IEnumerator)`대신 `UniTask(struct)`비동기로 GC Alloc 저하
- **Load시간 최적화**
    - `UniTask.WhenAll` 병렬로드로 `Load`시간 단축, 대기 없이 다음 로직이 이어지도록 개선

### 결과

- 모든 팀원들이 기술에 대한 R&D 없이도 통일된 룰을 설정하여 **작업환경을 개선**함.
- 구조가 바뀌더라도 접근점이 하나로 되어 있어 **유지보수성을 높**일 수 있었음.

### 관련  코드

- **ResourcesManager**
    
    ```csharp
    public class ResourcesManager : Singleton<ResourcesManager>
    {
        private static Dictionary<string, Object> _resources = new Dictionary<string, Object>();    
        private static Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
    
    	  #region Load
    	  public async UniTask<T> LoadRefAsync<T>(AssetReference reference) where T : Object
        {
            string primaryKey = await GetPrimaryKey(reference);
            
            if (!_resources.ContainsKey(primaryKey))
                return null;
    
            return _resources[primaryKey] as T;
        }
    
        public T Load<T>(string address) where T : Object
        {
            if(string.IsNullOrEmpty(address)) return null;
    
            if (!_resources.ContainsKey(address))
            {
                Debug.LogWarning($"[AddressableSystem] {address} 주소의 에셋이 로드되지 않았습니다.");
                return null;
            }
    
            return _resources[address] as T;
        }
    
        public async UniTask<T> LoadAsync<T>(string address) where T : Object
        {
            if (string.IsNullOrEmpty(address)) return null;
    
            if (!_resources.ContainsKey(address))
            {
                var handle = Addressables.LoadAssetAsync<T>(address);
                var asset = await handle.Task;
    
                _resources.Add(address, asset);
            }
    
            return _resources[address] as T;
        }
        #endregion
    
        #region Sprite
        public Sprite SpriteLoad(string address)
        {
            if(!_sprites.ContainsKey(address))
            {
                Debug.Log($"[스프라이트] {address}의 Sprite가 없습니다.");
                return null;
            }
    
            return _sprites[address];
        }
    
        public async UniTask SpriteLoadLable(string label)
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            var locations = await locationsHandle.Task;
    
            List<UniTask> tasks = new List<UniTask>(100);
    
            foreach (var location in locations)
            {
                tasks.Add(SpriteLoadAndCache(location));
            }
    
            await UniTask.WhenAll(tasks);
    
            Addressables.Release(locationsHandle);
        }
    
        private async UniTask SpriteLoadAndCache(IResourceLocation location)
        {
            var handle = Addressables.LoadAssetAsync<Sprite>(location.PrimaryKey);
            var asset = await handle.Task;
    
            if (!_sprites.ContainsKey(location.PrimaryKey))
            {
                _sprites.Add(location.PrimaryKey, asset);
            }
        }
        #endregion
    
        #region Load&Unload Label
        public UniTask LoadAllLabel(AssetLabelReference[] labels)
        {
            List<UniTask> tasks = new List<UniTask>();
            foreach (var label in labels)
            {
                tasks.Add(LoadLabel(label));
            }
    
            return UniTask.WhenAll(tasks);
        }
        public UniTask UnloadAllLabel(AssetLabelReference[] labels)
        {
            List<UniTask> tasks = new List<UniTask>();
            foreach (var label in labels)
            {
                tasks.Add(UnloadLabel(label));
            }
            return UniTask.WhenAll(tasks);
        }
        #endregion
    
        #region Load
        public async UniTask LoadLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) return;
    
            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            var locations = await locationsHandle.Task;
    
            List<UniTask> tasks = new List<UniTask>(100);
    
            foreach (var location in locations)
            {
                tasks.Add(LoadAndCache(location));
            }
    
            await UniTask.WhenAll(tasks);
    
            Addressables.Release(locationsHandle);
            Debug.Log($"[로드 성공] : {label}");
        }
    
        public async UniTask LoadLabel(AssetLabelReference label)
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            var locations = await locationsHandle.Task;
    
            List<UniTask> tasks = new List<UniTask>(100);
    
            foreach (var location in locations)
            {
                tasks.Add(LoadAndCache(location));
            }
    
            await UniTask.WhenAll(tasks);
    
            Addressables.Release(locationsHandle);
        }
    
        private async UniTask LoadAndCache(IResourceLocation location)
        {
            var handle = Addressables.LoadAssetAsync<Object>(location);
            var asset = await handle.Task;
    
            if (!_resources.ContainsKey(location.PrimaryKey))
                _resources.Add(location.PrimaryKey, asset);
            else
                _resources[location.PrimaryKey] = asset;
        }
    
        public async UniTask<T[]> LoadAll<T>(string label) where T : Object
        {
            if (string.IsNullOrEmpty(label)) return null;
    
            var handle = Addressables.LoadAssetsAsync<T>(label, null);
            var result = await handle.Task;
    
            return result.ToArray();
        }
    
        public async UniTask<T> LoadAsync<T>(AssetReference reference) where T : Object
        {
            string primaryKey = await GetPrimaryKey(reference);
    
            if (!_resources.ContainsKey(primaryKey))
            {
                var handle = reference.LoadAssetAsync<T>();
                var asset = await handle.Task;
                _resources.Add(primaryKey, asset);
            }
    
            return _resources[primaryKey] as T;
        }
        #endregion
        
        #region Unload
        public async UniTask UnloadLabel(string label)
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            var locations = await locationsHandle.Task;
    
            foreach (var location in locations)
            {
                if (_resources.ContainsKey(location.PrimaryKey))
                {
                    Unload(location.PrimaryKey);
                }
            }
    
            Addressables.Release(locationsHandle);
        }
    
        public async UniTask UnloadLabel(AssetLabelReference label)
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            var locations = await locationsHandle.Task;        
    
            foreach (var location in locations)
            {
                if (_resources.ContainsKey(location.PrimaryKey))
                {
                    Unload(location.PrimaryKey);
                }
            }
    
            Addressables.Release(locationsHandle);
        }
    
        public void Unload(string path)
        {
            if (_resources.TryGetValue(path, out var asset))
            {
                Addressables.Release(asset);
                _resources.Remove(path);
            }
        }
    
        public void UnloadAll()
        {
            foreach (var kvp in _resources)
            {
                Addressables.Release(kvp.Value);
            }
            _resources.Clear();
        }
        #endregion
    
        private async UniTask<string> GetPrimaryKey(AssetReference reference)
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(reference);
            var locations = await locationsHandle.Task;
            string primaryKey = locations.FirstOrDefault()?.PrimaryKey ?? reference.RuntimeKey.ToString();
            Addressables.Release(locationsHandle);
            return primaryKey;
        }
    }
    ```
    

---

# 4. 유닛 패시브 관리

---

### 구현

- UnitPassiveController : 패시브 중앙 관리
    - `Dictionary<string, UnitPassive> _passives`를 사용하여 패시브 효과의 Key(이름 등)로 등록/조회/제거를 O(1)에 가깝게 수행
    - `AddPassiveEffect` 시 `isChange` 플래그를 통해 기존 효과를 먼저 제거(Deactive)하고 새 효과를 활성화하여, **효과의 중복이나 누락 없이** 안전하게 갱신
    - `DeActiveAllPassive` 및 `PassiveAllDeActive` 메서드를 제공하여 유닛 사망 또는 전투 종료 시 모든 패시브 효과를 **일괄적으로 정리**
- UnitPassive : 효과
    - `Active()` 시 `TriggerType`에 따라 유닛의 Status 이벤트(`OnAttack`, `OnDied`) 또는 전역 `BattleManager` 이벤트에 **옵저버 패턴**으로 함수를 등록하고, `Deactive()` 시 정확하게 해제하여 메모리 누수를 방지
    - `OnInterval` 타입의 효과는 **UniTask**를 사용하여 `OnIntervalEffectAsync` 등의 비동기 함수로 처리
    - `CancellationTokenSource`를 비동기 함수에 전달하여, 패시브 해제 시 **루프를 즉시 취소**하고 종료시켜 **안정성**을 확보
    - **스텟 버프**
        - `BuffEffectActive` 함수는 스탯 타입에 따라 `StatusController`를 통해 **일반 스탯 영구 증가**(`AddStat`) 또는 **시간 기반 버프/디버프**(`ApplyEffect`)를 분리하여 적용
    - **공격/소환**
        - `AttackSpawn`과 `Spawn` 함수는 `Manager.Resources`를 이용해 **프리팹을 동적으로 로드**하고, 유닛 레벨 및 스탯 배수 등을 적용하여 공격 오브젝트 또는 유닛을 전장에 생성

### 구현 이유

- OnInterval에서 코루틴대신 UniTask를 사용하여 오버헤드를 줄임

### 결과

- 유닛마다 각각의 적용되어야 하는 패시브를 따로 관리할 수 있었음.
    
    <img width="579" height="501" alt="image (3)" src="https://github.com/user-attachments/assets/e781ee53-0cc5-4ede-b90c-ccb4df31595b" />


### 관련 코드

- 핵심 코드
    - UnitPassiveController 핵심 로직
        
        ```csharp
        public void AddPassiveEffect(SynergyEffect effect, int statMultiplier = 1, bool isChange = false)
        {
            if (isChange) // 효과 갱신 시 기존 효과 제거
                RemovePassiveEffect(effect); 
        
            if (!_passives.ContainsKey(effect.Key))
            {
                _passives.Add(effect.Key, new UnitPassive(effect, _owner, statMultiplier));
                _passives[effect.Key].Active(); // UnitPassive 활성화 위임
            }
        }
        ```
        
- 전체 코드
    - UnitPassiveController
        
        ```csharp
        public class UnitPassiveController
        {
            private UnitBase _owner;
            private Dictionary<string, UnitPassive> _passives = new Dictionary<string, UnitPassive>(128);
        
            public UnitPassiveController(GameObject owner)
            {
                _owner = ComponentProvider.Get<UnitBase>(owner);
            }
        
            public void AddPassiveEffect(SynergyEffect effect, int statMultiplier = 1, bool isChange = false)
            {
                if (isChange)
                {
                    RemovePassiveEffect(effect);
                }
        
                if (!_passives.ContainsKey(effect.Key))
                {
                    _passives.Add(effect.Key, new UnitPassive(effect, _owner, statMultiplier));
                    _passives[effect.Key].Active();
                }
            }
        
            public void RemovePassiveEffect(SynergyEffect effect)
            {
                if (_passives.ContainsKey(effect.Key))
                {
                    _passives[effect.Key].Deactive();
                    _passives.Remove(effect.Key);
                }
            }
        
            public void DeActiveAllPassive()
            {
                PassiveAllDeActive();
                _passives.Clear();
            }
        
            public void PassiveAllDeActive()
            {
                foreach (var passive in _passives.Values)
                {
                    passive.Deactive();
                }
            }
        
            public void RefreshBaseStats()
            {
                foreach (var passive in _passives.Values)
                {
                    if(passive.Effect.TriggerType != TriggerType.Base)
                    
                    passive.Active();
                }
            }
        }
        ```
        
    - UnitPassive Active함수
        
        ```csharp
        public class UnitPassive
        {
            public SynergyEffect Effect;
            private UnitBase _owner;
            private int _currentActivations;
            private int _mulriplier;
            private bool _isActive;
        
            private CancellationTokenSource _cts; // Interval 루프 중단용
        
            public UnitPassive(SynergyEffect effect, UnitBase owner, int statMulriplier = 1)
            {
                _currentActivations = 0;
                Effect = effect;
                _owner = owner;
                _mulriplier = statMulriplier;
        
                _cts = new CancellationTokenSource();
            }
            #region Active & Deactive
            /// <summary>
            /// 패시브 발동 시작
            /// </summary>
            public void Active()
            {
                switch (Effect.TriggerType)
                {
                    case TriggerType.Base:
                        EffectActives();
                        break;
                    case TriggerType.OnDied:
                        _owner.StatusController.OnDied += EffectActives;
                        break;
                    case TriggerType.OnAttack:
                        _owner.StatusController.OnAttack += EffectActives;
                        break;
                    case TriggerType.OnUseSkill:
                        _owner.StatusController.OnSkill += EffectActives;
                        break;
                    case TriggerType.OnInterval:
                        BattleManager.OnBattleStarted += OnInterval;
                        BattleManager.OnBattleEnded += TokenClear;
                        break;
        
                    case TriggerType.OnBattleStart:
                        BattleManager.OnBattleStarted += EffectActives;
                        break;
        
                    case TriggerType.OnBattleEnded:
                        BattleManager.OnBattleEnded += EffectActives;
                        break;
                }
            }
        
            /// <summary>
            /// 패시브 비활성화 및 정리
            /// </summary>
            public void Deactive()
            {
                TokenClear();
                _isActive = false;
        
                switch (Effect.TriggerType)
                {
                    case TriggerType.Base:
                        RemoveStat();
                        break;
                    case TriggerType.OnAttack:
                        _owner.StatusController.OnAttack -= EffectActives;
                        break;
                    case TriggerType.OnDied:
                        _owner.StatusController.OnDied -= EffectActives;
                        break;
                    case TriggerType.OnUseSkill:
                        _owner.StatusController.OnSkill -= EffectActives;
                        break;
                    case TriggerType.OnInterval:
                        BattleManager.OnBattleStarted -= OnInterval;
                        BattleManager.OnBattleEnded -= TokenClear;
                        break;
        
                    case TriggerType.OnBattleStart:
                        BattleManager.OnBattleStarted -= EffectActives;
                        break;
        
                    case TriggerType.OnBattleEnded:
                        BattleManager.OnBattleEnded -= EffectActives;
                        break;
                }
            }
            #endregion
        }
        ```
        
    - UnitPassive 나머지 함수
        
        ```csharp
        #region Interval
        private void OnInterval()
        {
            if (Effect.IsDelay)
                OnDelayEffectAsync(_cts.Token).Forget();
            else
                OnIntervalEffectAsync(_cts.Token).Forget();
        }
        
        /// <summary>
        /// 일반적인 Interval 효과 (MaxActivations 만큼 실행)
        /// </summary>
        private async UniTask OnIntervalEffectAsync(CancellationToken token)
        {
            while (_currentActivations < Effect.MaxActivations && !token.IsCancellationRequested)
            {
                EffectActives();
        
                try
                {
                    await UniTask.WaitForSeconds(Effect.Interval, cancellationToken: token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        
            if (Effect.NextEffect != null)
            {
                await RunNextEffectAsync(token);
            }
        }
        
        /// <summary>
        /// Delay 효과 (첫 효과 후 Delay → NextEffect)
        /// </summary>
        private async UniTask OnDelayEffectAsync(CancellationToken token)
        {
            while (_currentActivations < Effect.MaxActivations && !token.IsCancellationRequested)
            {
                try
                {
                    await UniTask.WaitForSeconds(Effect.Interval, cancellationToken: token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
        
                EffectActives();
            }
        
            // Delay 대기
            try
            {
                await UniTask.WaitForSeconds(Effect.DelayTime, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        
            // NextEffect 실행
            if (Effect.NextEffect != null)
            {
                await RunNextEffectAsync(token);
            }
        }
        
        /// <summary>
        /// NextEffect 실행 로직 (Interval 여부에 따라 단발/주기)
        /// </summary>
        private async UniTask RunNextEffectAsync(CancellationToken token)
        {
            if (Effect.NextEffect.Interval > 0)
            {
                while (!token.IsCancellationRequested)
                {
                    NextEffectActives();
        
                    try
                    {
                        await UniTask.WaitForSeconds(Effect.NextEffect.Interval, cancellationToken: token);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                }
            }
            else
            {
                NextEffectActives();
            }
        }
        #endregion
        
        private void RemoveStat()
        {
            if (!Effect.IsBuff)
                return;
        
            foreach (var stat in Effect.StatModifiers)
            {
                _owner.StatusController.RemoveStat(stat.StatType, Effect.Key);
            }
        }
        
        private void NextEffectActives()
        {
            if (Effect.NextEffect == null)
                return;
        
            if (Effect.NextEffect.EffectApplyType == EffectApplyType.All)
                return;
        
            NextBuffEffectActive();
            NextAttackActive();
        }
        
        #region BuffEffect
        private void BuffEffectActive()
        {
            if (!Effect.IsBuff)
                return;
        
            BuffEffectActive(Effect);
        }
        private void NextBuffEffectActive()
        {
            if (!Effect.NextEffect.IsBuff)
                return;
        
            BuffEffectActive(Effect.NextEffect);
        }
        private void BuffEffectActive(SynergyEffect _effect)
        {
            switch (_effect.EffectType)
            {
                case EffectType.Buff_Debuff:
                    for (int i = 0; i < _effect.SynergyBuffDatas.Length; i++)
                    {
                        SynergyBuffData data = _effect.SynergyBuffDatas[i];
                        _owner.StatusController.ApplyEffect(
                            new BuffEffectData { StatType = data.StatType, Duration = data.Duration },
                            data.Value * _mulriplier, _effect.Key);
                    }
                    break;
        
                case EffectType.Increase:
                    foreach (var stat in _effect.StatModifiers)
                    {
                        if (stat.StatType == StatType.CurHp)
                            _owner.StatusController.IncreaseHealth(Mathf.RoundToInt(stat.Value * _mulriplier));
                        else if (stat.StatType == StatType.CurMana)
                            _owner.StatusController.IncreaseMana(Mathf.RoundToInt(stat.Value * _mulriplier));
                        else if (stat.StatType == StatType.Shield)
                            _owner.StatusController.IncreaseShield(Mathf.RoundToInt(stat.Value * _mulriplier));
                        else
                            _owner.StatusController.AddStat(stat.StatType, stat.Value * _mulriplier, $"{_effect.Key}__{_currentActivations}");
                    }
                    break;
            }
        }
        #endregion
        
        #region AttackEffect
        private void AttackActive()
        {
            if (!Effect.IsAttack)
                return;
        
            AttackSpawn(Effect);
        }
        
        private void NextAttackActive()
        {
            if (!Effect.NextEffect.IsAttack)
                return;
        
            AttackSpawn(Effect.NextEffect);
        }
        
        private void AttackSpawn(SynergyEffect effect)
        {
            if (effect.AttackPrefab == null)
            {
                Debug.LogWarning($"[시너지 공격 시스템] 해당 주소에 Prefab이 없습니다. 주소 : {effect.AttackAddress}");
                return;
            }
        
            if (_owner.Target == null)
                return;
        
            GameObject attackObj = effect.SpawnPositionType == SpawnPositionType.Self ?
                Manager.Resources.Instantiate(effect.AttackPrefab, _owner.transform.position, Quaternion.identity, true) :
                Manager.Resources.Instantiate(effect.AttackPrefab, _owner.Target.position, Quaternion.identity, true);
        
            AttackObject attack = ComponentProvider.Get<AttackObject>(attackObj);
        
            attack.Setup(effect.Power, effect.AttackDealy);
        }
        #endregion
        
        #region SpawnEffect
        private void SpawnActive()
        {
            if (!Effect.IsSpawn)
                return;
        
            Vector3 pos = _owner.transform.position;
            Spawn(pos);
        }
        
        private void Spawn(Vector3 pos)
        {
            if (Effect.SpawnPrefab == null)
            {
                Debug.LogWarning($"[시너지 스폰 시스템] 해당 주소에 Prefab이 없습니다. 주소 : {Effect.SpawnAddress}");
                return;
            }
        
            Debug.Log($"[시너지 스폰 시스템] {Effect.SpawnPrefab.name} 소환");
        
            GameObject spawnEffect = Manager.Resources.Instantiate<GameObject>(Effect.SpawnEffectPrefab, pos, true);        
            Manager.Resources.Destroy(spawnEffect, 2);
        
            GameObject unitObj = GameObject.Instantiate(Effect.SpawnPrefab, Vector3.zero, Quaternion.identity);
            UnitBase spawnUnit = ComponentProvider.Get<UnitBase>(unitObj);
            UnitData data = Manager.Resources.Load<UnitData>(Effect.UnitDataAddress);
        
            spawnUnit.StatusController.Invincible();
        
            if (spawnUnit == null)
                return;
        
            if (Effect.SpawnType == SpawnStatType.Level)
            {
                spawnEffect.transform.localScale = new Vector3(Effect.UnitLevel, Effect.UnitLevel, Effect.UnitLevel);
        
                if (Effect.IsMultiplier)
                {
                    spawnUnit.StatusController.StatMultiplier = Effect.UnitStatMultiplier;
                    spawnUnit.Status = new UnitStatus(data, Effect.UnitLevel);
                    spawnUnit.Init(Effect.SpawnUnitStats);
                    spawnUnit.Fight();
                }
                else
                {
                    spawnUnit.Init();
                }
            }
            else if (Effect.SpawnType == SpawnStatType.LowUpgrade)
            {
                int level = _owner.Status.Level - 1 >= 0 ? _owner.Status.Level - 1 : 0;
                spawnUnit.Status = new UnitStatus(data, level);
                spawnUnit.Init();
                spawnUnit.Fight();
            }
        
            unitObj.transform.position = pos;
            BattleManager.OnSpawnUnit?.Invoke(spawnUnit);
        }
        #endregion
        #endregion
        ```
        

---

# 5. 시너지 시스템 구현

---

### 구현

- 실시간 시너지 카운트 및 이벤트 전달
    - `SynergyController`는 유닛의 **유닛/직업 시너지**를 분리하여 `_synergyCounts` 배열에 저장
    - 유닛이 배치되거나 철수될 때마다 `AddSynergy`/`RemoveSynergy`를 호출하여 해당 시너지의 카운트를 업데이트하고, **`OnSynergyChanged` 이벤트를 발생**시켜 시너지 데이터를 갱신
        
      <img width="545" height="248" alt="image (4)" src="https://github.com/user-attachments/assets/1234a0a4-ee91-4020-ab0b-6ac347f7eafb" />

- 시너지 레벨 판별 및 효과 전환
    - `CheckSynergy` 메서드는 `SynergyController`의 최신 카운트를 받고 SynergyDB에서 `SynergyData`를 받아와 `SynergyData`의 `Check` 메서드를 호출
    - `SynergyData.Check()`는 현재 카운트에 따라 가장 높은 `SynergyLevelData`를 판별
    - 새로운 효과가 결정되면, 기존 효과를 모두 해제하고 새로운 효과를 모두 적용
        
      <img width="514" height="437" alt="image (5)" src="https://github.com/user-attachments/assets/7ae9375f-3098-443e-b7dc-1842f3417b41" />

        
- 복합 타겟팅 및 패시브 적용
    - EffectTargetType에 따라 유닛 배열에서 **효과를 받을 특정 유닛 그룹**을 필터링
    - 필터링된 타겟 유닛들에게 개별 `UnitPassiveController`를 통해 **패시브 효과를 위임 및 등록**
- 글로벌 패시브 연동
    - **EffectApplyType.All** 또는 **NextEffect**가 글로벌 효과일 경우, `SynergyEffectManager.Instance.GlobalPassiveController`를 통해 전역적으로 효과를 등록/해제하여, 개별 유닛이 아닌 전장에 영향을 미치는 시너지 효과를 관리할 수 있도록 시스템을 확장

### 결과

- 모든 유닛의 시너지 효과 동기화 됨
- 클래스 마다 역할을 나눠 SynergyController를 로비의 시너지 카운팅에 재사용
    
    **<전체 구성>**
    
    <img width="1022" height="448" alt="image (6)" src="https://github.com/user-attachments/assets/c860d54c-7cd2-4462-8e15-fc2179fa74be" />
    

### 관련 코드

- SynergyData
    
    ```csharp
    public abstract class SynergyData : ScriptableObject
    {
        public Sprite ActiveIcon => Manager.Resources.SpriteLoad(activeIconAddress);
        public Sprite DeActiveIcon => Manager.Resources.SpriteLoad(deActiveIconAddress);
        protected string activeIconAddress;
        protected string deActiveIconAddress;
        [Space]
    
        public Color SynergyColor;
        public string SynergyName;
        [TextArea]
        public string Description;
        public SynergyLevelData[] SynergyLevelData;
    
        private SynergyEffect[] _currentEffects;
        public int CurrentUpgradeIdx;
        protected int _synergy;
    
        public virtual void Init()
        {
            _currentEffects = new SynergyEffect[SynergyLevelData[0].Effects.Length];
            CurrentUpgradeIdx = -1;
        }
    
        public void Check(int newCount, UnitBase[] units)
        {
            SynergyEffect[] newEffects = null;
    
            CurrentUpgradeIdx = -1;
    
            for (int i = 0; i < SynergyLevelData.Length; i++)
            {
                if (newCount >= SynergyLevelData[i].SynergyNeedCount)
                {
                    newEffects = SynergyLevelData[i].Effects;
                    CurrentUpgradeIdx = i;
                }
                else
                {
                    break;
                }
            }
    
            if (newEffects == null)
                return;
    
            foreach (var effect in _currentEffects)
            {
                effect?.RemoveEffect(units, _synergy);
            }
    
            foreach (var effect in newEffects)
            {
                effect?.ApplyEffect(units, _synergy);
            }
    
            _currentEffects = newEffects;
        }
    }
    
    ```
    
- SynergyEffect
    
    ```csharp
    [CreateAssetMenu(fileName = "SynergyEffect", menuName = "Data/Synergy/Effect")]
    public class SynergyEffect : ScriptableObject
    {
        //[Header("MetaData")]
        private string GuidKey = Guid.NewGuid().ToString();
        public string Key => name;
    
        [TextArea]
        public string Description;
        public bool IsActivationsClear;
        public bool IsAttack;
        public bool IsSpawn;
        public bool IsBuff;
        public bool IsDelay;
        public bool IsFirstOnly;
    
        //[Header("Delay")]
        public float DelayTime;
    
        //[Header("SpawnEffect")]
        public string SynergyEffectAddress;
        public float EffectDuration = 2;
    
        //[Header("SpawnType (유닛 소환)")]
        public bool IsUnitPosition;         // 소환 위치 정의 (유닛위치 or 전장 중앙)
        public int UnitStatMultiplier { get; private set; }
        public string UnitDataAddress;
        [Range(0, 2)] public int UnitLevel;
        public UnitStats SpawnUnitStats;    // 가중치
        public bool IsMultiplier;           // 시너지 유닛의 Level에 따른 배수 적용 여부
    
        public SpawnStatType SpawnType;     // 유닛소환 시 스텟 타입 설정
        public Synergy SpawnSynergy;        // 유닛을 소환하는 시너지
        public GameObject SpawnPrefab => Manager.Resources.Load<GameObject>(SpawnAddress);
        public GameObject SpawnEffectPrefab => Manager.Resources.Load<GameObject>(SpawnEffectAddress);
        public string SpawnAddress;
        public string SpawnEffectAddress;
    
        public EffectApplyType EffectApplyType;
    
        //[Header("AttackType (공격)")]
        public SpawnPositionType SpawnPositionType;
        public float Power;
        public float AttackDealy = 1;
        public GameObject AttackPrefab => Manager.Resources.Load<GameObject>(AttackAddress);
        public string AttackAddress;
    
        //[Header("EffectType")]
        public EffectType EffectType;
        public SynergyBuffData[] SynergyBuffDatas;
        public StatEffectModifier[] StatModifiers;
    
        //[Header("TriggerType")]
        public TriggerType TriggerType;
        public float Interval;
        public uint MaxActivations;
    
        //[Header("TargetType")]
        public EffectTargetType TargetType;
    
        //[Header("NextEffect")]
        public SynergyEffect NextEffect;    
    
        public void ApplyEffect(UnitBase[] units, int synergy)
        {
            if(IsMultiplier)
                UnitStatMultiplier = units.GetSynergyUnitsTotalLevel(SpawnSynergy);
    
            if (TargetType == EffectTargetType.Cross)
            {
                ActiveCross(units, synergy);
                return;
            }
    
            if (EffectApplyType == EffectApplyType.Self)
            {
                foreach (var unit in GetTarget(units, synergy))
                {
                    unit.StatusController.PassiveController.AddPassiveEffect(this);
                }
            }
            else
            {
                SynergyEffectManager.Instance.GlobalPassiveController.AddPassiveEffect(this, GetTarget(units, synergy), isChange: true, delay: DelayTime);
            }
    
            if(NextEffect != null && NextEffect.EffectApplyType == EffectApplyType.All)
            {
                SynergyEffectManager.Instance.GlobalPassiveController.AddPassiveEffect(NextEffect, GetTarget(units, synergy), isChange: true, delay: DelayTime);
            }
        }
    
        public void RemoveEffect(UnitBase[] units, int synergy)
        {
            if (EffectApplyType == EffectApplyType.Self)
            {
                foreach (var unit in GetTarget(units, synergy))
                {
                    unit.StatusController.PassiveController.RemovePassiveEffect(this);
                }
            }
            else
            {
                SynergyEffectManager.Instance.GlobalPassiveController.RemovePassiveEffect(this);
            }
    
            if (NextEffect != null && NextEffect.EffectApplyType == EffectApplyType.All)
            {
                SynergyEffectManager.Instance.GlobalPassiveController.RemovePassiveEffect(NextEffect);
            }
        }
    
        private void ActiveCross(UnitBase[] units, int synergy)
        {
            foreach (UnitBase unit in units)
            {
                if (unit == null) continue;
    
                int unitSynergy = (int)unit.Status.Data.Synergy;
                int unitClassSynergy = (int)unit.Status.Data.ClassSynergy;
    
                if (!(unitSynergy == synergy) && !(unitClassSynergy == synergy))
                    continue;
    
                int synergyCount = 0;
                var currentSlot = unit.CurrentSlot;
    
                var directions = new Vector2Int[]
                {
                        new Vector2Int(0, 1),
                        new Vector2Int(0, -1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(1, 0)
                };
    
                foreach (var dir in directions)
                {
                    var targetPos = new Vector2Int(currentSlot.x + dir.x, currentSlot.y + dir.y);
    
                    // 해당 위치에 유닛이 있는지 찾기                
                    var targetUnit = units.FirstOrDefault(u => u != null &&
                                                               u.CurrentSlot.x == targetPos.x &&
                                                               u.CurrentSlot.y == targetPos.y);
    
                    if (targetUnit != null)
                    {
                        unitSynergy = (int)targetUnit.Status.Data.Synergy;
                        unitClassSynergy = (int)targetUnit.Status.Data.ClassSynergy;
    
                        if (unitSynergy == synergy || unitClassSynergy == synergy)
                        {
                            synergyCount++;
                        }
                    }
                }
    
                if (synergyCount == 0)
                    continue;
    
                unit.StatusController.PassiveController.AddPassiveEffect(this, synergyCount, true);
            }
        }
    
        protected UnitBase[] GetTarget(UnitBase[] units, int synergy)
        {
            List<UnitBase> unitBases = new List<UnitBase>();
    
            if (TargetType == EffectTargetType.Ally)
            {
                foreach (var unit in units)
                {
                    if(unit != null)
                        unitBases.Add(unit);
                }
                return unitBases.ToArray();
            }            
    
            switch (TargetType)
            {
                case EffectTargetType.SameSynergy:
                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
    
                        if ((int)unit.Status.Data.Synergy == synergy ||
                            (int)unit.Status.Data.ClassSynergy == synergy)
                            unitBases.Add(unit);
                    }
                    break;
                case EffectTargetType.Column:
                    var targetColumns = new HashSet<int>();
    
                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
    
                        if ((int)unit.Status.Data.Synergy == synergy ||
                            (int)unit.Status.Data.ClassSynergy == synergy)
                        {
                            int column = unit.CurrentSlot.x;
                            targetColumns.Add(column);
                        }
                    }
    
                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
    
                        int unitColumn = unit.CurrentSlot.x;
                        if (targetColumns.Contains(unitColumn))
                        {
                            unitBases.Add(unit);
                        }
                    }
                    break;
                case EffectTargetType.Row:
                    var targetRows = new HashSet<int>();
    
                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
    
                        if ((int)unit.Status.Data.Synergy == synergy ||
                            (int)unit.Status.Data.ClassSynergy == synergy)
                        {
                            int Row = unit.CurrentSlot.y;
                            targetRows.Add(Row);
                        }
                    }
    
                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
    
                        int unitRow = unit.CurrentSlot.y;
                        if (targetRows.Contains(unitRow))
                        {
                            unitBases.Add(unit);
                        }
                    }
                    break;
                case EffectTargetType.ColumnAndRow:
                    var targetCols = new HashSet<int>();
                    var targetRowsSet = new HashSet<int>();
                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
    
                        if ((int)unit.Status.Data.Synergy == synergy ||
                            (int)unit.Status.Data.ClassSynergy == synergy)
                        {
                            int column = unit.CurrentSlot.x;
                            int row = unit.CurrentSlot.y;
                            targetCols.Add(column);
                            targetRowsSet.Add(row);
                        }
                    }
                    foreach (var unit in units)
                    {
                        if (unit == null) continue;
    
                        int unitColumn = unit.CurrentSlot.x;
                        int unitRow = unit.CurrentSlot.y;
                        if (targetCols.Contains(unitColumn) || targetRowsSet.Contains(unitRow))
                        {
                            unitBases.Add(unit);
                        }
                    }
                    break;
            }
    
            return unitBases.ToArray();
        }
    }
    ```
    
- SynergyController
    
    ```csharp
    public class SynergyController : MonoBehaviour
    {
        private int[] _synergyCounts;
        public event Action<int, int> OnSynergyChanged;
    
        public void Init()
        {
            _synergyCounts = new int[(int)Synergy.Length];
        }
    
        public void AddSynergy(Synergy unitSynergy, ClassType classSynergy)
        {
            int unitSynergyIdx = (int)unitSynergy;
            int classSynergyIdx = (int)classSynergy;
    
            _synergyCounts[unitSynergyIdx]++;
            _synergyCounts[classSynergyIdx]++;        
    
            OnSynergyChanged?.Invoke(unitSynergyIdx, _synergyCounts[unitSynergyIdx]);
            OnSynergyChanged?.Invoke(classSynergyIdx, _synergyCounts[classSynergyIdx]);
        }
    
        public void RemoveSynergy(Synergy unitSynergy, ClassType classSynergy)
        {
            int unitSynergyIdx = (int)unitSynergy;
            int classSynergyIdx = (int)classSynergy;
    
            _synergyCounts[unitSynergyIdx]--;
            _synergyCounts[classSynergyIdx]--;
    
            OnSynergyChanged?.Invoke(unitSynergyIdx, _synergyCounts[unitSynergyIdx]);
            OnSynergyChanged?.Invoke(classSynergyIdx, _synergyCounts[classSynergyIdx]);
        }
    
        public int GetSynergyUnitCount(int synergyIdx)
        {
            return _synergyCounts[synergyIdx];
        }
    }
    ```
    
- CheckSynergy
    
    ```csharp
    private void CheckSynergy(UnitBase unit)
    {
        SynergyData synergy = Manager.Data.SynergyDB.GetSynergy((int)unit.Status.Data.Synergy);
        SynergyData classSynergy = Manager.Data.SynergyDB.GetSynergy((int)unit.Status.Data.ClassSynergy);
    
        if (synergy == null) return;
    
        synergy.Check(SynergyController.GetSynergyUnitCount((int)unit.Status.Data.Synergy), GetUnits());
        classSynergy.Check(SynergyController.GetSynergyUnitCount((int)unit.Status.Data.ClassSynergy), GetUnits());
    }
    ```
    

---

# 6. 데이터 관리

---

### 구현

- `DataManager`와 `DBManager`를 싱글톤으로 구성
- `DataManager`에서 데이터를 관리, `DBManager`에서 DB 관리
- `DBManager`에서 이벤트를 통해 `DataManager` 데이터 업데이트로 실시간 데이터 유지
    - **DataManager**
        
        ```csharp
        public class DataManager : Singleton<DataManager>
        {
            // 유닛 데이터 관련
            public Dictionary<string, UnitData> UnitDataDic;    
            public UnitData[] EnemyUnitDatas;
            public UnitData[] PlayerUnitDatas;
        
            // 시너지
            public SynergyDatabase SynergyDB;
        
            // 스테이지 관련
            public StageGameData StageGameData = new();
            public StageDatas StageDatas = new();
        
            // 인게임
            public UnitSpawnChanceData UnitSpawnChanceData;
            public CharacterSellAmountData CharacterSellAmountData;
            public AugmentChanceData AugmentChanceData;
            public PriceDatas PriceDatas;
            public AUGData[] AugmentDatas;
        
            // 애니메이션 데이터
            public AnimationManager AnimationManager = new();
        
            // 마법석 데이터 임시로 추가
            public Dictionary<string, MagicStone> MagicStoneDic;
            public MagicStoneData[] MagicStoneDatas;
            public MagicStone[] MagicStones;
            public MagicStonLevelChanceData MagicStoneLevelChanceData;
        
            // 프리셋 데이터 관련
            public PresetDatabase PresetDB { get; private set; } = new PresetDatabase();
        
            // 맵 데이터 관련
            public MapDatabase MapDB { get; private set; } = new MapDatabase();
            public StageGridData StageGridData = new();
            public EventData[] EventDatas;
        }
        ```
        
    - **DBManager**
        
        ```csharp
        public class DBManager : Singleton<DBManager>
        {
            public CharDB charDB = new CharDB();
            public StageDB stageDB = new StageDB();
            public MagicStoneDB magicStoneDB = new MagicStoneDB();
            public QuestDB questDB = new QuestDB();
            public ShopDB shopDB = new ShopDB();
            public TimeDB timeDB = new TimeDB();
        }
        ```
        
        - 이벤트 구독 코드 예시
            
            ```csharp
            public void EventHandler()
            {
                var baseRef = FirebaseManager.DataReference
                    .Child("UserData")
                    .Child(_uid)
                    .Child(StageDataPath);
            
                baseRef.ChildChanged += UpdateStageClearDatas;
                baseRef.ChildAdded += OnNewRegionAdded;
            
                LoadExistingRegionsAndAttachListeners(baseRef).Forget();
            
                Debug.Log("StageData 상위 리스너 및 기존 지역 리스너 등록 완료");
            }
            
            private async UniTaskVoid LoadExistingRegionsAndAttachListeners(DatabaseReference baseRef)
            {
                var snapshot = await baseRef.GetValueAsync();
                if (!snapshot.Exists)
                {
                    Debug.Log("DB에 스테이지 데이터 없음. 리스너는 새로 생길 때 자동 추가됨.");
                    return;
                }
            
                foreach (var regionChild in snapshot.Children)
                {
                    var regionKey = regionChild.Key;
                    var regionRef = baseRef.Child(regionKey);
                    regionRef.ChildChanged += UpdateStageClearDatas;
                    Debug.Log($"기존 지역 리스너 등록 완료 → {regionKey}");
                }
            }
            ```
            
<img width="863" height="497" alt="image (7)" src="https://github.com/user-attachments/assets/6d7436c0-83bd-478d-b302-35bbf79dec6f" />

### 구현 이유

- 싱글톤으로 구성하여 데이터의 **유일성과 무결성을 보장**
- 전역적인 접근점을 제공하여 **쉬운 데이터 참조**
- 이벤트 구조를 통한 **실시간 데이터 반영**

### 결과

- 데이터에 대한 참조가 쉬워 **개발속도 향상**
- 구조에 대한 **복잡성 Down**

### 고려할 점

- 하지만 싱글톤에 의존성을 지니게되고 단위 테스트가 어려워짐
- DI(의존성 주입)도 고려해야함

---

# 7. 단위 테스트 환경 구축

---

### 구현

- `ButtonAttribute`를 함수위에 기입
- 해당 함수를 실행하는 버튼이 커스텀 에디터를 통해 인스펙터에 그려지도록 구성.

![TestButtonAttribute](https://github.com/user-attachments/assets/1d312d83-d68a-4d50-a152-1836b82a8062)

### 구현 이유

- 스킬을 테스트 하려면 게임을 시작해 스테이지에 입장해야하는 등 작은 단위를 테스트하는데 불편함이 있었음.

### 결과

- 작은 단위 테스트에 대한 시간을 많이 줄일 수 있었음.

### 관련  코드

- **ButtonAttribute**
    
    ```csharp
    public class ButtonAttribute : Attribute
    {
        public string ButtonName;
    
        public ButtonAttribute(string buttonName = "")
        {
            ButtonName = buttonName;
        }
    }
    ```
    
- **CustomEditor**
    
    ```csharp
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonAttributeDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
    
            var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    
            foreach (var method in methods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                if (buttonAttribute != null)
                {
                    string buttonName = string.IsNullOrEmpty(buttonAttribute.ButtonName) ? method.Name : buttonAttribute.ButtonName;
    
                    if (GUILayout.Button(buttonName))
                    {
                        method.Invoke(target, null);
                    }
                }
            }
        }
    }
    ```
    
- **실제 적용 코드**
    
    ```csharp
    #if UNITY_EDITOR
            [Button("Open")]
            public void TestOpen()
            {
                OpenShop();
            }
    
            [Button("Close")]
            public void TestClose()
            {
                CloseShop();
            }
    #endif
    ```
    

---

# 8. 기획을 고려한 유닛 공격, 스킬 데이터 캡슐화

---

### 구현

- `Attack`, `Skill SO`데이터를 UnitData에 주입하는 방식으로 구성
- 공격 애니메이션 시 `AttackData.Attack()`, 스킬 애니메이션 시`SkillData.Active()`를 실행
    
    <img width="473" height="559" alt="image (8)" src="https://github.com/user-attachments/assets/29f2b673-5946-4ccb-8a0c-1b2719d476ef" />


### 구현 이유

- 프로젝트 초반부터 제작에 들어갔어야 했고 기획의 변경점이 있을 수도 있다는 이야기를 들음
- `Attack`과 `Skill`을 캡슐화 하여 제작하면 추후 기획이 변경됨에 따라 주입을 바꾸는 식으로 유연하게 가능할거라고 생각했음

### 결과

- 실제로 기획에서 일반공격이 변경됨에 따라 비슷한 형식을 가지게 되었고 데이터를 재활용 할 수 있었음
- 기획이 변경되어도 유연한 대처가 가능했음

### 관련 코드

- **UnitData(Field)**
    
    ```csharp
    public class UnitData : MetaData
    {
        [Header("MetaData")]
        public Grade Grade;
        public string AddressableAddress;
        public GameObject UnitPrefab => Manager.Resources.Load<GameObject>(AddressableAddress);
        public int ID;
        public int PerferredLine;
        public int Cost;
    
        [Header("AnimationData")]
        public bool isNotChange;
        public AnimatorData AnimatiorData;
    
        [Header("Attack_Data")]
        public UnitSkill Skill;
        public UnitAttackData AttackData; // Melee, Ranged 등 공격 타입에 따라 다름
    
        [Header("Unit_Stat")]    
        public UnitStats[] UnitStats; // 3개 1,2,3 성
        public UnitStats[] UpgradeStats; // UnitStats의 레벨 강화 반영
    
        [Header("Synergy")]
        public ClassType ClassSynergy;
        public Synergy Synergy;
    
        [Header("Upgrade")]
        public UpgradeUnitData UpgradeData;
        public LevelUpData LevelUpData;
        public UpgradeStatData UpgradeStatData;
    }
    ```
    
- **AttackData**
    
    ```csharp
    public abstract class UnitAttackData : ScriptableObject
    {
        [Header("ID")]
        public int ID;
    
        public DamageType DamageType;
        public float AttackPower;
    
        [Header("Effect")]
        public string EffectAddress;
    
        public virtual void Attack(IAttacker attacker)
        {
            
        }
    }
    ```
    

---

# 9. 아군, 적 유닛 데이터 공유

---

### 구현

- 오토배틀러 특성 상 아군, 적군 모두 동일한 로직으로 동작이 가능
- 아군, 적 유닛을 `Layer`로 판단
- 프리팹을 공용으로 사용가능하게 하여 아군 유닛도 적 유닛으로 세팅이 가능하도록 세팅
- 기본적으로 프리팹을 아군유닛으로 맞추고 적을 세팅할때에는 `Layer`변경등의 작업을 수행
- `FSM`의 상태또한 공유하도록 구성

### 구현 이유

- 기획서를 분석 후 아군 유닛과 적 유닛이 크게 다르지 않은 것을 확인하였기 때문에 공유 함으로써 코드의 재사용성을 높이기 위해

### 결과

- 아군인가, 적인가 에 대한 신경을 쓰지 않아도 되어서 **개발속도 향상**
- 하나로 묶여 있다보니 스킬작동방식, 공격 방식, 상태 등이 변경되어도 한번에 변경가능

### 고려할 점

- 만약 추후 적에게만 해당하는 매커니즘이 추가되었을 때 변경이 어렵고
- 코드가 더 복잡해질 가능성이 있음

---

# 10. 파싱 간편화

---

### 구현

- `CsvLoadData`라는 파싱 데이터를 모두 가지는 데이터를 구성
- 데이터를 `List`로 받고 해당 데이터들은 세팅된 데이터를 바탕으로 `GetURL()`시 알맞는 포멧의 URL로 변환
- 파서가 해당 URL과 세팅된 데이터를 기반으로 파싱
    <img width="636" height="654" alt="image (9)" src="https://github.com/user-attachments/assets/03268774-d0f7-4cf5-8803-ad54459a16d7" />
    <img width="1297" height="685" alt="image (10)" src="https://github.com/user-attachments/assets/78070921-4069-461a-8ba3-3e06111ffefb" />

### 구현 이유

- 파싱할 데이터 테이블이 생길때 마다 병렬파싱에 추가 등 코드적으로 수정해야할 것들이 많았기 때문

### 결과

- 간단히 코드에 `Enum`추가 및 함수를 세팅하고 SO에서 데이터 세팅만 하면 자동으로 병렬적으로 Load되도록 처리가 가능

### 관련 코드

- **CsvData**
    
    ```csharp
    [Serializable]
    public struct CsvData
    {
        public CsvType CsvType;
        public int StartLine;
    
        [TextArea]
        [SerializeField] string _url;
    
        [Header("예시 : A2:C12")]
        [SerializeField] string range;
    
        [SerializeField] int gid;
    
        public string GetURL()
        {
            string baseUrl = _url;
            
            int editIndex = baseUrl.IndexOf("/edit");
            if (editIndex > -1)
            {
                baseUrl = baseUrl.Substring(0, editIndex);
            }
    
            string result = "";
    		
    		// tsv
            if(string.IsNullOrEmpty(range))
                result = $"{baseUrl}/export?format=tsv&gid={gid}";
            else
                result = $"{baseUrl}/export?format=tsv&gid={gid}&range={range}";
    
            return result;
        }
    }
    ```
    
- **파서 구성**
    - **LoadCSV**
        
        가져온 데이터를 알맞은 string 2차원 배열로 만들어주는 함수
        
        ```csharp
        private async UniTask LoadCSV(string url, Action<string[][]> onParsed, int startLine = 1)
        {
            using UnityWebRequest req = UnityWebRequest.Get(url);
        
            await req.SendWebRequest().ToUniTask();
        
            if (!string.IsNullOrEmpty(req.error))
            {
                Debug.LogError($"TSV 다운로드 실패: {url}, Error: {req.error}");
                return;
            }
        
            string raw = req.downloadHandler.text.Trim();
            string[] lines = raw.Split('\n');
            List<string[]> parsed = new();
        
            for (int i = startLine - 1; i < lines.Length; i++)
            {
                string[] row = lines[i].Trim().Split('\t');
                parsed.Add(row);
            }
        
            onParsed?.Invoke(parsed.ToArray());
        }
        ```
        
    - **GetSetupMethod**
        
        Enum으로 세팅된 세팅 함수를 가져오는 함수
        
        ```csharp
        private Action<string[][]> GetSetupMethod(CsvType csvType)
        {
            switch(csvType)
            {
                case CsvType.PlayerUnit:
                    return PlayerUnitStatSetup;
                case CsvType.PlayerSkillData:
                    return PlayerSkillSetup;
                case CsvType.Monster: 
                    return MonsterSetup;
                case CsvType.MonsterSkillData:
                    return MonsterSkillSetup;
                case CsvType.StageFirstReward:
                    return StageFirstRewardSetup;
                case CsvType.StageReward:
                    return StageRewardSetup;
                case CsvType.Floor:
                    return FloorRewardSetup;
                default:
                    Debug.LogError($"알 수 없는 CSV 이름: {csvType.ToString()}");
                    return null;
            }
        }
        ```
        
    - **csvLoadData기반 병렬적 로드**
        
        csvLoadData기반 이기 때문에 새로운 파싱 데이터를 SO에 추가만 해주면 병렬적으로 실행
        
        ```csharp
        List<UniTask> tasks = new List<UniTask>(10);
        
        foreach (var csvData in _csvLoadData.CsvDatas)
        {
            tasks.Add(LoadCSV(csvData.GetURL(), GetSetupMethod(csvData.CsvType), csvData.StartLine));
        }
        
        await UniTask.WhenAll(tasks);
        ```
        

---

# 11. ComponentProvider 구현

---

### 구현

- `ComponentProvider.Add<Component>(gameObject, Component);` 로 설정
- `Dictionary`로 Component들을 관리
- Unique한 Key를 위해 `gameObject.**GetInstanceID()**`를 사용

### 구현 이유

- 프로젝트 특성상 여러 유닛 컴포넌트를 외부에서 참조해야하는 경우가 많아 참조할때 마다 `GetComponent`를 하는 것은 효율이 좋지 않다고 판단하여 컨포넌트를 캐싱하는 공용 저장소를 구성하여 보다 효율적인 환경을 구현
- `DragSystem`, `ToolTip` 등에서 반복적으로 유닛 컴포넌트를 얻어야 하는 경우가 많아
- 오버헤드를 줄이기 위해 `GetComponent`를 줄이는 방향으로 Provider를 구성하게 됨

### 결과

- `GetComponent` 없이 `UnitBase`컴포넌트를얻는 것이 가능하여 오버헤드가 줄어듬
- `Dictionary`로 관리하여 **탐색속도 향상**
- 전 후 비교
    
    **<전>**
    <img width="550" height="331" alt="image (11)" src="https://github.com/user-attachments/assets/3a7d4d80-e3ee-4279-8b03-22f6f9a7a940" />
    
    **<후>**
    <img width="533" height="345" alt="image (12)" src="https://github.com/user-attachments/assets/546cd3d8-8ffa-4e9e-a0f5-727c8d6246a0" />

    

### 관련 코드

- **ComponentProvider**
    
    ```csharp
    public static class ComponentProvider
    {
        private static readonly Dictionary<string, Component> _components = new Dictionary<string, Component>(5000);
    
        public static void Add<T>(GameObject obj, T component) where T : Component
        {
            string key = GenerateKey<T>(obj);
    
            if (!_components.ContainsKey(key))
            {            
                _components[key] = component;
            }        
        }
    
        public static void Remove<T>(GameObject obj) where T : Component
        {
            string key = GenerateKey<T>(obj);
            if (_components.ContainsKey(key))
            {
                _components.Remove(key);
            }
        }
    
        public static T Get<T>(GameObject obj) where T : Component
        {
            if(obj == null)
                return null;
    
            string key = GenerateKey<T>(obj);
    
            if (_components.TryGetValue(key, out Component component))
            {            
                return component as T;
            }
            return null;
        }
    
        private static string GenerateKey<T>(GameObject obj) where T : Component    
        {
            return $"{obj.GetInstanceID()}_{typeof(T).Name}";
        }
    }
    ```
    

---

# 12. 슬롯과 유닛 배치 시스템

---

### 구현

**배틀 슬롯**

- `SlotCreater`에서 설정된 Size에 따라 Slot을 동적 생성
- 생성 시 Slot에 Vector2Int형태로 Grid 좌표 값 부여 후 `Dictionary<Vector2Int, UnitSlot>` 반환
- UnitSlotManager에서 Init 메서드 실행 시 SlotCreater가 생성한 Dictionary를 받아옴
- UnitSlotManager에서 모든 슬롯을 관리
- 생성된 Slot은 SetUnit(Unit) 함수를 통해 해당 슬롯에 유닛을 세팅

**UI 슬롯**

- `Dictionary<string, List<int>> _unitSlotDic`를 사용하여 유닛의 Address와 해당 유닛이 위치한 UI 슬롯 인덱스들을 다대다 관계로 관리.
    - 특정 유닛 종류의 보유 개수 **GetUnitCount()**를 **O(1)**에 가깝게 빠르게 조회 가능
- `UnitStatus[] _cachedUnitsArray`를 슬롯 인덱스에 매핑하여, 슬롯 인덱스 기반의 유닛 정보 조회를 최적화.

**드래그 앤 드롭**

- **전처리**기 지시자를 사용하여 PC와 모바일의 입력 로직을 완벽하게 분리.
    - **PC**: Input.GetMouseButton(), Input.GetKeyDown() 사용.
    - **모바일:** Input.GetTouch()의 TouchPhase (Began, Moved, Ended, Canceled) 상태를 활용.
- **클릭, 드래그 분리**
    - 누름 상태가 임계값 미만으로 해제될 경우 HandleShortClick()을 호출하여 유닛 상세 정보 툴팁 표시 (정보 조회 기능).
    - 임계값 이상 누르고 있을 경우에만 StartDrag()를 호출하여 유닛 이동을 허용 (배치/이동 기능).

      ![Click,Drag](https://github.com/user-attachments/assets/72f9fd60-6263-4768-a636-d47d3a2b9cf8)
        
        
- **OnUnitDropped** 이벤트를 외부에 노출.
    - 유닛이 슬롯에 성공적으로 드롭될 경우, 전역 이벤트(또는 관련 컨트롤러)를 호출하여 데이터 동기화 및 배치 로직 처리를 시스템 외부로 위임.
        ![DragAndDrop](https://github.com/user-attachments/assets/887f5704-5f1b-4411-9f20-5b95156eed32)
 
- **GameObject, UI 각각 환경 대응**
    - **월드 유닛 감지 :** Physics2D.RaycastAll을 사용하여 3D 월드의 유닛 콜라이더 감지.
    - **UI 슬롯 감지 :** EventSystem.current.RaycastAll을 사용하여 UI 레이어의 드롭 영역 (UI_UnitSlot) 감지.

### 구현 이유

- 전처리로 멀티 플랫폼 환경에 대응 가능, Editor 테스트 용이
- GameObject, UI를 각각 따로 처리하여 안전

### 관련 코드

- **Battle_Slot 관련**
    - SlotCreater
        
        ```csharp
        public class SlotCreater : MonoBehaviour
        {
            [SerializeField] protected GameObject _slotPrefab;
            [SerializeField] protected Transform _slotParent;
            protected static Vector2 _offset = new Vector2(1.7f, 1.6f);
            public Vector2Int Size;    
        
            public virtual Dictionary<Vector2Int, UnitSlot> Init()
            {
                Dictionary<Vector2Int, UnitSlot> unitSlotDic = new Dictionary<Vector2Int, UnitSlot>();
        
                for (int i = 0; i < Size.y; i++)
                {
                    for (int j = 0; j < Size.x; j++)
                    {
                        UnitSlot slot = Instantiate(_slotPrefab, GetPos(j, i), Quaternion.identity, _slotParent).GetComponent<UnitSlot>();
        
                        int reversedX = Size.x - j;
        
                        Vector2Int pos = new Vector2Int(reversedX, i + 1);
                        slot.Init(reversedX, pos);
        
                        unitSlotDic.Add(pos, slot);
                    }
                }
        
                return unitSlotDic;
            }
        
            protected Vector2 GetPos(int x, int y)
            {
                Vector2 parentPos = _slotParent.position;
        
                float totalWidth = (Size.x - 1) * _offset.x;
                float totalHeight = (Size.y - 1) * _offset.y;
        
                float xOffset = -totalWidth / 2f;
                float yOffset = -totalHeight / 2f;
        
                return parentPos + new Vector2(x * _offset.x + xOffset, y * _offset.y + yOffset);
            }
        
            public virtual void DeActiveSlots()
            {
                _slotParent.gameObject.SetActive(false);
            }
        
            public virtual void ActiveSlots()
            {
                _slotParent.gameObject.SetActive(true);
            }
        }
        ```
        
    - SlotManager
        
        ```csharp
        public class UnitSlotManager : MonoBehaviour
        {
            public SlotCreater SlotCreater;
            public Dictionary<Vector2Int, UnitSlot> UnitSlotDic = new Dictionary<Vector2Int, UnitSlot>(20);
        
            public void Init()
            {
                UnitSlotDic = SlotCreater.Init();
            }
        
            public UnitSlot GetUnitSlot(UnitBase unit)
            {
                return GetUnitSlot(unit.CurrentSlot);
            }
        
            public UnitSlot GetUnitSlot(Vector2Int pos)
            {
                return UnitSlotDic.TryGetValue(pos, out UnitSlot slot) ? slot : null;
            }
        
            public void SetUnitSlot(UnitBase unit)
            {
                UnitSlotDic[unit.CurrentSlot].SetUnit(unit);
            }
        }
        ```
        
    - UnitSlot
        
        ```csharp
        public class UnitSlot : MonoBehaviour
        {
            public UnitBase Unit;
        
            private SpriteRenderer _sr;
            private int _line;
            private Vector2Int _pos;
        
            private void Awake()
            {
                _sr = GetComponent<SpriteRenderer>();        
            }
        
            public void Init(int line, Vector2Int pos)
            {
                _line = line;
                _pos = pos;
            }  
        
            public void SetUnit(UnitBase unit)
            {
                if (unit == null)
                {
                    ClearSlot();
                    return;
                }
        
                unit.Idle();
                unit.CurrentSlot = _pos;
                unit.gameObject.transform.position = transform.position;
                unit.gameObject.transform.SetParent(transform, true);
                Unit = unit;
            }
        
            public void ClearSlot()
            {
                Unit = null;
            }
        
            public Vector2Int GetPos()
            {
                return _pos;
            }
        }
        ```
        
- **UI_Slot 관련**
    - UI_UnitSlotController
        
        ```csharp
        public class UI_UnitSlotController : MonoBehaviour
        {
            [Header("Dependencies")]
            [SerializeField] UnitDragDropSystem _dragDropSystem;
            [SerializeField] UnitController _unitController;
        
            [Header("UI Elements")]
            [SerializeField] Transform _content;
            [SerializeField] GridLayoutGroup _gridLayoutGroup;
            [SerializeField] int _slotCount;
            [SerializeField] int _offset;
        
            [SerializeField] UI_UnitSlot[] _unitSlots;
            private UnitStatus[] _cachedUnitsArray = new UnitStatus[UnitController.UnitMaxCount];
            private Dictionary<string, List<int>> _unitSlotDic = new Dictionary<string, List<int>>(10);
        
            private void Start()
            {
                CreateSlots();
                UI_UnitSlot.OnUnitChanged += SetSlot;
            }
        
            private void OnDestroy()
            {
                UI_UnitSlot.OnUnitChanged -= SetSlot;
            }
        
            private void CreateSlots()
            {        
                _gridLayoutGroup.SetupGridLayoutGroup(_content, 5, 2, _offset, true);
        
                for (int i = 0; i < _slotCount; i++)
                {
                    _unitSlots[i].ClearSlot();
                    _unitSlots[i].Init(_dragDropSystem, i, this);
                }
            }
        
            public void SetSlot(UnitStatus unit, int idx)
            {
                _unitSlots[idx].SetSlot(unit);
                AddUnit(unit, idx);
            }
        
            public void ClearSlot(int idx)
            {
                UnitStatus unit = _unitSlots[idx].GetUnit();
                if (unit == null) return;
        
                _unitSlotDic[unit.Address].Remove(idx);
        
                _unitSlots[idx].ClearSlot();
        
                RemoveCahchedArray(idx);
            }
        
            public void ClearSlot(UnitStatus unit)
            {        
                ClearSlot(GetUnitSlot(unit).GetSlotIdx());
            }
        
            /// <summary>
            /// 빈 슬롯의 인덱스를 반환합니다. 없으면 -1 반환
            /// </summary>
            /// <returns></returns>
            public int GetEmptySlot()
            {
                for (int i = 0; i < _unitSlots.Length; i++)
                {
                    if (_unitSlots[i].IsEmpty())
                    {
                        return i;
                    }
                }
                return -1;
            }
        
            public void AddEmptySlotUnit(UnitStatus status)
            {
                int emptyIdx = GetEmptySlot();
        
                if (emptyIdx == -1)
                {
                    Debug.LogWarning("No empty slots available!");
                    return;
                }
        
                SetSlot(status, emptyIdx);
            }
        
            public void AddUnit(UnitStatus unit, int idx)
            {
                if (!_unitSlotDic.ContainsKey(unit.Address))
                {
                    _unitSlotDic.Add(unit.Address, new List<int>(5));
                }
        
                var slotList = _unitSlotDic[unit.Address];
        
                if (!slotList.Contains(idx))
                {
                    slotList.Add(idx);
                }
        
                AddCahchedArray(unit, idx);
            }
        
            public void RemoveUnit(UnitStatus unit, int idx)
            {
                if (_unitSlotDic.ContainsKey(unit.Address))
                {
                    _unitSlotDic[unit.Address].Remove(idx);
                }     
            }
        
            public void RemoveLastUnit(UnitStatus unit)
            {
                var slotList = _unitSlotDic[unit.Address];
                int lastIdx = slotList[slotList.Count - 1];
                ClearSlot(lastIdx);
            }
        
            /// <summary>
            /// 인 게임 슬롯에서 유닛을 제거합니다.
            /// </summary>    
            public void RemoveInGameSlot(UnitBase unit, int slotIdx, bool isSwitch = false)
            {
                if (unit == null || unit.StatusController == null)
                {
                    Debug.LogWarning("RemoveInGameSlot called with invalid unit");
                    return;
                }
        
                if (unit.CurrentSlot != Vector2Int.zero)
                {
                    UnitSlot slot = _unitController.GetUnitSlot(unit);
                    _unitController.RemoveUnit(slot); // 슬롯에 있는 유닛 삭제
                }
        
                if(!isSwitch)
                    RemoveUnit(unit.StatusController.Status, slotIdx);        
            }
        
            public void ReturnUnitToUI(UnitBase unit)
            {
                int emptySlotIdx = GetEmptySlot();
                if (emptySlotIdx == -1)
                {
                    Debug.LogWarning("No empty UI slots available!");
                    return;
                }
        
                SetSlot(unit.Status, emptySlotIdx);
            }
        
            public void AddInGameSlot(UnitStatus unit, int slotIdx, Vector2Int pos)
            {
                UnitSlot slot = _unitController.GetUnitSlot(pos);
                _unitController.AddUnit(unit, pos);
            }
        
            private UI_UnitSlot GetUnitSlot(UnitStatus unit)
            {
                if (_unitSlotDic.TryGetValue(unit.Address, out List<int> slotIdxs))
                {
                    foreach (int idx in slotIdxs)
                    {
                        if (_cachedUnitsArray[idx] == unit)
                        {
                            return _unitSlots[idx];
                        }
                    }
                }
                return null;
            }
            public int GetUnitCount(string address)
            {
                if (_unitSlotDic.TryGetValue(address, out List<int> slotIdxs))
                {
                    return slotIdxs.Count;
                }
                return 0;
            }
        
            public int GetUnitCount(UnitStatus unit)
            {
                return GetUnitCount(unit.Address);
            }
        
            private void AddCahchedArray(UnitStatus unit, int idx)
            {
                _cachedUnitsArray[idx] = unit;
            }
        
            private void RemoveCahchedArray(int idx)
            {
                _cachedUnitsArray[idx] = null;
            }
        
            public UnitStatus[] GetUnits()
            {
                return _cachedUnitsArray;
            }
        
            public UI_UnitSlot[] GetUnitSlots()
            {
                return _unitSlots;
            }
        }
        ```
        
- **DragDrop 관련**
    - Input 전처리
        
        ```csharp
            private void Update()
            {
        #if UNITY_EDITOR || UNITY_STANDALONE
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StartPress(Input.mousePosition);
                }
        
                if (_isPressing && Input.GetMouseButton(0))
                {
                    UpdatePress(Input.mousePosition);
                }
        
                if (IsDragging && _currentUnit != null && Input.GetMouseButton(0))
                {
                    DragUnit(Input.mousePosition);
                }
        
                if (Input.GetMouseButtonUp(0))
                {
                    if (_isPressing && !_dragStarted)
                    {
                        // 드래그 시작 전에 뗐다면 짧은 클릭으로 처리
                        HandleShortClick();
                    }
        
                    if (IsDragging)
                    {
                        ReleaseUnit();
                    }
        
                    EndPress();
                }
        
        #elif UNITY_ANDROID
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
        
                    if (touch.phase == TouchPhase.Began)
                    {
                        StartPress(touch.position);
                    }
        
                    if (_isPressing && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
                    {
                        UpdatePress(touch.position);
                    }
        
                    if (IsDragging && _currentUnit != null && touch.phase == TouchPhase.Moved)
                    {
                        DragUnit(touch.position);
                    }
        
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if (IsDragging)
                        {
                            ReleaseUnit();
                        }
                        else if (_isPressing)
                        {
                            HandleShortClick();
                        }
                        EndPress();
                    }
                }
        #endif
            }
        ```
        
    - 나머지 메서드
        
        ```csharp
        private void StartPress(Vector2 inputPosition)
        {
            _isPressing = true;
            _currentPressTime = 0f;
            _dragStarted = false;
            _pressStartPosition = inputPosition;
            _pressedObject = null;
            _isUI = false;
        
            Vector2 worldMouse = GetWorldMouseFromScreenPosition(inputPosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldMouse, Vector2.zero);
        
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider != null)
                    {
                        if (hits[i].collider.CompareTag("UnitTrigger") ||
                            hits[i].collider.CompareTag("Unit") ||
                            hits[i].collider.CompareTag("BattleUnit"))
                        {
                            _pressedObject = hits[i].collider.gameObject;
                            return;
                        }
                    }
                }
            }
            else
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };
        
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);
        
                foreach (var result in results)
                {
                    if (!result.gameObject.CompareTag("Slot"))
                        continue;
        
                    _isUI = true;
                    _pressedObject = result.gameObject;
                    break;
                }
            }
        }
        
        /// <summary>
        /// 누르기 업데이트 (시간 체크 및 드래그 시작 판단)
        /// </summary>
        private void UpdatePress(Vector2 currentPosition)
        {
            if (!_isPressing || _dragStarted)
                return;
        
            _currentPressTime += Time.deltaTime;
        
            if (_currentPressTime >= _dragThreshold)
            {
                _dragStarted = true;
                StartDrag();
            }
        }
        
        /// <summary>
        /// 드래그 시작
        /// </summary>
        private void StartDrag()
        {
            if (_pressedObject == null)
                return;
        
            if (!_isUI)
            {
                if (_pressedObject.CompareTag("UnitTrigger"))
                {
                    SetUnit(_pressedObject);
                }
            }
            else
            {
                _pressedObject.GetComponent<UI_UnitSlot>().OnBeginDrag();
            }
        }
        
        /// <summary>
        /// 짧은 클릭 처리 (UnitPanel 표시)
        /// </summary>
        private void HandleShortClick()
        {
            if (_pressedObject != null)
            {
                bool isEnemy = false;
        
                if (_pressedObject.CompareTag("Unit") || _pressedObject.CompareTag("UnitTrigger"))
                {
                    UnitBase unitBase = _pressedObject.GetComponentInParent<UnitBase>();
                    if (unitBase != null)
                    {
                        isEnemy = unitBase.GetAllyLayerMask().Contain(LayerMask.NameToLayer("Enemy"));
        
                        ToolTipController.UnitToolTip.Show(
                            unitBase.Status,
                            false,
                            true,
                            isEnemy
                        );
        
                        SynergyToolTipClose();
                    }
                }
                else if (_pressedObject.CompareTag("BattleUnit"))
                {
                    UnitBase unitBase = ComponentProvider.Get<UnitBase>(_pressedObject);
                    if (unitBase != null)
                    {
                        isEnemy = unitBase.GetAllyLayerMask().Contain(LayerMask.NameToLayer("Enemy"));
        
                        ToolTipController.UnitToolTip.Show(
                            unitBase.Status,
                            false,
                            false,
                            isEnemy
                        );
        
                        SynergyToolTipClose();
                    }
                }
                return;
            }
        
            ToolTipController.UnitToolTip.Close();
            SynergyToolTipClose();
        }
        
        private void SynergyToolTipClose()
        {
            //ToolTipController.SynergyToolTip.Close();
        }
        
        private void EndPress()
        {
            _isPressing = false;
            _currentPressTime = 0f;
            _dragStarted = false;
            _pressedObject = null;
        }
        
        private void DragUnit(Vector3 inputPosition)
        {
            inputPosition.z = -Camera.main.transform.position.z;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(inputPosition);
        
            _currentUnitBase.transform.position = mouseWorldPos + _offset;
        }
        
        private void ReleaseUnit()
        {
            bool isSlot;
            CheckUISlot(out isSlot);
        
            IsDragging = false;
        
            if (isSlot)
            {
                return;
            }
        
            if (_currentUnit != null)
            {
                CheckSlot();
            }
        
            Clear();
        }
        
        private void CheckUISlot(out bool isSlot)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
        
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
        
            isSlot = false;
            foreach (var result in results)
            {
                if (!result.gameObject.CompareTag("Slot"))
                    continue;
        
                if (result.gameObject.TryGetComponent<UI_UnitSlot>(out var dropHandler))
                {
                    dropHandler.OnDrop(pointerData);
        
                    isSlot = true;
                    break;
                }
            }
        }
        
        private void CheckSlot()
        {
            // 슬롯 체크
            Collider2D slotCollider = Physics2D.OverlapPoint(_currentUnitBase.transform.position, LayerMask.GetMask("Slot"));
        
            if (_currentSlotIdx != -1 && _currentUnitBase != null)
            {
                OnSlotChanged?.Invoke(slotCollider, _currentUnitBase);
            }
        
            if (slotCollider != null)
            {
                UnitSlot slot = slotCollider.GetComponent<UnitSlot>();
        
                OnUnitDropped?.Invoke(slot, _currentUnitBase);
            }
            else
            {
                if (_currentUnitBase != null)
                {
                    if (_currentUnitBase.CurrentSlot == Vector2Int.zero)
                        Destroy(_currentUnitBase.gameObject);
                    else
                        _currentUnitBase.transform.position = _pos; // 원래 위치로 되돌리기
        
                    _currentUnitBase.Idle();
                }
            }
        }
        
        private static Vector2 GetWorldMouseFromScreenPosition(Vector2 screenPosition)
        {
            Vector3 pos = screenPosition;
            pos.z = -Camera.main.transform.position.z;
            return Camera.main.ScreenToWorldPoint(pos);
        }
        
        public void SetUnit(GameObject unit)
        {
            IsDragging = true;
            _currentUnit = unit;
            _currentUnitBase = _currentUnit.GetComponentInParent<UnitBase>();
            _currentUnitBase.Drag();
            _offset = Vector2.zero;
            _pos = _currentUnitBase.transform.position;
        }
        
        public void SetUnit(GameObject unit, Action<Collider2D, UnitBase> action, int slotIdx)
        {
            OnSlotChanged = action;
        
            _currentSlotIdx = slotIdx;
            IsDragging = true;
            _currentUnit = unit;
            _currentUnitBase = _currentUnit.GetComponentInParent<UnitBase>();
            _currentUnitBase.Drag();
        
            _offset = Vector2.zero;
            _pos = _currentUnitBase.transform.position;
        }
        
        private void Clear()
        {
            _currentUnit = null;
            _currentUnitBase = null;
            OnSlotChanged = null;
            _currentSlotIdx = -1;
        }
        ```
        

---

# 13. 스와이프 (UX)

---

### 구현

**수직 스와이프 페이지 관리 시스템**

- `IDragHandler`, `IEndDragHandler`, `IBeginDragHandler`를 구현하여 유니티의 **EventSystem**을 통해 스와이프 입력을 직접 처리
- `OnEndDrag`에서 드래그 거리와 임계값을 비교하여 **사용자의 페이지 전환 의도**를 정확하게 판단
- **DOTween**을 ****활용하여 페이지 전환시 `DOAnchorPos`를 사용, `_tweenDuration`과 `_easeType`에 따라 부드러운 이동
    
    ![VerticalSwipe](https://github.com/user-attachments/assets/5f22c12b-0cfd-4192-9170-93e31fa6a58c)

**UI와 월드 오브젝트의 정밀한 좌표 동기화**

- 카메라의 FOV와 캔버스-월드 간의 거리를 이용하여 **worldPerPixel** 값을 계산
- 현재 UI 콘텐츠의 이동량에 **worldPerPixel**을 곱하여 월드 좌표계에서의 오프셋을 계산
- 각 월드 페이지의 기본 위치에 이 **worldOffsetY**를 더해 페이지의 위치를 실시간으로 업데이트
    
     ![ObjectSwipeMove](https://github.com/user-attachments/assets/1e3c0727-4e4c-400c-b81f-a1d6964f7147)
    

**입력 우선순위 및 상태 분기 처리**

- `OnDrag`에서 **eventData.delta.x**와 **eventData.delta.y**를 비교하여 드래그 방향을 **수직/수평으로 분리**하고, 수직 드래그일 때만 페이지 전환 로직을 실행
- `UnitDragDropSystem.IsDragging` 또는 `_isBattle` 상태일 경우 드래그를 **무시**하여 유닛 드래그나 전투 중에는 페이지가 움직이지 않도록 **입력 충돌을 방지**

### 구현 이유

- UI `RectTransform`의 픽셀 기반 움직임에 맞춰 월드 좌표계에 있는 `GameObject` 페이지를 따라 움직이게 해야 했음.

### 관련 코드

- SwipePager
    
    ```csharp
    public class SwipePager : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField] SlotPositionSetter _slotPositionSetter;
        [SerializeField] SwitchButtonController _switchButtonController;
        private enum DragDirection { None, Horizontal, Vertical }
        private DragDirection _dragDirection = DragDirection.None;
    
        [Header("UI Content")]
        [SerializeField] RectTransform _content;
    
        [Header("GameObject Pages")]
        [SerializeField] Transform[] _pages;
    
        [Header("Settings")]
        [SerializeField] float _swipeThreshold = 200f;
        [SerializeField] float _tweenDuration = 0.3f;
        [SerializeField] Ease _easeType = Ease.OutCubic;
        [SerializeField] int _currentPage = 0;
        [SerializeField] Vector2 _cameraOffset;
    
        [Header("Horizontal Settings")]
        private float _xLimit;
        public float XLimit
        {
            get
            {
                return _xLimit;
            }
            set
            {
                _xLimit = value;
                SetEnemyPosition();
            }
        }
    
        [SerializeField] float _xDragSensitivity;
        [SerializeField] float _xTweenDuration = .5f;
        private float _initialCameraX;
        private float _targetCameraX;
    
        private int _totalPages;
        private Vector3[] _originalPagePositions;
        private Vector2 _originalUIPosition;
    
        private float _lastDragDeltaX;
        private bool _isBattle => InGameManager.Instance.IsBattle;
        private Canvas _canvas;
        private Camera _cam;
    
        #region LifeCycle
        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            _cam = Camera.main;
            _slotPositionSetter.SetPositions();
            Init();
        }
    
        private void OnEnable()
        {
            BattleManager.OnGameStanby += () => MoveToPage(0);
        }
        
        #endregion    
    
        private void SetEnemyPosition()
        {
            _targetCameraX = _initialCameraX + XLimit;
        }
    
        private void Init()
        {
            _totalPages = _pages.Length;
    
            _originalPagePositions = new Vector3[_pages.Length];
            _originalUIPosition = _content.anchoredPosition;
    
            for (int i = 0; i < _pages.Length; i++)
            {
                _originalPagePositions[i] = _pages[i].position;
            }
    
            _initialCameraX = _cam.transform.position.x;
    
            SetAnchorPos();
            MoveToPage(_currentPage, instant: true);
        }
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragDirection = DragDirection.None;
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            if (UnitDragDropSystem.IsDragging || _isBattle)
                return;
    
            if (_dragDirection == DragDirection.None)
            {
                if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
                    _dragDirection = DragDirection.Horizontal;
                else
                    _dragDirection = DragDirection.Vertical;
            }
    
            if (_dragDirection == DragDirection.Horizontal && _currentPage == 1)
            {
                float deltaX = -eventData.delta.x * 0.01f;
                _lastDragDeltaX = deltaX;
    
                _targetCameraX = Mathf.Clamp(
                    _cam.transform.position.x + deltaX,
                    _initialCameraX,
                    _initialCameraX + XLimit
                );
    
                Vector3 camPos = _cam.transform.position;
                camPos.x = _targetCameraX;
                _cam.transform.position = camPos;
                return;
            }
    
            if (_dragDirection == DragDirection.Vertical)
            {
                if (_currentPage == 0 && 0 < eventData.delta.y)
                    return;
                if (_currentPage == _totalPages - 1 && 0 > eventData.delta.y)
                    return;
    
                _content.anchoredPosition += new Vector2(0, eventData.delta.y);
                SyncGameObjectsWithUI();
            }
        }
    
        public void OnEndDrag(PointerEventData eventData)
        {
            if (UnitDragDropSystem.IsDragging || _isBattle)
                return;
    
            if (_dragDirection == DragDirection.Horizontal && _currentPage == 1)
            {
                float flingDistance = _lastDragDeltaX * _xDragSensitivity;
    
                float targetX = _cam.transform.position.x + flingDistance;
    
                _targetCameraX = Mathf.Clamp(targetX, _initialCameraX, _initialCameraX + XLimit);
    
                _cam.transform.DOMoveX(_targetCameraX, _xTweenDuration)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(true);
    
                _lastDragDeltaX = 0;
            }
            else if (_dragDirection == DragDirection.Vertical)
            {
                if (_currentPage == 0 && 0 < eventData.delta.y)
                    return;
                if (_currentPage == _totalPages - 1 && 0 > eventData.delta.y)
                    return;
    
                float diff = eventData.position.y - eventData.pressPosition.y;
                if (Mathf.Abs(diff) > _swipeThreshold)
                {
                    if (diff < 0 && _currentPage < _totalPages - 1)
                        _currentPage++;
                    else if (diff > 0 && _currentPage > 0)
                        _currentPage--;
                }
    
                MoveToPage(_currentPage);
            }
    
            _dragDirection = DragDirection.None;
        }
    
        public void MoveToPage(int pageIndex, bool instant = false)
        {
            _currentPage = pageIndex;
    
            float height = ((RectTransform)_canvas.transform).rect.height;
            Vector2 targetPos = new Vector2(0, -pageIndex * height);
    
            if (instant)
            {
                _content.anchoredPosition = targetPos;
                SyncGameObjectsWithUI();
            }
            else
            {
                _content.DOAnchorPos(targetPos, _tweenDuration)
                    .SetEase(_easeType)
                    .SetUpdate(true)
                    .OnUpdate(() => SyncGameObjectsWithUI());
            }
        }
    
        private void SyncGameObjectsWithUI()
        {
            Vector2 uiOffset = _content.anchoredPosition - _originalUIPosition;
    
            RectTransform canvasRect = (RectTransform)_canvas.transform;
    
            float distance = Mathf.Abs(_cam.transform.position.z - _pages[0].position.z);
            float worldCanvasHeight = 2f * distance * Mathf.Tan(_cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
    
            float worldPerPixel = worldCanvasHeight / canvasRect.rect.height;
            float worldOffsetY = uiOffset.y * worldPerPixel;
    
            for (int i = 0; i < _pages.Length; i++)
            {
                Vector3 basePos;
    
                if (i == 1)
                {
                    basePos = _cam.transform.position + (Vector3)_cameraOffset;
                }
                else
                {
                    basePos = _originalPagePositions[i];
                }
    
                _pages[i].position = basePos + new Vector3(0, worldOffsetY, 0);
            }
        }
    
        private void SetAnchorPos()
        {
            for (int i = 0; i < _content.childCount; i++)
            {
                RectTransform rt = (RectTransform)_canvas.transform;
                RectTransform panel = _content.GetChild(i).GetComponent<RectTransform>();
                panel.anchoredPosition = new Vector2(0, i * rt.rect.height);
            }
        }
    
        public void MoveToEnemy()
        {
            float targetX = _initialCameraX + XLimit;
            _cam.transform.DOMoveX(targetX, 0.3f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);
        }
    
        public void MoveToBattle()
        {
            _cam.transform.DOMoveX(_initialCameraX, 0.3f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);
        }
    }
    ```
    

---

# 14. **실행 환경 동적 레이아웃**을 구현 (UX)

---

### 구현

- **모바일 Safe Area 및 해상도 대응**
    - `Screen.safeArea`를 사용하여 **노치 디자인이나 펀치 홀 카메라** 등으로 인해 UI가 가려지는 영역을 피하고, 실제 콘텐츠가 표시되는 안전 영역 내에 슬롯을 배치
    - `_battleSlotRatio`와 `_unitSlotRatio` 같은 비율 상수를 활용하여 **화면 높이(`safe.height`) 대비 상대적인 위치**(`centerYInSafe`, `centerYNextSafe`)를 계산 (이는 기기 해상도와 관계없이 슬롯 간의 시각적 비율을 일정하게 유지)
- **스크린 좌표를 월드 좌표로 변환**
    - **`Camera.main.ScreenToWorldPoint()`** 메서드를 사용하여 계산된 스크린 좌표 (`planScreen`, `battleScreen`, `enemyScreen`)를 **월드 좌표로 정확하게 변환**
- **다중 페이지 환경에 맞춘 초기 위치 분배**
    - 수직 페이지 전환 시스템에 맞추어 슬롯들을 **다른 페이지에 분리하여 초기 배치**
        - **_planSlot** : `Screen.height * 0` 에 배치.
        - **_battleSlot** / **_enemySlot**: `Screen.height * 1` 에 배치.
    - **_enemySlot**은 **_battleSlot**에서 월드 거리 `_distance`만큼 수평으로 이격시켜 배치하며, **카메라 이동 한계값**을 `_swipePager.XLimit`에 전달하여 **수평 이동 시스템과 연동**
        
        ![SwipeSlotSetting](https://github.com/user-attachments/assets/26ac7f18-172f-43d8-a559-b22af801335e)


### 구현 이유

- UI 요소의 배치 비율을 기반으로, 실제 상호작용이 일어나는 **월드 오브젝트를 정확한 위치에** 두어야했기 때문

### 관련 코드

- SlotPositionSetter
    
    ```csharp
    public class SlotPositionSetter : MonoBehaviour
    {
        [SerializeField] SwipePager _swipePager;
    
        [Header("Center")]
        [SerializeField] Transform _center;
        [Space]
    
        [Header("Slots")]
        [SerializeField] GameObject _battleSlot;
        [SerializeField] GameObject _planSlot;
        [SerializeField] GameObject _enemySlot;
        [Space]
    
        [SerializeField] float _zOffset = 10f;
        [SerializeField] float _distance = 3;
        [SerializeField] float _battleSlotRatio = 0.65f;
        [SerializeField] float _unitSlotRatio = 0.75f;
    
        [ContextMenu("Setting_Position")]
        public void SetPositions()
        {
            Rect safe = Screen.safeArea;
    
            float centerX = safe.x + safe.width * 0.5f;
            float centerYInSafe = safe.y + safe.height * _unitSlotRatio;
            float centerYNextSafe = safe.y + safe.height * _battleSlotRatio;
    
            float screenZ = Mathf.Abs(Camera.main.transform.position.z + _zOffset);
    
            Vector3 planScreen = new Vector3(centerX, centerYInSafe + Screen.height * 0, screenZ);
            Vector3 battleScreen = new Vector3(centerX, centerYNextSafe + Screen.height * 1, screenZ);
            Vector3 enemyScreen = new Vector3(centerX + safe.width * _distance, centerYNextSafe + Screen.height * 1, screenZ);
    
            Vector3 planWorld = Camera.main.ScreenToWorldPoint(planScreen);
            Vector3 battleWorld = Camera.main.ScreenToWorldPoint(battleScreen);
            Vector3 enemyWorld = battleWorld + Vector3.right * _distance;
            Vector3 centerWorld = battleWorld + Vector3.right * (_distance / 2);
    
            _planSlot.transform.position = planWorld;
            _battleSlot.transform.position = battleWorld;
            _enemySlot.transform.position = enemyWorld;
            _center.position = centerWorld;
    
            _swipePager.XLimit = _distance;
    
            SynergyEffectManager.Instance.SetCenter(_center.position);
        }
    }
    ```
    

---

# 15. DoTween을 이용한 UI

---

### 전투 시작
![BattleStartAnimation](https://github.com/user-attachments/assets/f23b9aef-bf31-4175-9c5a-6e17f46a6e4b)


### 전투 끝
![BattleEndAnimation](https://github.com/user-attachments/assets/5f98f718-cf7f-4010-beaf-3561681ce183)

### 전투 UI
![BoxAnimation](https://github.com/user-attachments/assets/34fc10bd-bccd-4792-8756-88206e836034)

### 전투 보상
![RewardAnimation](https://github.com/user-attachments/assets/c2ab4d14-458f-4a11-9b2e-bab6c8e5a862)

---

# 문제 및 해결

# 1. Addressable 순차적 로드 시 시간 문제

---

### 문제 상황

- Addressable Label 로드 시 시간이 오래 걸리는 문제

### 해결 방법

- 하나하나의 에셋로드를 UniTask.WhenAll로 병렬적으로 처리
    - LoadLabel코드
        
        ```csharp
        public async UniTask LoadLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) return;
        
            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            var locations = await locationsHandle.Task;
        
            List<UniTask> tasks = new List<UniTask>(100);
        
            foreach (var location in locations)
            {
                tasks.Add(LoadAndCache(location));
            }
        
            await UniTask.WhenAll(tasks);
        
            Addressables.Release(locationsHandle);
            Debug.Log($"[로드 성공] : {label}");
        }
        ```
        

### 결과

- 전
    <img width="419" height="48" alt="스크린샷 2025-09-10 212341" src="https://github.com/user-attachments/assets/617ed307-f49e-4ff1-924c-cb11c1d02103" />

- 후
    <img width="404" height="41" alt="스크린샷 2025-09-10 212601" src="https://github.com/user-attachments/assets/f7aefd83-fecd-4e2d-9d75-c20195cc2439" />
    

Load시간을 대폭 낮출 수 있었음.

---

# 2. CBT 단계 : 기획 딜레이로 인한 개발 지연

---

### 문제 상황

- 프로젝트에서 기획 속도보다 개발 속도가 더 빠른 상황이 발생하여 개발 중 텀(지연)이 생김

### 해결 방법

- 지연된 시간을 최대한 활용하기 위해 저희 개발팀은 다음과 같은 작업들을 진행
    
    ### 작업 들
    
    - 폴리싱 작업
        
        <img width="485" height="343" alt="image (13)" src="https://github.com/user-attachments/assets/7d367085-f3ad-4397-a16f-34687e4242b2" />
        
    - 프리팹 세팅 툴을 제작
        
        ![PrefabSettingTool](https://github.com/user-attachments/assets/9d94c7d7-0719-4f86-967d-1b7fa8a43c9f)

    - 네이밍 규칙을 통한 데이터 자동 주입

        <img width="613" height="433" alt="image (14)" src="https://github.com/user-attachments/assets/b40fec58-0849-429d-9d16-99af361ea71b" />
        
    - 파싱 코드
        
        ```csharp
        AnimationType attackAnimation   = Enum.TryParse(row[8], out AnimationType attackAnim) ? attackAnim : AnimationType.Magic_Attack;
        AnimationType skillAnimation    = Enum.TryParse(row[9], out AnimationType skillAnim) ? skillAnim : AnimationType.Magic_Attack;
        
        unitData.AnimatiorData.AttackAnimationType  = attackAnimation;
        unitData.AnimatiorData.SkillAnimationType   = skillAnimation;
        
        unitData.AddressableAddress = $"{synergyName}{lastDigit}";
        
        unitData.Icon       = Manager.Resources.SpriteLoad($"{unitData.AddressableAddress}_Icon");
        unitData.Skill.Icon = Manager.Resources.SpriteLoad($"{unitData.AddressableAddress}_Skillicon");
        ```
        

### 결과

- 추후 진행할 작업들을 미리 끝내둠으로써 시간을 절약

---

# 3. 재활용한 애니메이션의 이벤트 맵핑 문제

---

### 문제 상황

- Animation을 사용 시, Attack 애니메이션에는 Attack, Skill 애니메이션에는 Skill이 실행되도록 함
- 하지만 Attack 애니메이션을 Skill로 쓰는 유닛이 추가됨

### 해결 방법

- 현재 유닛의 상태에 따라 분기를 나눠 Attack, Skill을 실행하는 함수를 애니메이션 이벤트로 맵핑 통일
- **애니메이션 이벤트 코드**
    
    ```csharp
    private void OnActionEvent()
    {
        if (StateMachine._currentState == AttackState)
        {
            Owner.Attack();
        }
        else if (StateMachine._currentState == SkillState)
        {
            Owner.UseSkill();
        }
    }
    ```
    

### 결과

- 기획에서 해당 애니메이션을 다른 기능으로 사용하려고 하여도 분기를 추가하여 애니메이션의 재사용을 높일 수 있음
