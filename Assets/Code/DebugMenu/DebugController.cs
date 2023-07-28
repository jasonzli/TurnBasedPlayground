using Code.BattleSystem;
using UnityEngine;

namespace Code.DebugMenu
{
    public class DebugController : MonoBehaviour
    {
        private DebugViewModel _debugViewModel;
        [SerializeField] private DebugView _debugView;
        [SerializeField] private BattleConductor _conductor;
        
        public void Start()
        {
            _debugViewModel = new DebugViewModel(() =>
            {
                _conductor.ResetBattle();
            }, _conductor.playerOneData);
            
            _debugView.Initialize(_debugViewModel);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.P))
            {
                _debugViewModel.OpenDebugMenu();
            }
        }
    }
}