using System.Threading.Tasks;
using UnityEngine;

namespace Code.ViewScripts
{
    /// <summary>
    /// Not technically a view, but a control script purely for animation the health bar
    /// </summary>
    public class HealthBarUpdater : MonoBehaviour
    {
        [SerializeField] private RectTransform _healthBarFill;
        [SerializeField] private RectTransform _healthBarParent;

        private float animationTime = .4f;

        public async Task UpdateHealthBarFill(float percentToFill)
        {
            float elapsedTime = 0f;
            float startFill = _healthBarFill.sizeDelta.x;
            float endFill = _healthBarParent.rect.width * percentToFill;
            while (elapsedTime < animationTime)
            {
                float t = elapsedTime / animationTime;
                float newDelta = Mathf.Lerp(startFill,endFill,t * t * t);
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            
                _healthBarFill.sizeDelta = new Vector2(newDelta, _healthBarFill.sizeDelta.y);
            }
            _healthBarFill.sizeDelta = new Vector2(endFill, _healthBarFill.sizeDelta.y);
        }
    }
}
