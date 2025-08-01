using Unity.Collections;
using Unity.Entities;

namespace Game.ECS.Components
{
    public struct CEntityMapSingleton : IComponentData
    {
        public NativeParallelMultiHashMap<GridCell, Entity> EntityMap;
    }
}