using UnityEngine;
using Unity.Entities;
using UnityEngine.UIElements;
using Game.ECS.Components;

namespace Game.ECS.Utils
{
    class UIDocumentSingleton : MonoBehaviour
    {
        public static UIDocumentSingleton _instance;
        public UIDocument UIDocument;
        private Entity documentEntity;
        private EntityManager entityManager;

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                documentEntity = entityManager.CreateEntity();

                entityManager.AddSharedComponentManaged(documentEntity, new CUIDocument()
                {
                    uiDocument = UIDocument
                });
                entityManager.AddComponent<CUIDocumentTag>(documentEntity);
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }
}