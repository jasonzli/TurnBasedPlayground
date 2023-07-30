using Code.ViewModels;
using UnityEngine;

namespace Code.ViewScripts
{
    /// <summary>
    /// All views should have the basics for turning on and off
    /// If this were a more traditional form of MVVM, the VM would have the IViewModel to send the object through the
    /// OnPropertyChanged event, which would enable this inheritance to be stronger.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public abstract class ViewBase : MonoBehaviour
    {
        public virtual void Show() {gameObject.SetActive(true);}
        public virtual void Hide() {gameObject.SetActive(false);}

        public virtual void SetVisibility(bool visibility)
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