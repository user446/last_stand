using Game.ECS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial class EntityMappingSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            var entityMapSingleton = EntityManager.CreateEntity(typeof(CEntityMapSingleton));
            EntityManager.AddComponentData(entityMapSingleton, new CEntityMapSingleton()
            {
                EntityMap = new(1024, Allocator.Domain)
            });
            EntityManager.SetName(entityMapSingleton, "EntityMap");
        }

        protected override void OnUpdate()
        {
            var entityMap = SystemAPI.GetSingleton<CEntityMapSingleton>().EntityMap;
            entityMap.Clear();

            var entityMapParallelWriter = entityMap.AsParallelWriter();

            Entities
                .WithAll<LocalTransform>()
                .WithChangeFilter<LocalTransform>()
                .WithAll<CCollider>()
                .WithNone<CDestroyTag>()
                .WithNone<CInLimboTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, ref LocalTransform localTransform) =>
                {
                    entityMapParallelWriter.Add(localTransform.Position, entity);
                    ecb.AddComponent(entity, new CGridCell()
                    {
                        Value = localTransform.Position
                    });
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}