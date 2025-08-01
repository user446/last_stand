using System;
using Unity.Mathematics;

namespace Game.ECS.Components
{
    public struct GridCell : IEquatable<GridCell>
    {
        public const float CellSize = 1f;

        public int x;
        public int y;

        public GridCell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public readonly bool Equals(GridCell other)
        {
            return x == other.x && y == other.y;
        }

        public override readonly int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + x.GetHashCode();
                hash = hash * 31 + y.GetHashCode();
                return hash;
            }
        }

        public static implicit operator GridCell(float2 val) => new((int)math.floor(val.x / CellSize), (int)math.floor(val.y / CellSize));
        public static implicit operator GridCell(float3 val) => new((int)math.floor(val.x / CellSize), (int)math.floor(val.y / CellSize));
    }
}