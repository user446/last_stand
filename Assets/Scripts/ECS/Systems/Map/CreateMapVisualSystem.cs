using Game.ECS.Utils;
using Game.Settings;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.ECS.Systems
{
    public partial class CreatMapVisualSystem : SystemBase
    {
        protected override void OnCreate()
        {
            var gameSettingsHandle = Addressables.LoadAssetAsync<GameSettings>("GameSettings");
            var gameSettings = gameSettingsHandle.WaitForCompletion();
            CreateArenaEntity(float3.zero, gameSettings.ArenaSettings.VisualScale, gameSettings.ArenaSettings.WallMesh, gameSettings.ArenaSettings.WallMaterial);
        }

        protected override void OnUpdate()
        {       
        }

        private Entity CreateArenaEntity(float3 position, float scale, Mesh mesh, Material material)
        {
            var entityManager = World.EntityManager;

            var wallEntity = entityManager.CreateEntity();

            entityManager.SetName(wallEntity, "Arena");
            entityManager.AddVisuals(wallEntity, material, mesh);
            entityManager.AddComponentData(wallEntity, new LocalToWorld() { Value = Matrix4x4.identity });
            entityManager.AddComponentData(wallEntity, new LocalTransform()
            {
                Rotation = Quaternion.identity,
                Scale = scale,
                Position = position
            });
            return wallEntity;
        }
    }
}