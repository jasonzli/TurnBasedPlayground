using System.Collections.Generic;
using Code.BattleSystem;
using Code.ScriptableObjects;
using UnityEngine;

namespace Code.ViewScripts
{
    public class BattleActionSelectionPanelView : ViewBase
    {

        [SerializeField] private GameObject _actionRowPrefab;
        //private BattleActionSelecrtionViewModel _context;

        [SerializeField] private RectTransform _actionContainer;
        
        private List<GameObject> _actionRows = new List<GameObject>();
        
        private BattleActionSelectionViewModel _context;
        
        public void Initialize(BattleActionSelectionViewModel context)
        {
            _context = context;
            
            _animator = GetComponent<Animator>();
            _animator.ResetTrigger("Show");
            _animator.ResetTrigger("Hide");
            
            //Setup action rows for each action
            foreach (BattleActionData actionData in context.AvailableBattleActionData)
            {
                GameObject actionRow = Instantiate(_actionRowPrefab, _actionContainer);
                BattleActionRowView battleActionRowView = actionRow.GetComponent<BattleActionRowView>();
                battleActionRowView.Initialize(actionData);
                battleActionRowView.ActionButtonObject.onClick.AddListener(() => context.SendBattleActionData(actionData));
                _actionRows.Add(actionRow);
            }
        }

        public override void Show()
        {
            throw new System.NotImplementedException();
        }

        public override void Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}