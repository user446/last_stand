using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "ProjectileSettings", menuName = "Scriptable Objects/ProjectileSettings")]
    public class ProjectileSettings : ScriptableObject
    {
        public Mesh Mesh;
        public Material Material;
        public float Scale;
        public float Damage;
        public float Speed;
        public float Lifetime;
    }
}