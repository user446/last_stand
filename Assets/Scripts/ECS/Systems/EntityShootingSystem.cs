using Game.ECS.Components;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public partial class EntityShootingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CShooter>()
                .WithAll<LocalTransform>()
                .WithAll<CProjectileHolder>()
                .WithAll<CShootingDirection>()
                .WithNone<CShootingCooldown>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity shooter, EntityCommandBuffer ecb, in LocalTransform shooterTransform,
                    in CShooter shooterComponent, in CShootingDirection shootingDirection, in CProjectileHolder projectileHolder) =>
            {
                var projectile = ecb.Instantiate(projectileHolder.ProjectilePrototype);
                ecb.SetComponent(projectile, new LocalTransform()
                {
                    Position = shooterTransform.Position,
                    Rotation = shooterTransform.Rotation,
                    Scale = projectileHolder.Scale
                });
                ecb.AddComponent(projectile, new CMoving()
                {
                    Direction = shootingDirection.Value,
                    Speed = projectileHolder.Speed
                });
                ecb.AddComponent(projectile, new CCollisionDamage() { Value = projectileHolder.Damage });
                ecb.AddComponent(projectile, new CCollider() { Radius = (int)projectileHolder.Scale });
                ecb.AddComponent(projectile, new CLifetime() { Time = projectileHolder.Lifetime });
                ecb.AddComponent(shooter, new CShootingCooldown() { Time = shooterComponent.Cooldown });

                ecb.RemoveComponent<DisableRendering>(projectile);
                ecb.RemoveComponent<CInLimboTag>(projectile);
            }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}