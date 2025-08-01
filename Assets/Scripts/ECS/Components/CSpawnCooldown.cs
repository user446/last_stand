using Unity.Entities;

namespace Game.ECS.Components
{
    public struct CSpawnCooldown : IComponentData
    {
        public float Time;
    }
}