using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "VisualSettings", menuName = "Scriptable Objects/VisualSettings")]
    public class VisualSettings : ScriptableObject
    {
        public Mesh Mesh;
        public Material Material;
        public float Scale;
    }
}