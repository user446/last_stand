using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    public partial class DamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CDamage>()
                .WithAll<CHealth>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, ref CHealth health, in CDamage damage) =>
                {
                    health.Value -= damage.Value;
                    ecb.RemoveComponent<CDamage>(entity);
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}