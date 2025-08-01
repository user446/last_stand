using Game.ECS.Components;
using Game.ECS.Utils;
using Game.Settings;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.ECS.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class PlayerSpawnSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
            if (!SystemAPI.HasSingleton<CPlayerMainTag>()
                && SystemAPI.HasSingleton<CGameplaySingleton>()
                && !SystemAPI.HasSingleton<CGameOverSingleton>())
            {
                var playerSettingsHandle = Addressables.LoadAssetAsync<EntitySettings>("PlayerSettings");
                var playerSettings = playerSettingsHandle.WaitForCompletion();
                CreatePlayer(playerSettings);
            }
        }

        private void CreatePlayer(EntitySettings playerSettings)
        {
            var entityManager = World.EntityManager;

            var playerEntity = entityManager.CreateEntity(
                typeof(CHealth),
                typeof(CPlayerTag),
                typeof(CPlayerMainTag),
                typeof(CPlayerControlTag),
                typeof(CProjectileHolder),
                typeof(CCollider),
                typeof(CShooter),
                typeof(CShootingDirection),
                typeof(CInArena));

            entityManager.SetName(playerEntity, "Player");
            entityManager.AddVisuals(playerEntity, playerSettings.VisualSettings.Material, playerSettings.VisualSettings.Mesh);
            entityManager.AddComponentData(playerEntity, new LocalToWorld() { Value = Matrix4x4.identity });
            entityManager.AddComponentData(playerEntity, new LocalTransform()
            {
                Rotation = Quaternion.identity,
                Scale = playerSettings.VisualSettings.Scale
            });

            entityManager.SetComponentData(playerEntity, new CHealth() { Value = playerSettings.HealthSettings.Health });
            entityManager.SetComponentData(playerEntity, new CProjectileHolder()
            {
                ProjectilePrototype = CreateProjectilePrototype(playerSettings.ProjectileSettings),
                Scale = playerSettings.ProjectileSettings.Scale,
                Speed = playerSettings.ProjectileSettings.Speed,
                Damage = playerSettings.ProjectileSettings.Damage,
                Lifetime = playerSettings.ProjectileSettings.Lifetime
            });
            entityManager.SetComponentData(playerEntity, new CShooter()
            {
                Cooldown = playerSettings.ShootingSettings.Cooldown
            });
            entityManager.AddComponent<CShootingDirection>(playerEntity);
            entityManager.SetComponentData(playerEntity, new CCollider()
            {
                Radius = (int)playerSettings.VisualSettings.Scale
            });
        }

        private Entity CreateProjectilePrototype(ProjectileSettings projectileSettings)
        {
            var entityManager = World.EntityManager;

            var projectileEntity = entityManager.CreateEntity(
                typeof(CPlayerTag),
                typeof(CCollider),
                typeof(DisableRendering),
                typeof(CInLimboTag),
                typeof(LocalTransform),
                typeof(CInArena));

            entityManager.SetName(projectileEntity, $"PPP({projectileEntity.Index},{projectileEntity.Version})");

            entityManager.AddVisuals(projectileEntity, projectileSettings.Material, projectileSettings.Mesh);
            entityManager.AddComponentData(projectileEntity, new LocalToWorld() { Value = Matrix4x4.identity });
            return projectileEntity;
        }
    }
}