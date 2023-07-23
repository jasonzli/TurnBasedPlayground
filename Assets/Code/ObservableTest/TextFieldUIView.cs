using TMPro;
using UnityEngine;

namespace ObservableTest
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TextFieldUIView : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField inputField;
        private Observable<float> distanceContext;
        
        void OnEnable()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        public void Initialize(Observable<float> distanceContext)
        {
            inputField.onValueChanged.AddListener(UpdateBinding);
            this.distanceContext = distanceContext;
            distanceContext.AddListener(OnValueUpdated);
        }

        private void UpdateBinding(string inputText)
        {
            float.TryParse(inputText, out float distance);
            distanceContext.Value = distance;
        }
        
        private void OnValueUpdated(float value)
        {
            inputField.text = value.ToString();
        }

        void OnDisable()
        {
            distanceContext.RemoveListener(OnValueUpdated);
        }
    }
}
