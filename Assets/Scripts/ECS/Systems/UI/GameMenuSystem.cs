using Game.ECS.Components;
using Unity.Entities;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.ECS.Systems
{
    public partial class GameMenuSystem : SystemBase
    {
        private VisualElement UI;
        private Button startGameButton;
        private Button exitGameButton;
        private VisualElement gameMenu;

        protected override void OnUpdate()
        {
            if (SystemAPI.HasSingleton<CUIDocumentTag>()
                && SystemAPI.HasSingleton<CGameSingleton>()
                && (!SystemAPI.HasSingleton<CGameplaySingleton>()
                    || SystemAPI.HasSingleton<CGameOverSingleton>()))
            {
                InitalizeMenu();
            }
        }

        private void InitalizeMenu()
        {
            UI = EntityManager
                .GetSharedComponentManaged<CUIDocument>(SystemAPI.GetSingletonEntity<CUIDocumentTag>())
                .uiDocument.rootVisualElement;
            gameMenu = UI.Q("StartGameMenu");
            if (!gameMenu.visible)
            {
                startGameButton = UI.Q("StartButton") as Button;
                exitGameButton = UI.Q("QuitButton") as Button;
                startGameButton.clicked += OnStartClicked;
                exitGameButton.clicked += OnExitClicked;
                startGameButton.text = "Try again";
                gameMenu.visible = true;
            }
        }

        private void OnStartClicked()
        {
            ClearSubscribtions();
            if(!SystemAPI.HasSingleton<CGameplaySingleton>())
                EntityManager.CreateSingleton<CGameplaySingleton>();
            if (SystemAPI.HasSingleton<CGameOverSingleton>())
            {
                var gameOverEntity = SystemAPI.GetSingletonEntity<CGameOverSingleton>();
                EntityManager.DestroyEntity(gameOverEntity);
            }
            gameMenu.visible = false;
        }

        private void OnExitClicked()
        {
            if (SystemAPI.HasSingleton<CGameplaySingleton>())
            {
                var gameOverEntity = SystemAPI.GetSingletonEntity<CGameplaySingleton>();
                EntityManager.DestroyEntity(gameOverEntity);
            }
            ClearSubscribtions();
            SceneManager.LoadSceneAsync("MainMenu");
        }

        private void ClearSubscribtions()
        {
            if (startGameButton != null)
                startGameButton.clicked -= OnStartClicked;
            if (exitGameButton != null)
                exitGameButton.clicked -= OnExitClicked;
        }
    }
}