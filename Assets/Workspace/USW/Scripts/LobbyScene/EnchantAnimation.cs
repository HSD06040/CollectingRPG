using UnityEngine;
using UnityEngine.UI;

public class EnchantAnimation : MonoBehaviour
{
    [Header("버튼 설정")]
    [SerializeField] private Button characterButton;
    [SerializeField] private Button magicStoneButton;

    [Header("패널 설정")]
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject magicStonePanel;

    [Header("애니메이션 설정")]
    [SerializeField] private Animator animator;
    [SerializeField] private string characterClipName = "EnchantButton_HeroClick";
    [SerializeField] private string magicStoneClipName = "EnchantButton_MagicClick";

    private void Start()
    {
        if (characterButton != null)
            characterButton.onClick.AddListener(OnCharacterButtonClick);
        
        if (magicStoneButton != null)
            magicStoneButton.onClick.AddListener(OnMagicStoneButtonClick);
        
        if (characterPanel != null)
            characterPanel.SetActive(true);
        if (magicStonePanel != null)
            magicStonePanel.SetActive(false);
    }

    private void OnCharacterButtonClick()
    {
        if (animator != null)
        {
            animator.Play(characterClipName);
        }
        
        SwitchPanel(characterPanel, magicStonePanel);
    }

    private void OnMagicStoneButtonClick()
    {
        if (animator != null)
        {
            animator.Play(magicStoneClipName);
        }
        
        SwitchPanel(magicStonePanel, characterPanel);
    }

    private void SwitchPanel(GameObject panelToShow, GameObject panelToHide)
    {
        if (panelToShow != null)
            panelToShow.SetActive(true);
        
        if (panelToHide != null)
            panelToHide.SetActive(false);
    }

    private void OnDestroy()
    {
        if (characterButton != null)
            characterButton.onClick.RemoveListener(OnCharacterButtonClick);
        
        if (magicStoneButton != null)
            magicStoneButton.onClick.RemoveListener(OnMagicStoneButtonClick);
    }
}
