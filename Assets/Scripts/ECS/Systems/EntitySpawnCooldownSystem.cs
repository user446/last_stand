using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    public partial class EntitySpawnCooldownSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CSpawnCooldown>()
                .WithNone<CDestroyTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, ref CSpawnCooldown cooldown) =>
                {
                    if ((cooldown.Time -= SystemAPI.Time.DeltaTime) <= 0)
                        ecb.RemoveComponent<CSpawnCooldown>(entity);
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}