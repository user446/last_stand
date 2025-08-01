using Game.ECS.Components;
using Game.ECS.Utils;
using Game.Settings;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.ECS.Systems
{
    public partial class EnemiesSpawnSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            var enemySettings = Addressables.LoadAssetAsync<EntitySettings>("EnemySettings").WaitForCompletion();
            var gameSettings = Addressables.LoadAssetAsync<GameSettings>("GameSettings").WaitForCompletion();
            var enemyPrototype = CreateEnemy(enemySettings);
            var projPrototype = CreateProjectilePrototype(enemySettings.ProjectileSettings);

            CreateSpawner(enemyPrototype, projPrototype, gameSettings.EnemySpawnSettings.DefaultPosition,
                gameSettings.EnemySpawnSettings.WaveCooldown, gameSettings.EnemySpawnSettings.WaveQuantity);
        }

        protected override void OnUpdate()
        {
            if (SystemAPI.HasSingleton<CGameplaySingleton>())
            {
                var enemyLookup = SystemAPI.GetComponentLookup<CEnemyTag>(isReadOnly: true);
                var playerLookup = SystemAPI.GetComponentLookup<CPlayerTag>(isReadOnly: true);
                enemyLookup.Update(this);
                playerLookup.Update(this);

                Entities
                    .WithAll<CEnemySpawner>()
                    .WithAll<LocalTransform>()
                    .WithNone<CSpawnCooldown>()
                    .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                    .ForEach((Entity spawner, int entityInQueryIndex, EntityCommandBuffer ecb, in LocalTransform spawnerTransform,
                        in CEnemySpawner enemySpawner) =>
                {
                    //spawn enemies
                    var rnd = Unity.Mathematics.Random.CreateFromIndex((uint)entityInQueryIndex);

                    for (int i = 0; i < enemySpawner.SpawnCount; i++)
                    {
                        var enemyEntity = ecb.Instantiate(enemySpawner.EnemyPrototype);

                        //let them just move up after spawn
                        ecb.AddComponent(enemyEntity, new CMoving()
                        {
                            Direction = Vector3.up,
                            Speed = 5
                        });
                        ecb.SetComponent(enemyEntity, new LocalTransform()
                        {
                            Position = spawnerTransform.Position + rnd.NextFloat3(new float3(0, 0, 0), new float3(3, 3, 0)),
                            Scale = enemySpawner.EnemyScale
                        });
                        ecb.RemoveComponent<DisableRendering>(enemyEntity);
                        ecb.RemoveComponent<CInLimboTag>(enemyEntity);
                    }
                    //cooldown after spawn
                    ecb.AddComponent(spawner, new CSpawnCooldown()
                    {
                        Time = enemySpawner.Cooldown
                    });
                }).WithBurst(synchronousCompilation: true).ScheduleParallel();
            }
        }

        private Entity CreateSpawner(Entity enemyPrototype, Entity projectilePrototype, float3 position,
            float cooldown, int spawnCount)
        {
            var entityManager = World.EntityManager;
            var spawnerEntity = entityManager.CreateEntity(
                typeof(LocalToWorld), typeof(LocalTransform),
                typeof(CEnemySpawner), typeof(CEnemyTag), typeof(CEnemyMobTag),
                typeof(DisableRendering), 
                typeof(CInLimboTag));

            entityManager.SetName(spawnerEntity, "Spawner");
            entityManager.SetComponentData(spawnerEntity, new CEnemySpawner()
            {
                Cooldown = cooldown,
                SpawnCount = spawnCount,
                EnemyScale = entityManager.GetComponentData<LocalTransform>(enemyPrototype).Scale,
                EnemyPrototype = enemyPrototype,
                EnemyProjectilePrototype = projectilePrototype
            });

            entityManager.AddComponentData(spawnerEntity, new LocalTransform()
            {
                Position = position
            });
            return spawnerEntity;
        }

        private Entity CreateEnemy(EntitySettings enemySettings)
        {
            var entityManager = World.EntityManager;

            var enemyEntity = entityManager.CreateEntity(
                typeof(CHealth),
                typeof(CEnemyTag),
                typeof(CEnemyMobTag),
                typeof(CCollisionDamage),
                typeof(CCollider),
                typeof(CIgnoreBoundsTag),
                typeof(CProjectileHolder),
                typeof(CShooter),
                typeof(CInLimboTag));

            entityManager.SetName(enemyEntity, $"Enemy({enemyEntity.Index},{enemyEntity.Version})");
            entityManager.AddVisuals(enemyEntity, enemySettings.VisualSettings.Material, enemySettings.VisualSettings.Mesh);
            entityManager.AddComponent<DisableRendering>(enemyEntity);
            entityManager.AddComponentData(enemyEntity, new LocalToWorld() { Value = Matrix4x4.identity });
            entityManager.AddComponentData(enemyEntity, new LocalTransform()
            {
                Rotation = Quaternion.identity,
                Scale = enemySettings.VisualSettings.Scale,
                Position = new float3(100, 100, 0)
            });

            entityManager.SetComponentData(enemyEntity, new CHealth() { Value = enemySettings.HealthSettings.Health });
            entityManager.SetComponentData(enemyEntity, new CProjectileHolder()
            {
                ProjectilePrototype = CreateProjectilePrototype(enemySettings.ProjectileSettings),
                Scale = enemySettings.ProjectileSettings.Scale,
                Speed = enemySettings.ProjectileSettings.Speed,
                Damage = enemySettings.ProjectileSettings.Damage,
                Lifetime = enemySettings.ProjectileSettings.Lifetime
            });
            entityManager.SetComponentData(enemyEntity, new CShooter()
            {
                Cooldown = enemySettings.ShootingSettings.Cooldown
            });
            entityManager.SetComponentData(enemyEntity, new CCollider()
            {
                Radius = (int)enemySettings.VisualSettings.Scale
            });

            return enemyEntity;
        }

        private Entity CreateProjectilePrototype(ProjectileSettings projectileSettings)
        {
            var entityManager = World.EntityManager;

            var projectileEntity = entityManager.CreateEntity(
                typeof(CEnemyTag),
                typeof(CCollider),
                typeof(DisableRendering),
                typeof(CInLimboTag),
                typeof(LocalTransform));

            entityManager.SetName(projectileEntity, $"EPP({projectileEntity.Index},{projectileEntity.Version})");

            entityManager.AddVisuals(projectileEntity, projectileSettings.Material, projectileSettings.Mesh);
            entityManager.AddComponentData(projectileEntity, new LocalToWorld() { Value = Matrix4x4.identity });
            return projectileEntity;
        }
    }
}