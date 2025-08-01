using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public EnemySpawnSettings EnemySpawnSettings;
        public ArenaSettings ArenaSettings;
    }
}