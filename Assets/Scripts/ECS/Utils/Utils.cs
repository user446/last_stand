using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Game.ECS.Utils
{
    public static class Utils
    {
        public static void AddVisuals(this EntityManager entityManager, Entity entity, Material material, Mesh mesh)
        {
            var renderMeshDesc = new RenderMeshDescription(
                shadowCastingMode: UnityEngine.Rendering.ShadowCastingMode.Off,
                receiveShadows: false);

            var renderMeshArray = new RenderMeshArray(new Material[] { material }, new Mesh[] { mesh });

            RenderMeshUtility.AddComponents(
                entity,
                entityManager,
                renderMeshDesc,
                renderMeshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
        }
    }
}