using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "ShootingSettings", menuName = "Scriptable Objects/ShootingSettings")]
    public class ShootingSettings : ScriptableObject
    {
        public float Cooldown;
    }
}