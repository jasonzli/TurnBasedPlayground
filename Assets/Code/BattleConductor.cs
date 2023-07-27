using System.Collections;
using Code.URL_Test;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Code
{

    //THis is a model
    public class BattleSystem
    {
        public IPlayerActor Player { get; set; }
        public IBattleActor Enemy { get; set; }

        public IBattleAction attackAction;
        public IBattleAction healAction;
        public IBattleAction guardAction;

        public BattleSystem(ActorData playerData, ActorData enemyData)
        {
            Player = new PlayerActor(playerData.Name,playerData.Health, Random.value > .5f);
            Enemy = new EnemyActor(enemyData.Name,enemyData.Health, Random.value > .5f);
            
            //Create BattleActions from aforementioned parameters
            attackAction = new DamageAction(Player.AttackParameters, Player, Enemy);
            healAction = new HealAction(Player.HealParameters, Player, Player);
            guardAction = new GuardAction(Player.GuardParameters, Player, Player);
        }

        public void QueueAction(IBattleAction action)
        {
            action.Execute();
            Debug.Log("Actor performs " + action.Parameters.moveName + "!");
            Debug.Log("Actor deals " + action.Parameters.hpDamage + " damage!");
            Debug.Log("Actor heals " + action.Parameters.healAmount + " HP!");
            Debug.Log("Actor applies guard: " + action.Parameters.doesApplyGuard);
            Debug.Log("Player has " + Player.CurrentHP + " HP remaining!");
            Debug.Log("Enemy has " + Enemy.CurrentHP + " HP remaining!");
            //Tell us who is guarding
            Debug.Log("Player is guarding: " + Player.Guarded);
            Debug.Log("Enemy is guarding: " + Enemy.Guarded);
        }
    }
    /// <summary>
    /// Quick and dirty conductor for the battle system.
    /// </summary>
    public class BattleConductor : MonoBehaviour
    {
        private BattleSystem _battleSystem;
        [SerializeField] private ActorData _playerData;
        [SerializeField] private ActorData _enemyData;


        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _guardButton;
        
        IBattleAction attackAction;
        IBattleAction healAction;
        IBattleAction guardAction;
        
        [SerializeField] BattleActorUIView _playerPanel;
        [SerializeField] BattleActorUIView _enemyPanel;
        private PlayerPanelViewModel playerOnePanelViewModel;
        private PlayerPanelViewModel playerTwoPanelViewModel;
        void Start()
        {
            _battleSystem = new BattleSystem(_playerData,_enemyData);

            playerOnePanelViewModel = new PlayerPanelViewModel(_battleSystem.Player,true);
            playerTwoPanelViewModel = new PlayerPanelViewModel(_battleSystem.Enemy,false);
            _playerPanel.Initialize(playerOnePanelViewModel);
            _enemyPanel.Initialize(playerTwoPanelViewModel);
            
            attackAction = _battleSystem.attackAction;
            healAction = _battleSystem.healAction;
            guardAction = _battleSystem.guardAction;
            
            _attackButton.onClick.AddListener(PlayerAttack);
            _healButton.onClick.AddListener(PlayerHeal);
            _guardButton.onClick.AddListener(PlayerGuard);
        }

        void PlayerAttack()
        {
            _battleSystem.QueueAction(attackAction);
            StartCoroutine(FetchResult(_enemyData.URL));
        }
        
        void PlayerHeal()
        {
            _battleSystem.QueueAction(healAction);
            StartCoroutine(FetchResult(_enemyData.URL));
        }
        
        void PlayerGuard()
        {
            _battleSystem.QueueAction(guardAction);
            playerOnePanelViewModel.UpdateFromBattleActor(_battleSystem.Player);
            StartCoroutine(FetchResult(_enemyData.URL));
        }

        IEnumerator FetchResult(string url)
        {
            BattleActionParameters data;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Error: " + webRequest.error);
                    yield break;
                }

                
                try
                {
                    data = JsonUtility.FromJson<BattleActionParameters>(webRequest.downloadHandler.text);
                }
                catch
                {
                    Debug.Log("Data had a problem fetching, try again");
                    yield break;
                }
                
                
            }
            DamageAction enemyAttack = new DamageAction(data, _battleSystem.Enemy, _battleSystem.Player);
            _battleSystem.QueueAction(enemyAttack);
            if (_battleSystem.Player.Guarded)
            {
                _battleSystem.Player.Guarded = false;
            }

            if (_battleSystem.Enemy.Guarded)
            {
                _battleSystem.Enemy.Guarded = false;
            }

            playerOnePanelViewModel.UpdateFromBattleActor(_battleSystem.Player);
            playerTwoPanelViewModel.UpdateFromBattleActor(_battleSystem.Enemy);
        }
    }

    public interface IBattleActor
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public bool Guarded { get; set; }
    }

    /// <summary>
    /// Represents a single action in a battle.
    /// </summary>
    public struct BattleActionParameters
    {
        public string moveName;
        public int hpDamage;
        public int healAmount;
        public bool doesApplyGuard;
    
        public string MoveName
        {
            get => moveName;
            set => moveName = value;
        }
    
        public int Damage
        {
            get => hpDamage;
            set => hpDamage = value;
        }
    
        public int HealAmount
        {
            get => healAmount;
            set => healAmount = value;
        }
    
        public bool ApplyGuard
        {
            get => doesApplyGuard;
            set => doesApplyGuard = value;
        }

        public BattleActionParameters(string moveName, int damage, int healAmount, bool applyGuard) : this()
        {
            MoveName = moveName;
            Damage = damage;
            HealAmount = healAmount;
            ApplyGuard = applyGuard;
        }
    
    }

    public interface IBattleAction
    {
        BattleActionParameters Parameters { get; }
        IBattleActor Source { get; }
        IBattleActor Target { get; }
        bool Execute();
    }

    /// <summary>
    /// BattleActions are created from parameters and executed on a target from a source
    /// They are fully constructed from BattleActionParamaters, but are left to individual classes for how to apply this
    /// </summary>
    public abstract class BattleActionBase : IBattleAction
    {
        public BattleActionParameters Parameters { get; }
        public IBattleActor Source { get; }
        public IBattleActor Target { get; }

        protected BattleActionBase(BattleActionParameters parameters, IBattleActor source, IBattleActor target)
        {
            Parameters = parameters;
            Source = source;
            Target = target;
        }

        protected BattleActionBase()
        {
            BattleActionParameters parameters = new BattleActionParameters();
            Source = null;
            Target = null;
        }

        public abstract bool Execute();
    }

    public class DamageAction : BattleActionBase
    {
        public DamageAction(BattleActionParameters parameters, IBattleActor player, IBattleActor target) : base(parameters, player, target) { }
        public int Result { get; private set; } = 0;
        public override bool Execute()
        {
            if (Target.Guarded)
            {
                Result = 0;
                return true;
            }
        
            Target.CurrentHP = Mathf.Max(0, Target.CurrentHP - Parameters.hpDamage);
            return true;
        }
    }

    public class HealAction : BattleActionBase
    {
        public HealAction(BattleActionParameters parameters, IBattleActor player, IBattleActor target) : base(parameters, player, target){ }

        public override bool Execute()
        {
            if(Target.CurrentHP == Target.MaxHP)
            {
                return false;
            }
            Target.CurrentHP = Mathf.Min(Target.MaxHP, Target.CurrentHP + Parameters.healAmount);
            return true;
        }
    }

    public class GuardAction : BattleActionBase
    {
        public GuardAction(BattleActionParameters parameters, IBattleActor player, IBattleActor target) : base(parameters, player, target){ }

        public override bool Execute()
        {
            //Return false if target is already guarded and the parameters does apply guard
            if (Parameters.doesApplyGuard == Target.Guarded)
            {
                return false;
            }
        
            Target.Guarded = Parameters.doesApplyGuard;
            return true;
        }
    }

    public abstract class BattleActorBase : IBattleActor
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }
        private int _maxHP;
        public int MaxHP { get => _maxHP; set => _maxHP = value; }
        private int _currentHP;
        public int CurrentHP { get => _currentHP; set => _currentHP = value; }
        private bool _guarded;
        public bool Guarded { get => _guarded; set => _guarded = value; }
        
        public BattleActorBase(string name, int maxHP, bool guarded)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            Guarded = guarded;
        }
    }

    public interface IPlayerActor : IBattleActor
    {
        public BattleActionParameters AttackParameters { get; }
        public BattleActionParameters HealParameters { get; }
        public BattleActionParameters GuardParameters { get; }
    }
    public class PlayerActor : BattleActorBase, IPlayerActor
    {
        public PlayerActor(string name, int maxHP, bool guarded) : base(name, maxHP, guarded) { }
    
        public BattleActionParameters AttackParameters { get; } = new BattleActionParameters("Attack", 10, 0, false);
        public BattleActionParameters HealParameters { get; } = new BattleActionParameters("Heal", 0, 15, false);
        public BattleActionParameters GuardParameters { get; } = new BattleActionParameters("Guard", 0, 0, true);

    }

    public class EnemyActor : BattleActorBase
    {
        public EnemyActor(string name, int maxHP, bool guarded) : base(name, maxHP, guarded) { }
    }
}