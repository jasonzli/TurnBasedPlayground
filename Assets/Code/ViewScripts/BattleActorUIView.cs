using UnityEngine;
namespace Code
{
    [RequireComponent(typeof(Animator))]
    public class BattleActorUIView : MonoBehaviour, IView
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
        [SerializeField] private RectTransform _LowerHealthContainer;
        [SerializeField] private RectTransform _MiddleHealthContainer;
        [SerializeField] private RectTransform _P1TokenContainer;
        [SerializeField] private RectTransform _P2TokenContainer;
    
        //values to actually update
        [SerializeField] private TMPro.TextMeshProUGUI _nameText;
        [SerializeField] private TMPro.TextMeshProUGUI _middleHPText;
        [SerializeField] private TMPro.TextMeshProUGUI _lowerHPText;

        private TMPro.TextMeshProUGUI _currentHPTextHolder;
        private Animator _animator;

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
                    _MiddleHealthContainer.gameObject.SetActive(true);
                    _LowerContainer.gameObject.SetActive(true);
                    _LowerHealthContainer.gameObject.SetActive(false);
                    _GuardContainer.gameObject.SetActive(true);
                    _currentHPTextHolder = _middleHPText;
                    break;
                
                //Two Panel View
                case(PanelUIState.STANDARD):
                    _TopContainer.gameObject.SetActive(true);
                    _MiddleContainer.gameObject.SetActive(false);
                    _MiddleHealthContainer.gameObject.SetActive(false);
                    _LowerContainer.gameObject.SetActive(true);
                    _LowerHealthContainer.gameObject.SetActive(true);
                    _GuardContainer.gameObject.SetActive(false);
                    _currentHPTextHolder = _lowerHPText;
                    break;
            }
        
            _currentHPTextHolder.text = $"{CurrentHP} / {MaxHP}";
        }
        public void UpdateCurrentHP(int hp)
        {
            CurrentHP = hp;
            _currentHPTextHolder.text = $"{CurrentHP} / {MaxHP}";
        }

        public void Show()
        {
            _animator.ResetTrigger("Hide");
            _animator.SetTrigger("Show");
        }

        public void Hide()
        {
            _animator.ResetTrigger("Show");
            _animator.SetTrigger("Hide");
        }
    }
}
