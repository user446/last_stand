using Unity.Entities;

namespace Game.ECS.Components
{
    public struct CShooter : IComponentData
    {
        public float Cooldown;
    }
}