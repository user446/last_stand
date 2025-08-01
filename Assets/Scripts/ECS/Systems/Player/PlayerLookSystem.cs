using Game.ECS.Components;
using Game.Settings;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Game.ECS.Systems
{
    public partial class PlayerShootSystem : SystemBase
    {
        private float3 pointerPosition;
        private EntitySettings playerSettings;

        protected override void OnCreate()
        {
            base.OnCreate();
            playerSettings = Addressables.LoadAssetAsync<EntitySettings>("PlayerSettings").WaitForCompletion();
            playerSettings.InputControlSettings.LookAction.action.performed += OnPlayerLookHandle;
        }

        private void OnPlayerLookHandle(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            pointerPosition = Camera.main.ScreenToWorldPoint(value);
        }

        protected override void OnUpdate()
        {
            var _pointerPos = pointerPosition;
            var offsetAngle = playerSettings.MovementSettings.RotationOffsetAngle;

            Entities
                .WithAll<CPlayerTag>()
                .WithAll<CPlayerControlTag>()
                .WithAll<CShootingDirection>()
                .WithNone<CDestroyTag>()
                .WithNone<CInLimboTag>()
                .ForEach((Entity entity, ref LocalTransform localTransform, ref CShootingDirection shootingDir) =>
            {
                var direction = _pointerPos - localTransform.Position;
                shootingDir.Value = math.normalize(new float3(direction.x, direction.y, 0));

                //rotate sprite
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offsetAngle;
                localTransform.Rotation = Quaternion.Euler(0, 0, angle);
            }).WithBurst(synchronousCompilation: true).Schedule();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            playerSettings.InputControlSettings.LookAction.action.performed -= OnPlayerLookHandle;
        }
    }
}