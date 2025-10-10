using TMPro;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _goldText;

    private void OnEnable()
    {
        InGameManager.Instance.Energy.AddEvent(GoldTextUpdate);
        GoldTextUpdate(InGameManager.Instance.Energy.Value);
    }

    private void OnDisable()
    {
        InGameManager.Instance?.Energy.RemoveEvent(GoldTextUpdate);
    }

    private void GoldTextUpdate(int amount)
    {
        _goldText.text = amount.ToString();
    }
}
