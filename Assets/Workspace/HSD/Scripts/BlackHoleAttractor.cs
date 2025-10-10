using UnityEngine;

public class BlackHoleAttractor : MonoBehaviour
{
    [Header("블랙홀 설정")]
    public int targetLayer = 6;        // Enemy 레이어의 ID
    public float attractionSpeed = 5f; // 빨아들이는 속도

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            Vector2 currentPosition = other.transform.position;
            Vector2 blackHoleCenter = this.transform.position;
            float step = attractionSpeed * Time.deltaTime;

            // 서서히 이동
            other.transform.position = Vector2.MoveTowards(
                currentPosition,
                blackHoleCenter,
                step
            );
        }
    }
}