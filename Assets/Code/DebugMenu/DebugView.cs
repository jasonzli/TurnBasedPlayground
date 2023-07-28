using Code.ScriptableObjects;
using Code.ViewScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.DebugMenu
{
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

            HealthValue = _context.HealthValue;
            HealPower = _context.HealPower;
            AttackPower = _context.AttackPower;
            
            _context.HealthValue.PropertyChanged += UpdateHealthValue;
            _context.HealPower.PropertyChanged += UpdateHealPower;
            _context.AttackPower.PropertyChanged += UpdateAttackPower;
            _context.Visibility.PropertyChanged += SetVisibility;

            ElementalButton.onClick.RemoveAllListeners();
            SnakeButton.onClick.RemoveAllListeners();
            CatButton.onClick.RemoveAllListeners();
            ResetButton.onClick.RemoveAllListeners();
            CloseButton.onClick.RemoveAllListeners();
            
            ElementalButton.onClick.AddListener(() => { _context.SetToActorData(_elementalData);});
            SnakeButton.onClick.AddListener(() => { _context.SetToActorData(_snakeData);});
            CatButton.onClick.AddListener(() => { _context.SetToActorData(_catData);});
            
            HealthAddButton.onClick.AddListener(() => { _context.AddHealth();});
            HealthSubtractButton.onClick.AddListener(() => { _context.SubtractHealth();});
            
            HealPowerAddButton.onClick.AddListener(() => { _context.AddHealPower();});
            HealPowerSubtractButton.onClick.AddListener(() => { _context.SubtractHealPower();});
            
            AttackPowerAddButton.onClick.AddListener(() => { _context.AddAttackPower();});
            AttackPowerSubtractButton.onClick.AddListener(() => { _context.SubtractAttackPower();});
            
            ResetButton.onClick.AddListener(() => { _context.CloseDebugMenu(); _context.TriggerReset(); });
            CloseButton.onClick.AddListener(() => { _context.CloseDebugMenu(); });

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