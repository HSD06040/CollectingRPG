using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveTextManager : MonoBehaviour
{
    [Header("텍스트 설정")]
    public TextMeshProUGUI textDisplay;
    
    [Header("대사 목록")]
    [SerializeField, TextArea(2, 5)] 
    private string[] dialogues = {
        "게임을 불러오는 중입니다...",
        "데이터를 확인하고 있습니다...",
        "리소스를 로딩하고 있습니다...",
        "거의 완료되었습니다..."
    };
    
    [Header("웨이브 효과 설정")]
    [SerializeField] private float waveSpeed = 2f;
    [SerializeField] private float waveHeight = 15f;
    [SerializeField] private float waveLength = 0.5f; // 글자 간 웨이브 간격
    [SerializeField] private AnimationCurve wavePattern = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("대사 출력 설정")]
    [SerializeField] private float dialogueInterval = 2f;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.3f;
    
    [Header("웨이브 방향")]
    [SerializeField] private WaveDirection waveDirection = WaveDirection.LeftToRight;
    
    public enum WaveDirection
    {
        LeftToRight,
        RightToLeft,
        CenterOut,
        OutToCenter
    }
    
    private Sequence dialogueSequence;
    private Tween waveTween;
    private Vector3[] originalVertices;
    private int currentDialogueIndex = 0;
    
    void Start()
    {
        if (textDisplay == null)
            textDisplay = GetComponent<TextMeshProUGUI>();
            
        DOTween.Init();
        StartDialogueSequence();
    }
    
    public void StartDialogueSequence()
    {
        if (dialogueSequence != null)
            dialogueSequence.Kill();
            
        dialogueSequence = DOTween.Sequence();
        currentDialogueIndex = 0;
        
        for (int i = 0; i < dialogues.Length; i++)
        {
            int index = i;
            
            dialogueSequence.AppendCallback(() => {
                currentDialogueIndex = index;
                ShowDialogue(dialogues[index]);
            });
            
            dialogueSequence.AppendInterval(dialogueInterval);
        }
        
        dialogueSequence.AppendCallback(() => {
            ShowDialogue("로딩 완료!", true);
        });
    }
    
    private void ShowDialogue(string text, bool isComplete = false)
    {
        StopWaveEffect();
        
        // 페이드 아웃
        textDisplay.DOFade(0f, fadeOutDuration).OnComplete(() => {
            textDisplay.text = text;
            
            // 페이드 인
            textDisplay.DOFade(1f, fadeInDuration).OnComplete(() => {
                if (isComplete)
                {
                    StartCompleteWaveEffect();
                }
                else
                {
                    StartSequentialWaveEffect();
                }
            });
        });
    }
    
    public void StartSequentialWaveEffect()
    {
        StopWaveEffect();
        
        waveTween = DOTween.To(() => 0f, x => UpdateSequentialWave(x), 360f, waveSpeed)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }
    
    private void UpdateSequentialWave(float time)
    {
        textDisplay.ForceMeshUpdate();
        var textInfo = textDisplay.textInfo;
        
        // 원본 버텍스 정보 저장 (첫 번째 호출 시)
        if (originalVertices == null || originalVertices.Length != textInfo.meshInfo[0].vertices.Length)
        {
            originalVertices = new Vector3[textInfo.meshInfo[0].vertices.Length];
            System.Array.Copy(textInfo.meshInfo[0].vertices, originalVertices, originalVertices.Length);
        }
        
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            
            if (!charInfo.isVisible) continue;
            
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            
            // 글자별 웨이브 오프셋 계산
            float charOffset = GetCharacterOffset(i, textInfo.characterCount);
            float waveTime = time + charOffset;
            
            // 웨이브 높이 계산
            float waveValue = Mathf.Sin(waveTime * Mathf.Deg2Rad) * waveHeight;
            waveValue *= wavePattern.Evaluate((waveTime % 360f) / 360f);
            
            // 4개 버텍스에 웨이브 적용
            for (int j = 0; j < 4; j++)
            {
                int vertIndex = charInfo.vertexIndex + j;
                if (vertIndex < originalVertices.Length)
                {
                    verts[vertIndex] = originalVertices[vertIndex] + new Vector3(0, waveValue, 0);
                }
            }
        }
        
        // 메쉬 업데이트
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textDisplay.UpdateGeometry(meshInfo.mesh, i);
        }
    }
    
    private float GetCharacterOffset(int charIndex, int totalChars)
    {
        float normalizedIndex = (float)charIndex / Mathf.Max(1, totalChars - 1);
        
        switch (waveDirection)
        {
            case WaveDirection.LeftToRight:
                return normalizedIndex * 360f * waveLength;
                
            case WaveDirection.RightToLeft:
                return (1f - normalizedIndex) * 360f * waveLength;
                
            case WaveDirection.CenterOut:
                float distanceFromCenter = Mathf.Abs(normalizedIndex - 0.5f) * 2f;
                return distanceFromCenter * 360f * waveLength;
                
            case WaveDirection.OutToCenter:
                float distanceToCenter = 1f - Mathf.Abs(normalizedIndex - 0.5f) * 2f;
                return distanceToCenter * 360f * waveLength;
                
            default:
                return normalizedIndex * 360f * waveLength;
        }
    }
    
    public void StartCompleteWaveEffect()
    {
        StopWaveEffect();
        
        Sequence completeSequence = DOTween.Sequence();
        
        // 완료 시 특별한 효과 - 모든 글자가 동시에 펀치
        completeSequence.Append(textDisplay.transform.DOPunchScale(Vector3.one * 0.3f, 0.8f, 8));
        
        // 이후 더 빠른 순차 웨이브
        completeSequence.AppendCallback(() => {
            waveTween = DOTween.To(() => 0f, x => UpdateSequentialWave(x), 360f, waveSpeed * 1.5f)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);
        });
    }
    
    public void StopWaveEffect()
    {
        if (waveTween != null)
        {
            waveTween.Kill();
            waveTween = null;
        }
        
        // 텍스트를 원래 상태로 복원
        if (originalVertices != null)
        {
            textDisplay.ForceMeshUpdate();
            var textInfo = textDisplay.textInfo;
            
            if (textInfo.meshInfo.Length > 0)
            {
                var meshInfo = textInfo.meshInfo[0];
                if (originalVertices.Length == meshInfo.vertices.Length)
                {
                    System.Array.Copy(originalVertices, meshInfo.vertices, originalVertices.Length);
                    meshInfo.mesh.vertices = meshInfo.vertices;
                    textDisplay.UpdateGeometry(meshInfo.mesh, 0);
                }
            }
        }
    }
    
    // 웨이브 방향 변경
    public void ChangeWaveDirection(WaveDirection newDirection)
    {
        waveDirection = newDirection;
    }
    
    // 에디터에서 테스트용
    [ContextMenu("다음 대사")]
    public void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Length - 1)
        {
            ShowDialogue(dialogues[currentDialogueIndex + 1]);
        }
    }
    
    [ContextMenu("대사 초기화")]
    public void ResetDialogue()
    {
        StopWaveEffect();
        if (dialogueSequence != null)
            dialogueSequence.Kill();
        originalVertices = null;
        StartDialogueSequence();
    }
    
    [ContextMenu("즉시 완료")]
    public void CompleteLoading()
    {
        if (dialogueSequence != null)
            dialogueSequence.Kill();
        StopWaveEffect();
        ShowDialogue("로딩 완료!", true);
    }
    
    [ContextMenu("웨이브 테스트")]
    public void TestWave()
    {
        StopWaveEffect();
        textDisplay.text = "웨이브 테스트!";
        originalVertices = null; // 원본 버텍스 초기화
        StartSequentialWaveEffect();
    }
    
    void OnDestroy()
    {
        if (dialogueSequence != null)
            dialogueSequence.Kill();
        StopWaveEffect();
    }
}