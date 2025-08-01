using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.ECS.Components
{
    public struct CProjectileSetup : IComponentData
    {
        public UnityObjectRef<Material> Material;
        public float Damage;
        public float Scale;
        public float3 Direction;
        public float Speed;
    }
}