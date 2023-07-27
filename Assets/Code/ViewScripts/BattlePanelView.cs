using UnityEngine;

namespace Code.ViewScripts
{

    public class BattlePanelView : ViewBase
    {
        //Serialized fields for the player 1, player 2 and center stubs
        [SerializeField] private RectTransform _player1Stub;
        [SerializeField] private RectTransform _player2Stub;
        [SerializeField] private RectTransform _centerStub;

        private void Initialize()
        {
            _animator = GetComponent<Animator>();
            _animator.ResetTrigger("Show");
            _animator.ResetTrigger("Hide");
        }
    
        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
        }
    
        public override void Show()
        {
            
            gameObject.SetActive(true);
            _animator.ResetTrigger("Hide");
            _animator.SetTrigger("Show");
        }
    
        public override void Hide()
        {
            
            gameObject.SetActive(false);
            _animator.ResetTrigger("Show");
            _animator.SetTrigger("Hide");
        }
    }
}