using Game.ECS.Components;
using Unity.Entities;

namespace Game.ECS.Systems
{
    public partial class CGameOverSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CPlayerMainTag>()
                .WithAll<CHealth>()
                .WithNone<CDestroyTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb, in CHealth health) =>
                {
                    if (health.Value <= 0)
                    {
                        var gameOver = ecb.CreateEntity();
                        ecb.AddComponent<CGameOverSingleton>(gameOver);
                        ecb.AddComponent<CDestroyTag>(entity);
                    }
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
        }
    }
}