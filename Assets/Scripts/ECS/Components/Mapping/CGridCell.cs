using Unity.Entities;

namespace Game.ECS.Components
{
    public struct CGridCell : IComponentData
    {
        public GridCell Value;
    }
}