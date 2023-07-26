using UnityEngine;

namespace Code
{
    public interface IView
    {
        public void Show();
        public void Hide();
    }


    public abstract class ViewBase : MonoBehaviour, IView
    {
        private Animator _animator;
        private static readonly int ShowCode = Animator.StringToHash("Show");
        private static readonly int HideCode = Animator.StringToHash("Hide");

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
        }
    
    
        public void Show()
        {
            _animator.ResetTrigger(HideCode);
            _animator.SetTrigger(ShowCode);
        }

        public void Hide()
        {
            _animator.ResetTrigger(ShowCode);
            _animator.SetTrigger(HideCode);
        }
    }
    [RequireComponent(typeof(Animator))]
    public class BattlePanelView : ViewBase
    {
        //Serialized fields for the player 1, player 2 and center stubs
        [SerializeField] private RectTransform _player1Stub;
        [SerializeField] private RectTransform _player2Stub;
        [SerializeField] private RectTransform _centerStub;
    
        private Animator _animator;

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
    
        public void Show()
        {
            _animator.ResetTrigger("Hide");
            _animator.SetTrigger("Show");
        }
    
        public void Hide()
        {
            _animator.ResetTrigger("Show");
            _animator.SetTrigger("Hide");
        }
    }
}