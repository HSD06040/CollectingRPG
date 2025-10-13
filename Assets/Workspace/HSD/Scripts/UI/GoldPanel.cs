using TMPro;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _goldText;
    [SerializeField] bool _isSilver = false;

    private void OnEnable()
    {
        if(_isSilver)
        {
            InGameManager.Instance.Silver.AddEvent(GoldTextUpdate);
            GoldTextUpdate(InGameManager.Instance.Silver.Value);
        }
        else
        {
            InGameManager.Instance.Energy.AddEvent(GoldTextUpdate);
            GoldTextUpdate(InGameManager.Instance.Energy.Value);
        }        
    }

    private void OnDisable()
    {
        if (_isSilver)
        {
            InGameManager.Instance?.Silver.RemoveEvent(GoldTextUpdate);
        }
        else
        {
            InGameManager.Instance?.Energy.RemoveEvent(GoldTextUpdate);
        }
    }

    private void GoldTextUpdate(int amount)
    {
        _goldText.text = amount.ToString();
    }
}
