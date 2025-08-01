using System;
using Unity.Entities;
using UnityEngine.UIElements;

namespace Game.ECS.Components
{
    public struct CUIDocument : ISharedComponentData, IEquatable<CUIDocument>
    {
        public UIDocument uiDocument;
        public readonly bool Equals(CUIDocument other)
        {
            return uiDocument?.GetHashCode() == other.uiDocument?.GetHashCode();
        }

        public override readonly int GetHashCode()
        {
            return uiDocument.GetHashCode();
        }
    }
}