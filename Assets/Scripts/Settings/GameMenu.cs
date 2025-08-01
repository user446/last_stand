using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "GameMenuDataSource", menuName = "Scriptable Objects/GameMenuDataSource")]
    public class GameMenuDataSource : ScriptableObject
    {
        public int PlayerHealth;
    }
}