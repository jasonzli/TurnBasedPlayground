using Code.ViewModels;
using UnityEngine;

namespace Code.ViewScripts
{
    [RequireComponent(typeof(Animator))]
    public class BattleActorUIView : ViewBase
    {
        private enum PanelUIState
        {
            STANDARD,
            GUARDING
        }
        private PanelUIState _panelUIState;
        private string Name { get; set; }
        private int CurrentHP { get; set; }
        private int MaxHP { get; set; }
        private bool IsGuarding { get; set; }
        private bool IsPlayerOne { get; set; }

        [SerializeField] private RectTransform _TopContainer;
        [SerializeField] private RectTransform _MiddleContainer;
        [SerializeField] private RectTransform _LowerContainer;
        [SerializeField] private RectTransform _GuardContainer;
        [SerializeField] private RectTransform _P1TokenContainer;
        [SerializeField] private RectTransform _P2TokenContainer;
        [SerializeField] private RectTransform _HealthContainer;
        [SerializeField] private HealthBarUpdater _healthBarUpdater;
        
    
        //values to actually update
        [SerializeField] private TMPro.TextMeshProUGUI _nameText;
        [SerializeField] private TMPro.TextMeshProUGUI _healthText;

        public void Initialize(PlayerPanelViewModel viewModelContext)
        {
            Name = viewModelContext.Name;
            _nameText.text = Name.ToUpper();
            CurrentHP = viewModelContext.CurrentHP;
            MaxHP = viewModelContext.MaxHP;
            IsGuarding = viewModelContext.IsGuarding;
            IsPlayerOne = viewModelContext.IsP1Side;

            SetupToken();
            DeterminePanelUIState(IsGuarding);
            SetUpUIPanelBasedOnStatus();
        
            UpdateCurrentHP(CurrentHP);
            viewModelContext.CurrentHP.PropertyChanged += UpdateCurrentHP;
            viewModelContext.IsGuarding.PropertyChanged += UpdatePlayerUIStatus;

            _animator.ResetTrigger("Show");
            _animator.ResetTrigger("Hide");
            Show();
        }

        private void SetupToken()
        {
            if (IsPlayerOne)
            {
                _P1TokenContainer.gameObject.SetActive(true);
            }
            else
            {
                _P2TokenContainer.gameObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
        }

        private void DeterminePanelUIState(bool isGuarding)
        {
            _panelUIState = isGuarding ? PanelUIState.GUARDING : PanelUIState.STANDARD;
        }

        public void UpdatePlayerUIStatus(bool isGuarding)
        {
            IsGuarding = isGuarding;
            DeterminePanelUIState(IsGuarding);
            SetUpUIPanelBasedOnStatus();
        }

        private void SetUpUIPanelBasedOnStatus()
        {
            switch (_panelUIState)
            {
                //Three Panel View
                case(PanelUIState.GUARDING):
                    _TopContainer.gameObject.SetActive(true);
                    _MiddleContainer.gameObject.SetActive(true);
                    _HealthContainer.SetParent(_MiddleContainer);
                    _LowerContainer.gameObject.SetActive(true);
                    _GuardContainer.gameObject.SetActive(true);
                    break;
                
                //Two Panel View
                case(PanelUIState.STANDARD):
                    _TopContainer.gameObject.SetActive(true);
                    _MiddleContainer.gameObject.SetActive(false);
                    _HealthContainer.SetParent(_LowerContainer);
                    _LowerContainer.gameObject.SetActive(true);
                    _GuardContainer.gameObject.SetActive(false);
                    break;
            }
        }
        public void UpdateCurrentHP(int hp)
        {
            CurrentHP = hp;
            _healthText.text = $"{CurrentHP} / {MaxHP}";
            _healthBarUpdater.UpdateHealthBarFill( (float) CurrentHP/MaxHP);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
            _animator.ResetTrigger("Hide");
            _animator.SetTrigger("Show");
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            _animator.ResetTrigger("Show");
            _animator.SetTrigger("Hide");
        }
    }
}
