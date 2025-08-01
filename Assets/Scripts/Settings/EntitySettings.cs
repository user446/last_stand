using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "EntitySettings", menuName = "Scriptable Objects/EntitySettings")]
    public class EntitySettings : ScriptableObject
    {
        public VisualSettings VisualSettings;
        public HealthSettings HealthSettings;
        public MovementSettings MovementSettings;
        public ShootingSettings ShootingSettings;
        public ProjectileSettings ProjectileSettings;
        public InputControlSettings InputControlSettings;
    }
}