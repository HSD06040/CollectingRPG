using System.Collections;
using UnityEngine;

public class BlackHoleScaler : MonoBehaviour
{
    private CircleCollider2D _collider;
    
    private float _startScale = 0.3f;   
    private float _targetScale = 1f;    
    private float _growDuration = 0.3f; 

    private float _baseRadius = 2f;    


    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();

        transform.localScale = Vector3.one * _startScale;
        _collider.radius = _baseRadius * _startScale;

        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        float elapsed = 0f;

        while (elapsed < _growDuration)
        {
            elapsed += Time.deltaTime;
            float timeRatio = elapsed / _growDuration;
          
            float scale = Mathf.Lerp(_startScale, _targetScale, timeRatio);
            transform.localScale = Vector3.one * scale;

            // Collider 반경 = 스케일
            _collider.radius = _baseRadius * scale;

            yield return null;
        }

        transform.localScale = Vector3.one * _targetScale;
        _collider.radius = _baseRadius * _targetScale;
    }
}