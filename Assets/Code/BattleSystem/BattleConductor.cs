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
        [SerializeField] BattleOverlayPanelUIView _battleOverlayPanelUIView;
        [SerializeField] BattleActorUIView _playerPanel;
        [SerializeField] BattleActorUIView _enemyPanel;
        [SerializeField] BattleActionSelectionPanelView _playerBattleActionSelectionPanelView;
        
        //Internal data for the system and actions
        private BattleSystem _battleSystem;
        private PlayerPanelViewModel _playerOnePanelViewModel;
        private PlayerPanelViewModel _playerTwoPanelViewModel;
        private BattleActionSelectionViewModel _playerBattleActionViewModel;
        private BattleOverlayPanelViewModel _battleOverlayPanelViewModel;
        
        void Start()
        {
            ResetBattle();
        }

        [ContextMenu("Reset")]
        void ResetBattle()
        {
            _battleSystem = new BattleSystem(_playerData,_enemyData);

            _playerOnePanelViewModel = new PlayerPanelViewModel(_battleSystem.PlayerOne,true);
            _playerTwoPanelViewModel = new PlayerPanelViewModel(_battleSystem.PlayerTwo,false);
            List<BattleActionData> playerOneActions = new List<BattleActionData>(){_playerData.AttackActionData,_playerData.HealActionData,_playerData.GuardActionData};
            _playerBattleActionViewModel = new BattleActionSelectionViewModel(playerOneActions,_battleSystem.PlayerOne,_battleSystem.PlayerTwo);
            _battleOverlayPanelViewModel = new BattleOverlayPanelViewModel("Combat Begins", false, true, () =>
            {
                ResetBattle();
            });
           
            _playerPanel.Initialize(_playerOnePanelViewModel);
            _enemyPanel.Initialize(_playerTwoPanelViewModel);
            _playerBattleActionSelectionPanelView.Initialize(_playerBattleActionViewModel);
            _battleOverlayPanelUIView.Initialize(_battleOverlayPanelViewModel);
            
            HideBattleActorUI();
            
            _playerBattleActionViewModel.OnActionSelected += SendAction;
            _battleOverlayPanelViewModel.OnPanelHidden += ShowBattleActorUI;
            _battleSystem.BattleOver += OnBattleOver;
            
        }

        private void ShowBattleActorUI()
        {
            _playerPanel.Show();
            _enemyPanel.Show();
            _playerBattleActionSelectionPanelView.Show();
        }

        private void HideBattleActorUI()
        {
            _playerPanel.Hide();
            _enemyPanel.Hide();
            _playerBattleActionSelectionPanelView.Hide();
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
            ResetBattle();
        }
        
    }

}