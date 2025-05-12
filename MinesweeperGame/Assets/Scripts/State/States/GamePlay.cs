using System.Collections;
using Managers;
using State.UIStates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace State.States
{
    internal class GamePlay : State
    {
        public static GamePlay Instance { get; } = new GamePlay();
        private static bool _isSceneLoading = false;

        public override void OnEnter()
        {
            Debug.Log("[GamePlay] OnEnter");

            // Prevent duplicate calls
            if (!_isSceneLoading)
            {
                LoadGameScene();
            }

            GamePlayPanel.OnMainMenuButton += HandleMainMenuButton;
            GamePlayPanel.OnOptionsButton += HandleOptionsButton;
            GamePlayPanel.OnResetButton += BootstrapField;
        }

        public override void OnExit()
        {
            Debug.Log("[GamePlay] OnExit");

            GamePlayPanel.OnMainMenuButton -= HandleMainMenuButton;
            GamePlayPanel.OnOptionsButton -= HandleOptionsButton;
            GamePlayPanel.OnResetButton -= BootstrapField;
        }

        private static void LoadGameScene()
        {
            // Check if scene is already loaded
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == "GameScene")
                {
                    Debug.Log("[GamePlay] GameScene already loaded.");
                    BootstrapField(); // Reinitialize game board
                    return;
                }
            }

            // Load if not already
            _isSceneLoading = true;
            StateManager.StartCoroutine(LoadGameSceneAsync());
        }

        private static IEnumerator LoadGameSceneAsync()
        {
            Debug.Log("[GamePlay] Loading GameScene async...");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

            asyncLoad.completed += operation =>
            {
                Debug.Log("[GamePlay] Scene is Loaded async");
                _isSceneLoading = false;
                BootstrapField();
            };

            while (!asyncLoad.isDone)
            {
                Debug.Log($"[GamePlay] - Loading Scene: {asyncLoad.progress * 100}%");
                yield return null;
            }
        }

        private static void HandleMainMenuButton()
        {
            Debug.Log("[GamePlay] Going to Main Menu. Unloading GameScene...");

            // Properly unload scene before navigating
            StateManager.StartCoroutine(UnloadAndSwitchToMainMenu());
        }

        private static IEnumerator UnloadAndSwitchToMainMenu()
        {
            if (SceneManager.GetSceneByName("GameScene").isLoaded)
            {
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("GameScene");

                while (!unloadOp.isDone)
                {
                    Debug.Log($"[GamePlay] - Unloading Scene: {unloadOp.progress * 100}%");
                    yield return null;
                }

                Debug.Log("[GamePlay] GameScene unloaded.");
            }

            StateManager.SetState(MainMenu.Instance);
        }

        private static void HandleOptionsButton()
        {
            StateManager.PushState(OptionsMenu.Instance);
        }

        private static void BootstrapField()
        {
            Debug.Log("[GamePlay] Bootstrapping field...");
            GameManagment.Instance.BootstrapField();
        }
    }
}