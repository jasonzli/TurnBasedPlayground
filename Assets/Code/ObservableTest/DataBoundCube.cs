using UnityEngine;

namespace ObservableTest
{
    public class DataBoundCube : MonoBehaviour
    {
        private CubeLogic cubeLogic;
        public TextFieldUIView textFieldUIView;

        public void Start()
        {
            cubeLogic = new CubeLogic();
            
            cubeLogic.Initialize();
            textFieldUIView.Initialize(cubeLogic.itemDataSource.position);
            
            cubeLogic.itemDataSource.position.AddListener(OnPositionChanged);
        }
        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            cubeLogic.itemDataSource.position.AddListener(OnPositionChanged);
        }

        public void FixedUpdate()
        {
            cubeLogic.Update(Time.time);
        }
        
        private void OnPositionChanged(float newPosition)
        {
            transform.position = new Vector3(newPosition, 0, 0);
        }


    }
}