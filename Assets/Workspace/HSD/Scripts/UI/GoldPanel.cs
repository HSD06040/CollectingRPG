using TMPro;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _goldText;

    private void OnEnable()
    {
        InGameManager.Instance.Silver.AddEvent(GoldTextUpdate);
        GoldTextUpdate(InGameManager.Instance.Silver.Value);
    }

    private void OnDisable()
    {
        InGameManager.Instance?.Silver.RemoveEvent(GoldTextUpdate);
    }

    private void GoldTextUpdate(int amount)
    {
        _goldText.text = amount.ToString();
    }
}
