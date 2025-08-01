using Game.ECS.Components;
using Unity.Entities;
using UnityEngine.UIElements;
using UnityEngine.VFX;

namespace Game.ECS.Systems
{
    public partial class UIPlayerHealthSystem : SystemBase
    {
        private VisualElement UI;
        protected override void OnUpdate()
        {
            if (SystemAPI.HasSingleton<CUIDocumentTag>()
                && SystemAPI.HasSingleton<CGameSingleton>()
                && SystemAPI.HasSingleton<CGameplaySingleton>()
                && SystemAPI.HasSingleton<CPlayerMainTag>())
            {
                if (UI == null)
                {
                    UI = EntityManager
                        .GetSharedComponentManaged<CUIDocument>(SystemAPI.GetSingletonEntity<CUIDocumentTag>())
                        .uiDocument.rootVisualElement;
                }
                else
                {
                    var playerEntity = SystemAPI.GetSingletonEntity<CPlayerMainTag>();
                    var playerHealth = SystemAPI.GetComponent<CHealth>(playerEntity);
                    var playerHealthLabel = UI.Q("PlayerHealthValue") as Label;
                    playerHealthLabel.text = $"Health: {playerHealth.Value}";
                }
            }
        }
    }
}