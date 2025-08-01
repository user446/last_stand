using Game.ECS.Components;
using Game.Settings;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.AddressableAssets;

namespace Game.ECS.Systems
{
    public partial class CMapBoundsSystem : SystemBase
    {
        private GameSettings gameSettings;

        protected override void OnCreate()
        {
            base.OnCreate();
            var gameSettingsHandle = Addressables.LoadAssetAsync<GameSettings>("GameSettings");
            gameSettings = gameSettingsHandle.WaitForCompletion();
        }

        protected override void OnUpdate()
        {
            var radius = gameSettings.ArenaSettings.Radius;
            var ignoreLookup = GetComponentLookup<CIgnoreBoundsTag>(isReadOnly: true);

            Entities
                .WithAll<LocalTransform>()
                .WithAll<CMoving>()
                .WithReadOnly(ignoreLookup)
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, ref LocalTransform transform, ref CMoving moving) =>
                {
                    var distance = math.distance(float3.zero, transform.Position);
                    if (distance > radius)
                    {
                        if (!ignoreLookup.HasComponent(entity))
                        {
                            transform.Position *= radius / distance;
                            moving.Speed = 0;
                        }
                        ecb.RemoveComponent<CInArena>(entity);
                    }
                    else
                        ecb.AddComponent<CInArena>(entity);
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}