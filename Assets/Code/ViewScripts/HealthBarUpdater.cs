using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.ViewScripts
{
    /// <summary>
    /// Not technically a view, but a control script purely for animation the health bar and local tooltip control
    /// </summary>
    public class HealthBarUpdater : MonoBehaviour
    {
        [SerializeField] private RectTransform _healthBarFill;
        [SerializeField] private RectTransform _healthBarParent;
        [SerializeField] private EventTrigger _tooltipEventTrigger;
        [SerializeField] private Animator _animator;

        void OnEnable()
        {
            _animator = GetComponent<Animator>();
            ResetTriggers();
            
            //Clear Event triggers
            //At this point, by convention, 0 is enter, 1 is exit
            _tooltipEventTrigger.triggers[0].callback.RemoveAllListeners();
            _tooltipEventTrigger.triggers[1].callback.RemoveAllListeners();
            
            //Set up animations
            _tooltipEventTrigger.triggers[0].callback.AddListener((data) =>
            {
                ResetTriggers();
                _animator.SetTrigger("Show");
            });
            _tooltipEventTrigger.triggers[1].callback.AddListener((data) =>
            {
                ResetTriggers();
                _animator.SetTrigger("Hide");
            });
        }
        
    
        private void ResetTriggers()
        {
            _animator.ResetTrigger("Show");
            _animator.ResetTrigger("Hide");
        }
        
        public void Show()
        {
            ResetTriggers();
            _animator.SetTrigger("Show");
        }
        
        public void Hide()
        {
            ResetTriggers();
            _animator.SetTrigger("Hide");
        }

        
        private float healthBarAnimationTime = .4f;
        public async Task UpdateHealthBarFill(float percentToFill)
        {
            float elapsedTime = 0f;
            float startFill = _healthBarFill.sizeDelta.x;
            float endFill = _healthBarParent.rect.width * percentToFill;
            while (elapsedTime < healthBarAnimationTime)
            {
                float t = elapsedTime / healthBarAnimationTime;
                float newDelta = Mathf.Lerp(startFill,endFill,t * t * t);
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            
                _healthBarFill.sizeDelta = new Vector2(newDelta, _healthBarFill.sizeDelta.y);
            }
            _healthBarFill.sizeDelta = new Vector2(endFill, _healthBarFill.sizeDelta.y);
        }
    }
}
