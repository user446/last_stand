using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "ArenaSettings", menuName = "Scriptable Objects/ArenaSettings")]
    public class ArenaSettings : ScriptableObject
    {
        public Mesh WallMesh;
        public Material WallMaterial;
        public float VisualScale;        public float Radius;
    }
}