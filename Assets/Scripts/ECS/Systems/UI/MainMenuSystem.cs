using System;
using Game.ECS.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.ECS.Systems
{
    public partial class MainMenuSystem : SystemBase
    {
        private VisualElement UI;
        private Button startGameButton;
        private Button exitGameButton;
        private VisualElement startGameMenu;
        private bool initalized;
        private Label playerHealthLabel;


        protected override void OnUpdate()
        {
            if (SystemAPI.HasSingleton<CUIDocumentTag>() && !initalized)
            {
                InitalizeMenu();
                initalized = true;
            }
        }

        private void InitalizeMenu()
        {
            UI = EntityManager
                .GetSharedComponentManaged<CUIDocument>(SystemAPI.GetSingletonEntity<CUIDocumentTag>())
                .uiDocument.rootVisualElement;
            startGameButton = UI.Q("StartButton") as Button;
            exitGameButton = UI.Q("QuitButton") as Button;
            startGameMenu = UI.Q("StartGameMenu");
            startGameButton.text = "Play";
            startGameButton.clicked += OnStartClicked;
            exitGameButton.clicked += OnExitClicked;
            startGameMenu.visible = true;

            playerHealthLabel = UI.Q("PlayerHealthValue") as Label;
            playerHealthLabel.visible = false;

        }

        private void OnExitClicked()
        {
            ClearSubscribtions();
            Application.Quit();
        }

        private void OnStartClicked()
        {

            EntityManager.CreateSingleton<CGameSingleton>();
            EntityManager.CreateSingleton<CGameplaySingleton>();
            startGameMenu.visible = false;
            playerHealthLabel.visible = true;

            ClearSubscribtions();
            SceneManager.LoadSceneAsync("GameScene");
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