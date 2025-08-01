using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    [UpdateBefore(typeof(EntityDisposalSystem))]
    public partial class EntityLifetimeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CLifetime>()
                .WithNone<CDestroyTag>()
                .WithNone<CInLimboTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, ref CLifetime lifetime) =>
                {
                    if ((lifetime.Time -= SystemAPI.Time.DeltaTime) <= 0)
                        ecb.AddComponent<CDestroyTag>(entity);
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}