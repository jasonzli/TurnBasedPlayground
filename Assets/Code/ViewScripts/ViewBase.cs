using UnityEngine;

namespace Code.ViewScripts
{
    [RequireComponent(typeof(Animator))]
    public abstract class ViewBase : MonoBehaviour, IView
    {
        protected Animator _animator;
        
        public abstract void Show();
        public abstract void Hide();

        protected virtual void SetVisibility(bool visibility)
        {
            if (visibility)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}