using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial class ClearCollisionsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CCollision>()
                .WithNone<CInLimboTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb) =>
                {
                    ecb.RemoveComponent<CCollision>(entity);
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}