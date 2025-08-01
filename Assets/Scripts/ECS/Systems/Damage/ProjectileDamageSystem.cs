using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    public partial class ProjectileDamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var enemyLookup = SystemAPI.GetComponentLookup<CEnemyTag>(isReadOnly: true);
            var playerLookup = SystemAPI.GetComponentLookup<CPlayerTag>(isReadOnly: true);
            var healthLookup = SystemAPI.GetComponentLookup<CHealth>(isReadOnly: true);
            var projectileLookup = SystemAPI.GetComponentLookup<CProjectileTag>(isReadOnly: true);
            enemyLookup.Update(this);
            playerLookup.Update(this);
            healthLookup.Update(this);
            projectileLookup.Update(this);

            Entities
                .WithAll<CProjectileTag>()
                .WithAll<CCollision>()
                .WithAll<CCollisionDamage>()
                .WithNone<CDestroyTag>()
                .WithReadOnly(enemyLookup)
                .WithReadOnly(playerLookup)
                .WithReadOnly(healthLookup)
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, in CCollision collision,
                    in CCollisionDamage collisionDamage) =>
                {
                    if (!projectileLookup.HasComponent(collision.Other) &&
                            ((enemyLookup.HasComponent(entity) && playerLookup.HasComponent(collision.Other)) ||
                            (playerLookup.HasComponent(entity) && enemyLookup.HasComponent(collision.Other)))
                        && healthLookup.HasComponent(collision.Other))
                    {
                        ecb.AddComponent(collision.Other, new CDamage()
                        {
                            Value = collisionDamage.Value
                        });
                        ecb.AddComponent<CDestroyTag>(entity);
                    }
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}