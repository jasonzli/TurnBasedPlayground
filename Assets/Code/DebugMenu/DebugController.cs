using Code.BattleSystem;
using UnityEngine;

namespace Code.DebugMenu
{
    /// <summary>
    /// Monobehavior layer for setting up the debug menu.
    /// Has a dependency on needing to have a conductor to actually reference.
    /// </summary>
    public class DebugController : MonoBehaviour
    {
        private DebugViewModel _debugViewModel;
        [SerializeField] private DebugView _debugView;
        [SerializeField] private BattleConductor _conductor;
        
        public void Start()
        {
            _debugViewModel = new DebugViewModel(
            () => { _conductor.ResetBattle(false);},
       () => { _conductor.ResetBattle(true); }, //This is the only way to activate an unsafe battle.
                    _conductor.playerOneData
            );
            _debugView.Initialize(_debugViewModel);
        }

    }
}