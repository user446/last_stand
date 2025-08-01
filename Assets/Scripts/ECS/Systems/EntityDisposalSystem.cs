using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    [UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial class EntityDisposalSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CDestroyTag>()
                .WithNone<CInLimboTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb) =>
                {
                    ecb.DestroyEntity(entity);
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}