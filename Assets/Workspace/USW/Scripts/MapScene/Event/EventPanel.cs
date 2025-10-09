using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class EventPanel : MonoBehaviour
    {
        public static EventPanel Instance;
        public EventRewardChanceData[] EventRewardChances;        

        [Header("UI References")] 
        public GameObject _eventPanelUI;
        public TMP_Text _titleText;
        public TMP_Text _descriptionText;
        public CanvasGroup _eventPanelGroup;
        public CanvasGroup _descriptionCanvasGroup;

        [Header("Choice Buttons")] 
        public Button _yesButton;
        public Button _nvmButton;

        [Header("Fade Settings")] 
        public float _fadeDuration = 0.5f;

        private EventData _currentEvent;

#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] EventData _testEventData;
        [Button]
        private void TestShow()
        {
            ShowEvent(_testEventData);
        }
#endif

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (_eventPanelUI != null)
            {
                _eventPanelUI.SetActive(false);
            }

            if (_yesButton != null)
            {
                _yesButton.onClick.AddListener(() => OnChoiceSelected(true));
            }

            if (_nvmButton != null)
            {
                _nvmButton.onClick.AddListener(() => OnChoiceSelected(false));
            }
        }

        /// <summary>
        /// 이벤트 표시
        /// </summary>
        public void ShowEvent(EventData eventData)
        {
            if (eventData == null)
            {
                return;
            }
            _eventPanelUI.SetActive(true);
            _currentEvent = eventData;
            _eventPanelGroup.FadeIn(_fadeDuration).Forget();

            _titleText.text = eventData._eventTitle;
            _descriptionText.text = eventData._eventDescription;
        }

        /// <summary>
        /// Yes 또는 Nvm 선택
        /// </summary>
        private void OnChoiceSelected(bool acceptChallenge)
        {
            _descriptionCanvasGroup.interactable = false;

            ShowResultWithFade(acceptChallenge).Forget();
        }

        /// <summary>
        /// Fade 연출과 함께 결과 표시
        /// </summary>
        private async UniTask ShowResultWithFade(bool acceptChallenge)
        {
            EventOutcome outcome = acceptChallenge ? EventOutcome.Success : EventOutcome.Declined;

            string nextText = outcome switch
            {
                EventOutcome.Success => _currentEvent._successText,
                EventOutcome.Declined => _currentEvent._declinedText,
                _ => ""
            };

            await _descriptionText.DOFade(0f, _fadeDuration).AsyncWaitForCompletion();

            _descriptionText.text = nextText;

            await _descriptionText.DOFade(1f, _fadeDuration).AsyncWaitForCompletion();

            await UniTask.WaitForSeconds(_fadeDuration, true);

            if (outcome == EventOutcome.Success)
                ApplyReward();

            await UniTask.WaitForSeconds(1, true);
            await _eventPanelGroup.FadeOut(_fadeDuration);

            CloseEvent();
        }

        /// <summary>
        /// Energy 소비
        /// </summary>
        private void ConsumeEnergy(int amount)
        {
            // TODO: 실제 에너지 시스템 연결
        }

        /// <summary>
        /// 성공 시 보상 지급
        /// </summary>
        private void ApplyReward()
        {
            List<StageInGameRewardType> stageRewards = new List<StageInGameRewardType>();

            foreach (var rewardChance in EventRewardChances)
            {
                if (Random.Range(0f, 100f) < rewardChance.Chance)
                {
                    stageRewards.Add(rewardChance.stageInGameRewardType);
                }
            }

            if (stageRewards.Count > 0)
            {
                UIManager.Instance.Reward_UI.Show(stageRewards.ToArray());
            }
            else
            {
                UIManager.Instance.MessagePopup.Show("아무런 보상도 획득하지 못했습니다...");
            }
        }

        /// <summary>
        /// 실패 시 추가 패널티 적용
        /// </summary>
        private void ApplyPenalty()
        {
            // TODO: 패널티 시스템 연결
        }

        public void CloseEvent()
        {
            _eventPanelUI.SetActive(false);

            if (MapPlayerTracker.Instance != null)
            {
                MapPlayerTracker.OnEventEnded?.Invoke();
            }
        }

        private void OnDestroy()
        {
            if (_yesButton != null)
                _yesButton.onClick.RemoveAllListeners();

            if (_nvmButton != null)
                _nvmButton.onClick.RemoveAllListeners();
        }
    }
}