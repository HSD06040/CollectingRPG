using Cysharp.Threading.Tasks;
using DG.Tweening;
using Map;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace map
{
    public class StageShopPanel : MonoBehaviour
    {
        public static StageShopPanel Instance;

        public GameObject shopPanelUI; // Inspector에서 실제 패널 연결
        public Button _backButton;

        [Header("ItemListPanel")]
        [SerializeField] ItemListPanel_Agument _itemListPanel_Augment;
        [SerializeField] ItemListPanel_MagicStone _itemListPanel_MagicStone;

        [Header("AnimationSetting")]
        [SerializeField] Ease _downEase;
        [SerializeField] Ease _upEase;
        [SerializeField] float _duration = 0.5f;
        [SerializeField] Vector2 _start;
        [SerializeField] Vector2 _end;
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

        private void Awake()
        {
            Instance = this;
            Debug.Log("StageShopPanel Instance created!");
            // 여기서는 gameObject.SetActive(false) 하지 않음!

            _start = Vector3.zero;
            _end = _start + new Vector2(0, Screen.height);

            ((RectTransform)shopPanelUI.transform).anchoredPosition = _end;
        }

        private void Start()
        {
            if (shopPanelUI != null)
            {
                shopPanelUI.SetActive(false); // 패널만 비활성화
            }

            if (_backButton != null)
            {
                _backButton.onClick.AddListener(CloseShop);
            }
        }

        [ContextMenu("OpenShop")]
        public void OpenShop()
        {
            Debug.Log("OpenShop called!");
            if (shopPanelUI != null)
            {
                shopPanelUI.SetActive(true);
                MoveDown().Forget();
                Debug.Log("Shop panel activated!");
                SetupItems();
            }
            else
            {
                Debug.LogError("shopPanelUI is not assigned!");
            }
        }

        public void CloseShop()
        {
            if (shopPanelUI != null)
            {
                MoveUp().Forget();
            }

            if (MapPlayerTracker.Instance != null)
            {
                MapPlayerTracker.OnEventEnded?.Invoke();
            }
        }

        private void OnDestroy()
        {
            if (_backButton != null)
            {
                _backButton.onClick.RemoveListener(CloseShop);
            }
        }

        private void SetupItems()
        {
            _itemListPanel_Augment.SettingItmes();
            _itemListPanel_MagicStone.SettingItmes();
        }

        private async UniTask MoveUp()
        {
            await ((RectTransform)shopPanelUI.transform).DOAnchorPos(_end, _duration).SetEase(_upEase).SetUpdate(true).AsyncWaitForCompletion();
            shopPanelUI.SetActive(false);

            if (MapPlayerTracker.Instance != null)
            {
                MapPlayerTracker.OnEventEnded?.Invoke();
            }
        }

        private async UniTask MoveDown()
        {
            await ((RectTransform)shopPanelUI.transform).DOAnchorPos(_start, _duration).SetEase(_downEase).SetUpdate(true).AsyncWaitForCompletion();
        }
    }
}