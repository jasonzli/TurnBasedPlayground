using System;
using Random = UnityEngine.Random;

namespace ObservableTest
{
    public class CubeLogic
    {
        public ItemDataSource itemDataSource { get; set; }
        private float nextItemUpdateTime = 0f;

        public void Initialize()
        {
            itemDataSource = new ItemDataSource(0);
            nextItemUpdateTime = 2f;
        }

        public void Update(float simulationTime)
        {
            if (nextItemUpdateTime > simulationTime) return;
            
            nextItemUpdateTime += 2f;
            itemDataSource.position.Value = MathF.Round( Random.value * 5f);
        }
        
    }
}