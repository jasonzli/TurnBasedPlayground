using System.Collections;
using Code.ScriptableObjects;
using Code.ViewModels;
using Code.ViewScripts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Code.BattleSystem
{
    /// <summary>
    /// Quick and dirty conductor for the battle system.
    /// </summary>
    public class BattleConductor : MonoBehaviour
    {
        //Data to setup players
        [SerializeField] private ActorData _playerData;
        [SerializeField] private ActorData _enemyData;

        //Buttons and panels to display
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _guardButton;
        [SerializeField] BattleActorUIView _playerPanel;
        [SerializeField] BattleActorUIView _enemyPanel;
        
        //Internal data for the system and actions
        private BattleSystem _battleSystem;
        private IBattleAction _playerAttackAction;
        private IBattleAction _playerHealAction;
        private IBattleAction _playerGuardAction;
        private PlayerPanelViewModel playerOnePanelViewModel;
        private PlayerPanelViewModel playerTwoPanelViewModel;
        
        void Start()
        {
            _battleSystem = new BattleSystem(_playerData,_enemyData);

            playerOnePanelViewModel = new PlayerPanelViewModel(_battleSystem.PlayerOne,true);
            playerTwoPanelViewModel = new PlayerPanelViewModel(_battleSystem.PlayerTwo,false);
            
            _playerPanel.Initialize(playerOnePanelViewModel);
            _enemyPanel.Initialize(playerTwoPanelViewModel);
            
            _playerAttackAction = new PlayerBattleAction(_playerData.AttackActionData.AsParameters(), _battleSystem.PlayerOne, _battleSystem.PlayerTwo);
            _playerHealAction = new PlayerBattleAction(_playerData.HealActionData.AsParameters(), _battleSystem.PlayerOne, _battleSystem.PlayerOne);
            _playerGuardAction = new PlayerBattleAction(_playerData.GuardActionData.AsParameters(), _battleSystem.PlayerOne, _battleSystem.PlayerOne);
            
            _attackButton.onClick.AddListener(PlayerAttack);
            _healButton.onClick.AddListener(PlayerHeal);
            _guardButton.onClick.AddListener(PlayerGuard);
            
            _battleSystem.BattleOver += OnBattleOver;
        }
        
        private void OnBattleOver(IBattleActor winner)
        {
            Debug.Log($"Battle Over! {winner.Name} has won!");
        }
        
        private void PlayerAttack()
        {
            _battleSystem.PerformAction(_playerAttackAction);
            playerTwoPanelViewModel.UpdateFromBattleActor(_battleSystem.PlayerTwo);
        }

        private void PlayerHeal()
        {
            _battleSystem.PerformAction(_playerHealAction);
            playerTwoPanelViewModel.UpdateFromBattleActor(_battleSystem.PlayerTwo);
        }

        private void PlayerGuard()
        {
            _battleSystem.PerformAction(_playerGuardAction);
            playerTwoPanelViewModel.UpdateFromBattleActor(_battleSystem.PlayerTwo);
        }
    }
    
}