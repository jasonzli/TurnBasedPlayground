using UnityEngine;

namespace Code
{
    [CreateAssetMenu(fileName = "ActorData", menuName = "Create new Actor Data", order = 0)]
    public class ActorData : ScriptableObject
    {
        public string Name;
        public int Health;
        public bool UseURLBrain;
        public string URL;
    }
}