using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    public partial class ShootingCooldownSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CShootingCooldown>()
                .WithAll<CProjectileHolder>()
                .WithAll<CShooter>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, ref CShootingCooldown cooldown) =>
            {
                if ((cooldown.Time -= SystemAPI.Time.DeltaTime) <= 0)
                    ecb.RemoveComponent<CShootingCooldown>(entity);
            }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}