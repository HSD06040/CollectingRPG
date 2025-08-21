using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] TMP_Text _damageText;
    [SerializeField] float _startTime;
    [SerializeField] float _endTime;
    [SerializeField] Vector2 _startPosOffset;
    [SerializeField] Vector2 _startOffset;
    [SerializeField] Vector2 _endOffset;
    private Vector2 _startPos;
    private Vector2 _endPos;

    public void Init(float damage, bool isCrit)
    {
        _damageText.color = isCrit ? Color.yellow : Color.white;

        _damageText.text = damage.ToString("N0");
        transform.position = GetRandomStartPos();
        PopUp().Forget();
    }

    private async UniTask PopUp()
    {
        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();

        float elapsedTime = 0f;
        _startPos = transform.position;
        _endPos = _startPos + _startOffset;

        while (elapsedTime < _startTime)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector2.Lerp(_startPos, _endPos, elapsedTime / _startTime);

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        elapsedTime = 0f;
        _startPos = transform.position;
        _endPos = _startPos + _endOffset;

        while (elapsedTime < _endTime)
        {
            elapsedTime += Time.deltaTime;
            
            Color color = _damageText.color;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / _endTime);
            _damageText.color = color;

            transform.position = Vector2.Lerp(_startPos, _endPos, elapsedTime / _endTime);

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        Destroy(gameObject);
    }

    private Vector2 GetRandomStartPos()
    {
        return (Vector2)transform.position + new Vector2(Random.Range(-.5f, .5f), Random.Range(0.3f, 1f)) + _startPosOffset;
    }
}
