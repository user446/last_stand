using Game.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public partial class EnemiesShootPlayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (SystemAPI.HasSingleton<CPlayerMainTag>())
            {
                var playerPosition = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingletonEntity<CPlayerMainTag>()).ValueRO.Position;
                Entities
                    .WithAll<CEnemyMobTag>()
                    .WithAll<LocalTransform>()
                    .WithAll<CInArena>()
                    .WithAll<CProjectileHolder>()
                    .WithAll<CShooter>()
                    .WithAll<CShootingDirection>()
                    .WithNone<CInLimboTag>()
                    .WithNone<CShootingCooldown>()
                    .ForEach((Entity enemy, ref CShootingDirection shootingDirection, in CProjectileHolder holder, in LocalTransform transform) =>
                {
                    var direction = math.normalize(playerPosition - transform.Position);
                    shootingDirection.Value = direction;
                }).WithBurst().ScheduleParallel();
            }
        }
    }
}