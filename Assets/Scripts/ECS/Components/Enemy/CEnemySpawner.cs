using Unity.Entities;

namespace Game.ECS.Components
{
    public struct CEnemySpawner : IComponentData
    {
        public Entity EnemyPrototype;
        public Entity EnemyProjectilePrototype;
        public float Cooldown;
        public float EnemyScale;
        public int SpawnCount;
    }
}