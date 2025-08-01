using Game.ECS.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    /// <summary>
    /// Moves all entities
    /// </summary>
    public partial class EntityMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CMoving>()
                .ForEach((ref LocalTransform localTransform, in CMoving moving) =>
                {
                    localTransform.Position += moving.Speed * SystemAPI.Time.DeltaTime * moving.Direction;
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}