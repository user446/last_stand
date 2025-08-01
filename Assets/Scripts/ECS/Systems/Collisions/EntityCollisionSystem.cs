using Game.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(EntityMappingSystem))]
    public partial class EntityCollisionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var enemyLookup = SystemAPI.GetComponentLookup<CEnemyTag>(isReadOnly: true);
            var playerLookup = SystemAPI.GetComponentLookup<CPlayerTag>(isReadOnly: true);
            var collisionLookup = SystemAPI.GetComponentLookup<CCollision>(isReadOnly: true);
            var entityGridRO = SystemAPI.GetSingleton<CEntityMapSingleton>().EntityMap.AsReadOnly();

            enemyLookup.Update(this);
            playerLookup.Update(this);
            collisionLookup.Update(this);

            //simple way to make collisions
            //better one would be using IBufferElementData and separate entity with it

            Entities
                .WithAll<LocalTransform>()
                .WithAll<CCollider>()
                .WithAll<CGridCell>()
                .WithChangeFilter<LocalTransform>()
                .WithChangeFilter<CGridCell>()
                .WithNone<CInLimboTag>()
                .WithNone<CDestroyTag>()
                .WithNone<CCollision>()
                .WithReadOnly(entityGridRO)
                .WithReadOnly(enemyLookup)
                .WithReadOnly(playerLookup)
                .WithReadOnly(collisionLookup)
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, in CGridCell gridCell, in CCollider collider, in LocalTransform localTransform) =>
                {
                    var position = localTransform.Position;
                    for (int dx = -collider.Radius; dx <= collider.Radius; dx++)
                    {
                        for (int dy = -collider.Radius; dy <= collider.Radius; dy++)
                        {
                            var cell = new GridCell(gridCell.Value.x + dx, gridCell.Value.y + dy);
                            if (entityGridRO.TryGetFirstValue(cell, out var otherEntity, out var iterator))
                            {
                                do
                                {
                                    if (!entity.Equals(otherEntity) && !collisionLookup.HasComponent(otherEntity) &&
                                        ((enemyLookup.HasComponent(otherEntity) && playerLookup.HasComponent(entity)) ||
                                        (enemyLookup.HasComponent(entity) && playerLookup.HasComponent(otherEntity))))
                                    {
                                        ecb.AddComponent(entity, new CCollision()
                                        {
                                            Other = otherEntity,
                                        });
                                        ecb.AddComponent(otherEntity, new CCollision()
                                        {
                                            Other = entity
                                        });
                                    }
                                }
                                while (entityGridRO.TryGetNextValue(out otherEntity, ref iterator));
                            }
                        }
                    }
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}