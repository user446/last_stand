using Unity.Entities;
using Unity.Mathematics;

namespace Game.ECS.Components
{
    public struct CShootingDirection : IComponentData
    {
        public float3 Value;
    }
}