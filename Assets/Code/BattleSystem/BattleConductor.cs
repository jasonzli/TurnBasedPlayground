using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Code.ScriptableObjects;
using Code.Utility;
using Code.ViewModels;
using Code.ViewScripts;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Code.BattleSystem
{
    /// <summary>
    /// This conductor serves as the go-between and home for everything
    /// It is the home to the battle system
    /// It is the home to to UI references
    /// And home to ViewModels, and is responsible for hooking up the VMs with the Vs
    ///
    /// All the view behavior could be offloaded onto a UI manager of sorts but for now it's here. It's all here.
    /// </summary>
    public class BattleConductor : MonoBehaviour
    {
        //Data to setup players
        [Header("Actor Data Objects")]
        [SerializeField] private ActorData _playerData;

        public ActorData playerOneData
        {
            get { return _playerData; }
        }

        [SerializeField] private ActorData _enemyData;

        //Buttons and panels to display
        [Header("UI View Objects")]
        [SerializeField] private BattleOverlayPanelUIView _battleBeginsOverlayPanelUIView;
        [SerializeField] private BattleOverlayPanelUIView _playerWinView;
        [SerializeField] private BattleOverlayPanelUIView _enemyWinView;
        [SerializeField] private BattleActorUIView _playerPanel;
        [SerializeField] private BattleActorUIView _enemyPanel;
        [SerializeField] private BattleActionSelectionPanelView _playerBattleActionSelectionPanelView;
        [SerializeField] private ActionPanelView _actionPanelView;
        [SerializeField] private ErrorPanelView _errorPanelView;
        
        //Transforms for the player tokens
        [SerializeField] private PlayerTokenView _playerOneTokenView;
        [SerializeField] private PlayerTokenView _playerTwoTokenView;

        //ViewModels for the system and actions
        private BattleSystem _battleSystem;
        private PlayerPanelViewModel _playerOnePanelViewModel;
        private PlayerPanelViewModel _playerTwoPanelViewModel;
        private PlayerTokenViewModel _playerOneTokenViewModel;
        private PlayerTokenViewModel _playerTwoTokenViewModel;
        private BattleOverlayPanelViewModel _playerWinViewModel;
        private BattleOverlayPanelViewModel _enemyWinViewModel;
        private BattleActionSelectionViewModel _playerBattleActionViewModel;
        private BattleOverlayPanelViewModel _battleOverlayPanelViewModel;
        private ActionPanelViewModel _actionPanelViewModel;
        private ErrorPanelViewModel _errorPanelViewModel;

        //Internal  data for battle
        private List<IBattleActor> _turnOrder = new List<IBattleActor>();
        private Dictionary<string,BattleActionData> _playerBattleActionDictionary = new Dictionary<string, BattleActionData>();
        private Dictionary<string,BattleActionData> _enemyBattleActionDictionary = new Dictionary<string, BattleActionData>();
        private int turnIndex = 0;

        [Header("Player Material for Debug Purposes")]
        [SerializeField] private Material _playerMaterial;
        
        void Start()
        {
            ResetBattle();
        }

        [ContextMenu("Reset")]
        public async Task ResetBattle(bool unsafeBattle = false)
        {
            //Establish battle system
            _battleSystem = new BattleSystem(_playerData, _enemyData);

            //Setup viewmodels with context
            _playerOnePanelViewModel = new PlayerPanelViewModel(_battleSystem.PlayerOne, _playerData,true);
            _playerTwoPanelViewModel = new PlayerPanelViewModel(_battleSystem.PlayerTwo, _enemyData,false);

            _playerOneTokenViewModel = new PlayerTokenViewModel(Camera.main.transform, _playerTwoTokenView.transform,_battleSystem.PlayerOne);
            _playerTwoTokenViewModel = new PlayerTokenViewModel(Camera.main.transform, _playerOneTokenView.transform,_battleSystem.PlayerTwo);

            List<BattleActionData> playerOneActions = new List<BattleActionData>()
                { _playerData.AttackActionData, _playerData.HealActionData, _playerData.GuardActionData };
            _playerBattleActionViewModel = new BattleActionSelectionViewModel(playerOneActions, _battleSystem.PlayerOne,
                _battleSystem.PlayerTwo, unsafeBattle);
            
            _battleOverlayPanelViewModel = new BattleOverlayPanelViewModel("Combat Begins", false, true, () => { });
            _playerWinViewModel = new BattleOverlayPanelViewModel($"{_battleSystem.PlayerOne.Name} Wins!", true, false,
                () => { ResetBattle(); });
            _enemyWinViewModel = new BattleOverlayPanelViewModel($"{_battleSystem.PlayerTwo.Name} Wins!", true, false,
                () => { ResetBattle(); });

            _actionPanelViewModel = new ActionPanelViewModel();
            _errorPanelViewModel = new ErrorPanelViewModel(
                () => { ResetBattle();},
                () => { AttemptURLAction();});

            //Initialize views
            _playerPanel.Initialize(_playerOnePanelViewModel);
            UpdatePlayerTokenWorldMaterial(); // just for material setting because we are changing a material instance
            _enemyPanel.Initialize(_playerTwoPanelViewModel);
            _playerBattleActionSelectionPanelView.Initialize(_playerBattleActionViewModel);
            _battleBeginsOverlayPanelUIView.Initialize(_battleOverlayPanelViewModel);
            _actionPanelView.Initialize(_actionPanelViewModel);
            _errorPanelView.Initialize(_errorPanelViewModel);
            _playerWinView.Initialize(_playerWinViewModel);
            _enemyWinView.Initialize(_enemyWinViewModel);
            _playerOneTokenView.Initialize(_playerOneTokenViewModel);
            _playerTwoTokenView.Initialize(_playerTwoTokenViewModel);

            // Set internal data
            _turnOrder = new List<IBattleActor>();
            _turnOrder.Add(_battleSystem.PlayerOne);
            _turnOrder.Add(_battleSystem.PlayerTwo);
            turnIndex = 0;

            //Set up the event to alert the battle system
            _playerBattleActionViewModel.OnActionSelected += (action) => { SetAction(action);};

            //Set beginning
            HideAllUI();
            ShowBeginningOverlay();
        }

        #region Battle Controls

        /// <summary>
        /// A stateless-ish (turn order is checked) battle control "system" that is run from the UI
        /// the game doesn't have an awareness
        ///
        /// This method has ballooned now that I need to handle VFX
        /// </summary>
        /// <param name="action">The battleaction to execute this "turn"</param>
        private async Task SetAction(IBattleAction action)
        {
            //Player One needs to look at the Target
            _playerOneTokenViewModel.LookAtTarget();
            
            HidePlayerBattleActionPanel(); //player has taken a move, hide the UI
            ShowActionPanel(action); //show the action panel
            await Task.Delay(1500);
            HideActionPanel(); //hide the action panel;
            
            //Perform the visual effects phase
            await ExecuteVisualEffects(action);
            await Task.Delay(700);

            _battleSystem.PerformAction(action);
            UpdatePlayerViewModels();
            

            //Increment turn
            turnIndex = (turnIndex + 1) % _turnOrder.Count;

            //check for winner
            IBattleActor winner = _battleSystem.EvaluateWinner();
            if (winner != null)
            {
                bool isPlayerOneWinner = winner.Name == _battleSystem.PlayerOne.Name;
                if (isPlayerOneWinner)
                {
                    _playerTwoTokenViewModel.Knockdown();
                }
                else
                {
                    _playerOneTokenViewModel.Knockdown();
                }
                //visual task delay to see victory condition
                await Task.Delay(1600);
                BattleOver(winner);
                return;
            }
            
            await Task.Delay(200);
            
            //This is the hacky way we're gonna do this
            //Enemy turn
            if (_turnOrder[turnIndex] == _battleSystem.PlayerTwo)
            {
                IBattleAction urlAction = await FetchAction();
                if (urlAction == null)
                {
                    ShowErrorPanel();
                    return;
                }

                SetAction(urlAction);
            }
            //Our turn
            else
            {
                ShowAllBattleUI();
                _playerOneTokenViewModel.LookAtCamera();
            }
        }

        //URL only action, called from the error panel
        private async Task AttemptURLAction()
        {
            HideAllUI();
            //show the standard UI, minus the battle actions
            ShowBattleActorDataPanels();
            
            IBattleAction urlAction = await FetchAction();
            if (urlAction == null)
            {
                ShowErrorPanel();
                return;
            }

            SetAction(urlAction);
        }

        //Essential function to fetch the action from the URL
        private async Task<BattleAction> FetchAction()
        {
            //Get the action from the EnemyAction url
            string response = await URLUtility.FetchJSONStringFromURL(_enemyData.URL);

            BattleActionParameters battleActionData;
            if (response == null)
            {
                _errorPanelViewModel.UpdateErrorMessage("Bad response from server.");
                return null;
            }

            try
            {
                battleActionData = JsonUtility.FromJson<BattleActionParameters>(response);
            }
            catch (Exception e)
            {
                _errorPanelViewModel.UpdateErrorMessage("Invalid JSON response. Oh no!");
                return null;
            }

            //Battle Action data is good,create battle Action
            BattleAction urlAction =
                new BattleAction(battleActionData, _battleSystem.PlayerTwo, _battleSystem.PlayerOne);

            return urlAction;

        }
        
        
        private void BattleOver(IBattleActor winner)
        {
            HideAllUI();

            Debug.Log($"Battle Over! {winner.Name} has won!");
            if (winner.Name == _battleSystem.PlayerOne.Name)
            {
                ShowPlayerWinView();
            }

            if (winner.Name == _battleSystem.PlayerTwo.Name)
            {
                ShowEnemyWinView();
            }
        }



        #endregion


        #region UI State Control

        private async Task ShowBeginningOverlay()
        {
            HideAllUI();
            _battleOverlayPanelViewModel.SetVisibility(true);
            
            await Task.Delay(1500); // Pause it
            
            //Setup the battle panels
            _battleOverlayPanelViewModel.SetVisibility(false);
            
            await Task.Delay(1500); // Pause it
            
            ShowBattleActorDataPanels();
            ShowPlayerBattleActionPanel();
            
        }


        /// <summary>
        /// We want to be choosy when this actually updates
        /// </summary>
        private void UpdatePlayerViewModels()
        {
            _playerOnePanelViewModel.UpdateFromBattleActor();
            _playerTwoPanelViewModel.UpdateFromBattleActor();
            _playerOneTokenViewModel.UpdateFromBattleActor();
            _playerTwoTokenViewModel.UpdateFromBattleActor();
        }
        
        // Needs multiple show and hide controls from here
        // A lot of this could be side loaded into a UI manager, but for now it's here.
        // 1. Hide/Show begins panel
        // 2. Hide/Show battle UI
        // 3. Hide/Show action panel
        // 4. Hide/Show Win UI
        // 5. Hide/Show Error UI 
        // 6. Reset and hide *everything*

        private void HideAllUI()
        {
            _playerOnePanelViewModel.SetVisibility(false);
            _playerTwoPanelViewModel.SetVisibility(false);
            _battleOverlayPanelViewModel.SetVisibility(false);
            _errorPanelViewModel.SetVisibility(false);
            _actionPanelViewModel.SetVisibility(false);
            _playerWinViewModel.SetVisibility(false);
            _enemyWinViewModel.SetVisibility(false);
            _playerBattleActionViewModel.SetVisibility(false);
        }

        private void ShowPlayerWinView()
        {
            HideAllUI();
            _playerWinViewModel.SetVisibility(true);
        }

        private void ShowEnemyWinView()
        {
            HideAllUI();
            _enemyWinViewModel.SetVisibility(true);
        }

        private void ShowErrorPanel()
        {
            HideAllBattleUI();
            _errorPanelViewModel.SetVisibility(true);
        }

        private void ShowPlayerBattleActionPanel()
        {            
            _playerOneTokenViewModel.LookAtCamera();
            _playerBattleActionViewModel.SetVisibility(true);
        }
        
        private void HidePlayerBattleActionPanel()
        {
            _playerBattleActionViewModel.SetVisibility(false);
        }

        private void ShowActionPanel(IBattleAction action)
        {
            _actionPanelViewModel.SetVisibility(true);
            _actionPanelViewModel.SetAction(action);
        }

        private void HideActionPanel()
        {
            _actionPanelViewModel.SetVisibility(false);
        }

        private void HideAllBattleUI()
        {
            HideBattleActorUI();
            _actionPanelViewModel.SetVisibility(false);
        }

        private void ShowAllBattleUI()
        {
            ShowBattleActorDataPanels();
            ShowBattleActionSelectionPanel();
        }

        private void ShowBattleActionSelectionPanel()
        {
            _playerBattleActionViewModel.SetVisibility(true);
        }

        private void ShowBattleActorDataPanels()
        {
            _playerOnePanelViewModel.SetVisibility(true);
            _playerTwoPanelViewModel.SetVisibility(true);
        }

        private void HideBattleActorUI()
        {
            _playerOnePanelViewModel.SetVisibility(false);
            _playerTwoPanelViewModel.SetVisibility(false);
            _actionPanelViewModel.SetVisibility(false);
            _playerBattleActionViewModel.SetVisibility(false);
        }

        #endregion

        
        #region Player Token control

        /// <summary>
        /// Extremely hacky. In a better world this would be obtained from the action *data* and not the action itself
        /// </summary>
        /// <param name="action">the action to perform</param>
        private async Task ExecuteVisualEffects(IBattleAction action)
        {
            //Because we have the action, we can compare against the battle system to determine who is attacking and defending
            bool isPlayerOneAttacking = action.Source == _battleSystem.PlayerOne;
            PlayerTokenViewModel attacker = isPlayerOneAttacking ? _playerOneTokenViewModel : _playerTwoTokenViewModel;
            
            BattleActionType actionType = BattleActionType.None;
            
            //determine if it's an attacking type or not
            if (isPlayerOneAttacking)
            {
                actionType = action.Parameters.BattleActionType;
            }
            else
            {
                switch (action.Parameters.MoveName)
                {
                    case "Ink Blink":
                        actionType = BattleActionType.Guard;
                        break;
                    case "Sneak Beak":
                        actionType = BattleActionType.Guard;
                        break;
                    case "Hug":
                        actionType = BattleActionType.Attack;
                        break;
                    case "Self Love":
                        actionType = BattleActionType.Heal;
                        break;
                }
            }

            //Attack fails to do damage, get out now.
            if (actionType == BattleActionType.Attack && action.Target.Guarded)
            {
                attacker.PerformFailureAnimation();
                return;
            }

            attacker.PerformActionAnimation(actionType); //Do attack, guard, or heal

        }
        #endregion
        
        #region I want to change textures I'm sorry

        public void UpdatePlayerTokenWorldMaterial()
        {
            _playerMaterial.SetTexture("_MainTex", _playerData.HighResIcon);
        }

        #endregion
    }
}
