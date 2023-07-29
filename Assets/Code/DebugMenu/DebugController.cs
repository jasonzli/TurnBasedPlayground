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
            _debugViewModel = new DebugViewModel(
            () => { _conductor.ResetBattle(false);},
       () => { _conductor.ResetBattle(true); }, //This is the only way to activate an unsafe battle.
                    _conductor.playerOneData
            );
            _debugView.Initialize(_debugViewModel);
        }

    }
}