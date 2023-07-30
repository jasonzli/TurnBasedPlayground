using System.Collections.Generic;

using Code.ScriptableObjects;
using Code.Utility;
using Code.ViewModels;
using UnityEngine;

namespace Code.ViewScripts
{
    /// <summary>
    /// A container for the rows of actions that a player can take
    /// Should also handle being "active" or not on its own
    /// </summary>
    public class BattleActionSelectionPanelView : ViewBase
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _actionRowPrefab;
        [SerializeField] private RectTransform _actionContainer;
        
        private List<GameObject> _actionRows = new List<GameObject>();
        private bool _isUnsafeBattle = false;
        private BattleActionSelectionViewModel _context;
        
        public void Initialize(BattleActionSelectionViewModel context)
        {
            _context = context;
            gameObject.SetActive(true);

            _isUnsafeBattle = context.UnsafeBattle;
            
            _context.Visibility.PropertyChanged += SetVisibility;
            _context.UnsafeBattle.PropertyChanged += SetUnsafeBattle;
            
            //Clean up old action rows
            foreach(GameObject row in _actionRows)
            {
                Destroy(row);
            }
            
            //Setup action rows for each action
            foreach (BattleActionData actionData in context.AvailableBattleActionData)
            {
                GameObject actionRow = Instantiate(_actionRowPrefab, _actionContainer);
                BattleActionRowView battleActionRowView = actionRow.GetComponent<BattleActionRowView>();
                battleActionRowView.Initialize(
                    actionData, 
                    _isUnsafeBattle ?  actionData.AsBattleActionParameters() : actionData.AsSafeBattleActionParameters(),
                    () => { context.SendActionToConductor(actionData); });
                _actionRows.Add(actionRow);
            }
            
            ResetTriggers();
            SetVisibility(context.Visibility);
        }

        private void SetUnsafeBattle(bool isUnsafeBattle)
        {
            _isUnsafeBattle = isUnsafeBattle;
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