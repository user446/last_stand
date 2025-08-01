using Unity.Mathematics;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "EnemySpawnSettings", menuName = "Scriptable Objects/EnemySpawnSettings")]
    public class EnemySpawnSettings : ScriptableObject
    {
        public float3 DefaultPosition;
        public float WaveCooldown;
        public int WaveQuantity;
    }
}