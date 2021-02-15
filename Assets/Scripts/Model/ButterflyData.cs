using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "NewButterflyData", menuName = "Butterfly", order = 0)]
    public class ButterflyData : ScriptableObject
    {
        public int id;
        public Sprite sprite;
    }
}