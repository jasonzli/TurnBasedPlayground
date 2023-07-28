using UnityEngine;

namespace Code.ViewScripts
{

    public class BattlePanelView : ViewBase
    {
        //Serialized fields for the player 1, player 2 and center stubs
        [SerializeField] private RectTransform _player1Stub;
        [SerializeField] private RectTransform _player2Stub;
        [SerializeField] private RectTransform _centerStub;

        public override void Show()
        {
            gameObject.SetActive(true);
        }
    
        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}