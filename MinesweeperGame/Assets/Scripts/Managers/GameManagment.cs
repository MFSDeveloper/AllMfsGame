using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Managers
{
    public class GameManagment : Singleton<GameManagment>
    {
        [Header("Field Setup")]
        [SerializeField] public List<FieldSetup> fieldSetups;
        [SerializeField] public List<CellTheme> cellThemes;
        public FieldSetup CurrentFieldSetup;

        [Header("Field Dependencies")]
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform cellHolder;

        private FieldSetup selectedFieldSetup;

        private void Awake()
        {
            CurrentFieldSetup = fieldSetups[0];

            Field.OnGameStart += HandleGameStart;
            Field.OnGameLost += HandleGameLost;
            Field.OnGameWon += HandleGameWon;
        }

        private void OnDestroy()
        {
            Field.OnGameStart -= HandleGameStart;
            Field.OnGameLost -= HandleGameLost;
            Field.OnGameWon -= HandleGameWon;
        }

        public void BootstrapField()
        {
            var theme = cellThemes.Count > 0 ? cellThemes[0] : null;

            if (cellPrefab == null || cellHolder == null || theme == null)
            {
                Debug.LogError("[GameManagement] One or more dependencies are missing.");
                return;
            }

            Field.Instance.Configure(cellPrefab, cellHolder, theme);
            Field.Instance.Init(CurrentFieldSetup);

            InputManager.Instance.gameObject.SetActive(true);
        }

        private static void HandleGameStart()
        {
            InputManager.Instance.gameObject.SetActive(true);
        }

        private static void HandleGameLost()
        {
            InputManager.Instance.gameObject.SetActive(false);
        }

        private static void HandleGameWon()
        {
            InputManager.Instance.gameObject.SetActive(false);
        }

        public void SetSelectedFieldSetup(FieldSetup setup)
        {
            selectedFieldSetup = setup;
        }

        public FieldSetup GetSelectedFieldSetup()
        {
            return selectedFieldSetup;
        }
    }
}



