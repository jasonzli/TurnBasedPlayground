using Code.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Code.ViewScripts
{
    /// <summary>
    /// The view class for the battle actor data UI
    /// Configures the UI based on all the data in the viewmodel
    /// This engages in maybe more logic than it should, but it keeps all the complexity here rather than spread around
    /// </summary>
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

        [SerializeField] private Animator _animator;
        [SerializeField] private RectTransform _TopContainer;
        [SerializeField] private RectTransform _MiddleContainer;
        [SerializeField] private RectTransform _LowerContainer;
        [SerializeField] private RectTransform _GuardContainer;
        [SerializeField] private RectTransform _P1TokenContainer;
        [SerializeField] private RectTransform _P2TokenContainer;
        [SerializeField] private RectTransform _HealthContainer;
        [SerializeField] private HealthBarUpdater _healthBarUpdater;
        [SerializeField] private Image _P1ImageToken;
        [SerializeField] private Image _P2ImageToken;
        
    
        //values to actually update
        [SerializeField] private TMPro.TextMeshProUGUI _nameText;
        [SerializeField] private TMPro.TextMeshProUGUI _healthText;

        private PlayerPanelViewModel _context;

        public void Initialize(PlayerPanelViewModel context)
        {
            _context = context;
            Name = context.Name;
            gameObject.SetActive(true);
            _nameText.text = Name.ToUpper();
            CurrentHP = context.CurrentHP;
            MaxHP = context.MaxHP;
            IsGuarding = context.IsGuarding;
            IsPlayerOne = context.IsP1Side;

            _panelUIState = DeterminePanelUIState(IsGuarding);
            SetUpUIPanelBasedOnStatus();
            SetupToken();
        
            UpdateCurrentHP(CurrentHP);
            context.CurrentHP.PropertyChanged += UpdateCurrentHP;
            context.IsGuarding.PropertyChanged += UpdatePlayerUIStatus;
            context.Visibility.PropertyChanged += SetVisibility;

            ResetTriggers();
            SetVisibility(context.Visibility);
        }
        
        private void SetupToken()
        {
            if (IsPlayerOne)
            {
                _P1TokenContainer.gameObject.SetActive(true);
                _P1ImageToken.sprite = _context.ActorData.Icon;;
            }
            else
            {
                _P2TokenContainer.gameObject.SetActive(true);
                _P2ImageToken.sprite = _context.ActorData.Icon;;
            }
        }

        private PanelUIState DeterminePanelUIState(bool isGuarding)
        {
            return isGuarding ? PanelUIState.GUARDING : PanelUIState.STANDARD;
        }

        public void UpdatePlayerUIStatus(bool isGuarding)
        {
            IsGuarding = isGuarding;
            _panelUIState = DeterminePanelUIState(IsGuarding);
            SetUpUIPanelBasedOnStatus();
        }

        /// <summary>
        /// Determine the UI panel's view state based on the guarding status
        ///
        /// There might be other states later, so we'll leave it hacky like this
        /// </summary>
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

        private void ResetTriggers()
        {
            _animator.ResetTrigger("Show");
            _animator.ResetTrigger("Hide");
        }

        public override void Show()
        {
            ResetTriggers();
            _animator.SetTrigger("Show");
        }

        public override void Hide()
        {
            ResetTriggers();
            _animator.SetTrigger("Hide");
        }
    }
}
