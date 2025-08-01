using Game.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public partial class EnemiesFollowPlayerSystem : SystemBase
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
                    .WithAll<CMoving>()
                    .WithNone<CInLimboTag>()
                    .ForEach((Entity enemy, ref CMoving moving, in LocalTransform transform) =>
                {
                    var direction = math.normalize(playerPosition - transform.Position);
                    moving.Direction = direction;
                }).WithBurst().ScheduleParallel();
            }
        }
    }
}