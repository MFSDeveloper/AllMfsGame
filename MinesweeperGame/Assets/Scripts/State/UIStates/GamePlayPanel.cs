using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace State.UIStates
{
    public class GamePlayPanel : UIState
    {
        [SerializeField] private RectTransform panel;
        [Space]
        [Header("Buttons")]
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button optionsButton;
        [Space]
        [Header("GameOver Popup")]
        [SerializeField] private TMP_Text gameOverText;
        [SerializeField] private RectTransform gameOverPanel;
        [SerializeField] private Button resetButton;

        // Delegates
        public delegate void ButtonPress();
        public static event ButtonPress OnMainMenuButton;
        public static event ButtonPress OnOptionsButton;
        public static event ButtonPress OnResetButton;

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(panel);
            Assert.IsNotNull(mainMenuButton);
            Assert.IsNotNull(optionsButton);
            Assert.IsNotNull(resetButton);
#endif
            panel.gameObject.SetActive(false);
        }

        public override void OnEnter()
        {
            panel.gameObject.SetActive(true);
            gameOverPanel.gameObject.SetActive(false);

            mainMenuButton.onClick.AddListener(HandleMainMenuButton);
            optionsButton.onClick.AddListener(HandleOptionsButton);
            resetButton.onClick.AddListener(HandleResetButton);

            Field.OnGameLost += HandleGameLost;
            Field.OnGameWon += HandleGameWon;
        }

        public override void OnExit()
        {
            // Prevent any event calls after panel is destroyed or deactivated
            Field.OnGameLost -= HandleGameLost;
            Field.OnGameWon -= HandleGameWon;

            if (mainMenuButton != null) mainMenuButton.onClick.RemoveAllListeners();
            if (optionsButton != null) optionsButton.onClick.RemoveAllListeners();
            if (resetButton != null) resetButton.onClick.RemoveAllListeners();

            if (gameOverPanel != null) gameOverPanel.gameObject.SetActive(false);
            if (panel != null) panel.gameObject.SetActive(false);

            Debug.Log("[GamePlayPanel] OnExit called and cleaned up");
        }

        private void OnDestroy()
        {
            // Additional safety to clean up delegates
            Field.OnGameLost -= HandleGameLost;
            Field.OnGameWon -= HandleGameWon;

            Debug.LogWarning("[GamePlayPanel] Destroyed - listeners removed");
        }

        private void HandleMainMenuButton()
        {
            gameOverPanel.gameObject.SetActive(false);
            OnMainMenuButton?.Invoke();
        }

        private void HandleOptionsButton()
        {
            gameOverPanel.gameObject.SetActive(false);
            OnOptionsButton?.Invoke();
        }

        private void HandleResetButton()
        {
            gameOverPanel.gameObject.SetActive(false);
            OnResetButton?.Invoke();
        }

        private void HandleGameWon()
        {
            if (this != null && gameObject.activeInHierarchy)
                StartCoroutine(ShowGameOverPanelWithDelay("You Won!"));
        }

        private void HandleGameLost()
        {
            if (this != null && gameObject.activeInHierarchy)
                StartCoroutine(ShowGameOverPanelWithDelay("You Lost!"));
        }

        private System.Collections.IEnumerator ShowGameOverPanelWithDelay(string message)
        {
            yield return new WaitForSeconds(1f);
            if (gameOverText != null) gameOverText.text = message;
            if (gameOverPanel != null) gameOverPanel.gameObject.SetActive(true);
        }
    }
}




//using Core;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//#if UNITY_EDITOR
//using UnityEngine.Assertions;
//#endif

//namespace State.UIStates
//{
//    public class GamePlayPanel : UIState
//    {
//        [SerializeField] private RectTransform panel;
//        [Space]
//        [Header("Buttons")]
//        [SerializeField] private Button mainMenuButton;
//        [SerializeField] private Button optionsButton;
//        [Space]
//        [Header("GameOver Popup")]
//        [SerializeField] private TMP_Text gameOverText;
//        [SerializeField] private RectTransform gameOverPanel;
//        [SerializeField] private Button resetButton;

//        // Delegates
//        public delegate void ButtonPress();
//        public static event ButtonPress OnMainMenuButton;
//        public static event ButtonPress OnOptionsButton;
//        public static event ButtonPress OnResetButton;

//        private void Awake()
//        {
//#if UNITY_EDITOR
//            Assert.IsNotNull(panel);
//            Assert.IsNotNull(mainMenuButton);
//            Assert.IsNotNull(optionsButton);
//            Assert.IsNotNull(resetButton);
//#endif
//            panel.gameObject.SetActive(false);
//        }

//        public override void OnEnter()
//        {
//            panel.gameObject.SetActive(true);
//            gameOverPanel.gameObject.SetActive(false);
//            mainMenuButton.onClick.AddListener(HandleMainMenuButton);
//            optionsButton.onClick.AddListener(HandleOptionsButton);
//            resetButton.onClick.AddListener(HandleResetButton);

//            Field.OnGameLost += HandleGameLost;
//            Field.OnGameWon += HandleGameWon;
//        }


//        public override void OnExit()
//        {
//            if (this == null) return; // <- prevents calling on destroyed object
//            if (gameOverPanel != null) gameOverPanel.gameObject.SetActive(false);
//            if (mainMenuButton != null) mainMenuButton.onClick.RemoveAllListeners();
//            if (optionsButton != null) optionsButton.onClick.RemoveAllListeners();
//            if (resetButton != null) resetButton.onClick.RemoveAllListeners();

//            Field.OnGameLost -= HandleGameLost;
//            Field.OnGameWon -= HandleGameWon;

//            Debug.Log("[GamePlay] OnExit");
//        }


//        //public override void OnExit()
//        //{
//        //    panel.gameObject.SetActive(false);
//        //    gameOverPanel.gameObject.SetActive(false);
//        //    mainMenuButton.onClick.RemoveAllListeners();
//        //    optionsButton.onClick.RemoveAllListeners();
//        //    resetButton.onClick.RemoveAllListeners();

//        //    Field.OnGameLost -= HandleGameLost;
//        //    Field.OnGameWon -= HandleGameWon;
//        //}

//        private void HandleMainMenuButton()
//        {
//            gameOverPanel.gameObject.SetActive(false);
//            OnMainMenuButton?.Invoke();
//        }

//        private void HandleOptionsButton()
//        {
//            gameOverPanel.gameObject.SetActive(false);
//            OnOptionsButton?.Invoke();
//        }

//        private void HandleResetButton()
//        {
//            gameOverPanel.gameObject.SetActive(false);
//            OnResetButton?.Invoke();
//        }

//        private void HandleGameWon()
//        {
//            StartCoroutine(ShowGameOverPanelWithDelay("You Won!"));
//        }

//        private void HandleGameLost()
//        {
//            StartCoroutine(ShowGameOverPanelWithDelay("You Lost!"));
//        }

//        private System.Collections.IEnumerator ShowGameOverPanelWithDelay(string message)
//        {
//            yield return new WaitForSeconds(1f); // 1 second delay
//            gameOverText.text = message;
//            gameOverPanel.gameObject.SetActive(true);
//        }


//    }
//}
