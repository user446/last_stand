using Unity.Entities;

namespace Game.ECS.Components
{
    public struct CProjectileHolder : IComponentData
    {
        public Entity ProjectilePrototype;
        public float Scale;
        public float Speed;
        public float Damage;
        public float Lifetime;
    }
}