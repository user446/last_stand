using Game.ECS.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public partial class EnemiesProvideShootingDirections : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CEnemyMobTag>()
                .WithAll<LocalTransform>()
                .WithAll<CInArena>()
                .WithAll<CProjectileHolder>()
                .WithAll<CShooter>()
                .WithNone<CShootingDirection>()
                .WithNone<CInLimboTag>()
                .WithNone<CShootingCooldown>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity enemy, EntityCommandBuffer ecb) =>
            {
                ecb.AddComponent<CShootingDirection>(enemy);
            }).WithBurst().ScheduleParallel();
        }
    }
}