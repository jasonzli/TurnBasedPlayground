using UnityEngine;

namespace Code.ViewScripts
{
    public interface IView
    {
        public void Show();
        public void Hide();
    }

    public abstract class ViewBase : MonoBehaviour, IView
    {
        protected Animator _animator;
        public abstract void Show();
        public abstract void Hide();
    }
    
    [RequireComponent(typeof(Animator))]
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
            _animator.ResetTrigger("Hide");
            _animator.SetTrigger("Show");
        }
    
        public override void Hide()
        {
            _animator.ResetTrigger("Show");
            _animator.SetTrigger("Hide");
        }
    }
}