using Unity.Entities;

namespace Game.ECS.Components
{
    public struct CCollision : IComponentData
    {
        public Entity Other;
    }
}