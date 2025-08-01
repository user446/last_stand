using Unity.Entities;

namespace Game.ECS.Components
{
    public struct CShootingCooldown : IComponentData, IEnableableComponent
    {
        public float Time;
    }
}