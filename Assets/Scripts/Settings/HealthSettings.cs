using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "HealthSettings", menuName = "Scriptable Objects/HealthSettings")]
    public class HealthSettings : ScriptableObject
    {
        public float Health;
    }
}