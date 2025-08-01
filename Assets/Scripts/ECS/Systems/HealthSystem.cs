using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    public partial class HealthSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CHealth>()
                .WithNone<CPlayerMainTag>()
                .WithNone<CDestroyTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, in CHealth health) =>
                {
                    //here could've been also a health regen and game over event
                    if (health.Value <= 0)
                        ecb.AddComponent<CDestroyTag>(entity);
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}