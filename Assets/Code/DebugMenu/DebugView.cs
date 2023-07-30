using Code.ScriptableObjects;
using Code.ViewScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.DebugMenu
{
    /// <summary>
    /// A view that displays the debug menu and sets up all the functionality
    /// </summary>
    public class DebugView : ViewBase
    {
        [SerializeField] private Animator _animator;
        private DebugViewModel _context;

        [Header("UI Objects")]
        [SerializeField] private TextMeshProUGUI HealthText;
        [SerializeField] private TextMeshProUGUI HealPowerText;
        [SerializeField] private TextMeshProUGUI AttackPowerText;
        [SerializeField] private Button ElementalButton;
        [SerializeField] private Button SnakeButton;
        [SerializeField] private Button CatButton;
        [SerializeField] private Button ResetButton;
        [SerializeField] private Button CloseButton;
        [SerializeField] private Button OpenButton;
        [SerializeField] private Button CloseGameButton;
        [SerializeField] private Button ResetScreenSizeButton;
        [SerializeField] private Button UnsafeResetButton;
        [SerializeField] private EventTrigger OpenButtonTrigger;
        [SerializeField] private Button HealthAddButton;
        [SerializeField] private Button HealthSubtractButton;
        [SerializeField] private Button HealPowerAddButton;
        [SerializeField] private Button HealPowerSubtractButton;
        [SerializeField] private Button AttackPowerAddButton;
        [SerializeField] private Button AttackPowerSubtractButton;

        [Header("Actor Data")]
        [SerializeField] private ActorData _elementalData;
        [SerializeField] private ActorData _snakeData;
        [SerializeField] private ActorData _catData;
        
        private int HealthValue { get; set; }
        private int HealPower { get; set; }
        private int AttackPower { get; set; }
        
        public void Initialize(DebugViewModel context)
        {
            _context = context;
            gameObject.SetActive(true);

            //Clear existing listeners
            ElementalButton.onClick.RemoveAllListeners();
            SnakeButton.onClick.RemoveAllListeners();
            CatButton.onClick.RemoveAllListeners();
            ResetButton.onClick.RemoveAllListeners();
            CloseButton.onClick.RemoveAllListeners();
            CloseGameButton.onClick.RemoveAllListeners();
            ResetScreenSizeButton.onClick.RemoveAllListeners();
            
            //Set initial values from context
            HealthValue = _context.HealthValue;
            HealPower = _context.HealPower;
            AttackPower = _context.AttackPower;
            
            //Set up the listeners for the view model
            _context.HealthValue.PropertyChanged += UpdateHealthValue;
            _context.HealPower.PropertyChanged += UpdateHealPower;
            _context.AttackPower.PropertyChanged += UpdateAttackPower;
            _context.Visibility.PropertyChanged += SetVisibility;
            _context.ButtonVisibility.PropertyChanged += SetButtonVisibility;

            //Set up all button listeners
            ElementalButton.onClick.AddListener(() => { _context.SetToActorData(_elementalData);});
            SnakeButton.onClick.AddListener(() => { _context.SetToActorData(_snakeData);});
            CatButton.onClick.AddListener(() => { _context.SetToActorData(_catData);});
            
            HealthAddButton.onClick.AddListener(() => { _context.AddHealth();});
            HealthSubtractButton.onClick.AddListener(() => { _context.SubtractHealth();});
            
            HealPowerAddButton.onClick.AddListener(() => { _context.AddHealPower();});
            HealPowerSubtractButton.onClick.AddListener(() => { _context.SubtractHealPower();});
            
            AttackPowerAddButton.onClick.AddListener(() => { _context.AddAttackPower();});
            AttackPowerSubtractButton.onClick.AddListener(() => { _context.SubtractAttackPower();});
            
            //I will fully admit to not previously knowing this was possible to do this .triggers[index].callback reference. Unity!!!
            OpenButtonTrigger.triggers[0].callback.RemoveAllListeners();
            OpenButtonTrigger.triggers[1].callback.RemoveAllListeners();
            OpenButtonTrigger.triggers[0].callback.AddListener((data) => { _context.SetButtonVisibility(true); });
            OpenButtonTrigger.triggers[1].callback.AddListener((data) => { _context.SetButtonVisibility(false); });
            
            ResetButton.onClick.AddListener(() =>
            {
                _context.CloseDebugMenu();
                _context.TriggerResetWithData();
            });
            UnsafeResetButton.onClick.AddListener(() =>
            {
                _context.CloseDebugMenu();
                _context.TriggerResetWithDataUnsafe();
            });
            CloseButton.onClick.AddListener(() => { _context.CloseDebugMenu(); });
            ResetScreenSizeButton.onClick.AddListener(() =>
            {
#if UNITY_ANDROID
                Screen.SetResolution(1920,1080, true);
#else
                Screen.SetResolution(1920,1080, false);
#endif
            });
            CloseGameButton.onClick.AddListener(() => { Application.Quit();});
            OpenButton.onClick.AddListener(() => { _context.OpenDebugMenu(); });

            //Set initial visibility
            ResetTriggers();
            SetVisibility(_context.Visibility);

            UpdateAttackPower(AttackPower);
            UpdateHealPower(HealPower);
            UpdateHealthValue(HealthValue);
        }

        public void UpdateHealPower(int value)
        {
            HealPower = value;
            HealPowerText.text = "Heal Power: " + HealPower;
        }
        
        public void UpdateAttackPower(int value)
        {
            AttackPower = value;
            AttackPowerText.text = "Attack Power: " + AttackPower;
        }
        
        public void UpdateHealthValue(int value)
        {
            HealthValue = value;
            HealthText.text = "Health: " + HealthValue;
        }

        private void SetButtonVisibility(bool visibility)
        {
            _animator.ResetTrigger("ShowButton");
            _animator.ResetTrigger("HideButton");
            _animator.SetTrigger(visibility ? "ShowButton" : "HideButton");
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