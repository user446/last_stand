using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Scriptable Objects/MovementSettings")]
    public class MovementSettings : ScriptableObject
    {
        public int Speed;
        public float RotationOffsetAngle;
    }
}