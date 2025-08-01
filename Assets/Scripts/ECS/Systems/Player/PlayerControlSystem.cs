using Game.ECS.Components;
using Game.Settings;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Game.ECS.Systems
{
    public partial class PlayerControlSystem : SystemBase
    {
        private float3 moveDirection;
        private EntitySettings playerSettings;

        protected override void OnCreate()
        {
            base.OnCreate();
            playerSettings = Addressables.LoadAssetAsync<EntitySettings>("PlayerSettings").WaitForCompletion();
            playerSettings.InputControlSettings.MoveAction.action.performed += OnPlayerMoveHandle;
            playerSettings.InputControlSettings.MoveAction.action.canceled += OnPlayerMoveHandle;
        }

        private void OnPlayerMoveHandle(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            moveDirection = new float3(value.x, value.y, 0);
        }

        protected override void OnUpdate()
        {
            var _moveDirection = moveDirection;
            var _speed = playerSettings.MovementSettings.Speed;

            Entities
                .WithAll<CPlayerTag>()
                .WithAll<CPlayerControlTag>()
                .WithNone<CDestroyTag>()
                .WithNone<CInLimboTag>()
                .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
                .ForEach((Entity entity, EntityCommandBuffer ecb) =>
            {
                if (_moveDirection.Equals(float3.zero))
                    ecb.RemoveComponent<CMoving>(entity);
                else
                {
                    ecb.AddComponent(entity, new CMoving()
                    {
                        Direction = _moveDirection,
                        Speed = _speed
                    });
                }
            }).WithBurst(synchronousCompilation: true).Schedule();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            playerSettings.InputControlSettings.MoveAction.action.performed -= OnPlayerMoveHandle;
            playerSettings.InputControlSettings.MoveAction.action.canceled -= OnPlayerMoveHandle;
        }
    }
}