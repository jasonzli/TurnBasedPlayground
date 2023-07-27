using System.Collections.Generic;
using Code.ScriptableObjects;
using Code.ViewModels;
using Code.ViewScripts;
using UnityEngine;

namespace Code.BattleSystem
{
    /// <summary>
    /// This conductor serves as the gobetween and home for everything
    /// It is the home to the model (the battle system)
    /// It is the home to to UI references
    /// And home to ViewModels
    /// </summary>
    public class BattleConductor : MonoBehaviour
    {
        //Data to setup players
        [SerializeField] private ActorData _playerData;
        [SerializeField] private ActorData _enemyData;

        //Buttons and panels to display
        [SerializeField] BattleActorUIView _playerPanel;
        [SerializeField] BattleActorUIView _enemyPanel;
        [SerializeField] BattleActionSelectionPanelView _playerBattleActionSelectionPanelView;
        
        //Internal data for the system and actions
        private BattleSystem _battleSystem;
        private PlayerPanelViewModel _playerOnePanelViewModel;
        private PlayerPanelViewModel _playerTwoPanelViewModel;
        private BattleActionSelectionViewModel _playerBattleActionViewModel;
        
        void Start()
        {
            _battleSystem = new BattleSystem(_playerData,_enemyData);

            _playerOnePanelViewModel = new PlayerPanelViewModel(_battleSystem.PlayerOne,true);
            _playerTwoPanelViewModel = new PlayerPanelViewModel(_battleSystem.PlayerTwo,false);
            
            _playerPanel.Initialize(_playerOnePanelViewModel);
            _enemyPanel.Initialize(_playerTwoPanelViewModel);
            
            List<BattleActionData> playerOneActions = new List<BattleActionData>(){_playerData.AttackActionData,_playerData.HealActionData,_playerData.GuardActionData};
            
            _playerBattleActionViewModel = new BattleActionSelectionViewModel(playerOneActions,_battleSystem.PlayerOne,_battleSystem.PlayerTwo);
            _playerBattleActionSelectionPanelView.Initialize(_playerBattleActionViewModel);
            
            
            _playerBattleActionViewModel.OnActionSelected += SendAction;
            _battleSystem.BattleOver += OnBattleOver;
        }

        private void SendAction(IBattleAction action)
        {
            _battleSystem.PerformAction(action);
            UpdateViewModels();
        }

        private void UpdateViewModels()
        {
            _playerOnePanelViewModel.UpdateFromBattleActor();
            _playerTwoPanelViewModel.UpdateFromBattleActor();
        }
        
        private void OnBattleOver(IBattleActor winner)
        {
            Debug.Log($"Battle Over! {winner.Name} has won!");
        }
        
    }
    
}