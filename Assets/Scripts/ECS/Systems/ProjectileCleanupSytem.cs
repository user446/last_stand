using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    public partial class ProjectileCleanupSytem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CMoving>()
                .WithAll<CProjectileTag>()
                .WithNone<CInLimboTag>()
                .WithNone<CDestroyTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, ref CMoving moving) =>
            {
                if (moving.Speed == 0)
                    ecb.AddComponent<CDestroyTag>(entity);
            }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}