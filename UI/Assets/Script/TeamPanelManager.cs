using UnityEngine;
using UnityEngine.UI;

public class TeamPanelController : MonoBehaviour
{
    public GameObject mainPanel;  // 🔵 This is the root panel you want to 
    public GameObject teamPanel;

    // PG to C buttons' child panels
    public GameObject pgPanel;
    public GameObject sgPanel;
    public GameObject sfPanel;
    public GameObject pfPanel;
    public GameObject cPanel;

    // PG to C buttons
    public Button pgButton;
    public Button sgButton;
    public Button sfButton;
    public Button pfButton;
    public Button cButton;

    public Color selectedColor = Color.red;
    public Color defaultColor = Color.white;

    private int selectedPlayerCount = 0;
    public Text selectedCountText; // Assign in Inspector

    public ProgressBarController progressBarController; // 🔴 ADD THIS

    // 🟢 Update player count and progress bar
    public void UpdateSelectedCount(int change)
    {
        selectedPlayerCount += change;

        // Clamp count between 0 and 8 (safety check)
        selectedPlayerCount = Mathf.Clamp(selectedPlayerCount, 0, 8);

        // Update text like "4/8"
        selectedCountText.text = selectedPlayerCount + "/8";

        // Update progress bar
        if (progressBarController != null)
        {
            progressBarController.UpdateProgress(selectedPlayerCount);
        }
    }

    // Opens the main Team Panel
    public void OpenTeamPanel()
    {
        teamPanel.SetActive(true);
    }

    // Closes the main Team Panel
    public void CloseTeamPanel()
    {
        if (mainPanel != null)
            mainPanel.SetActive(false);
        else
            Debug.LogWarning("MainPanel not assigned in Inspector!");
    }


    // Show PG panel
    public void ShowPGPanel()
    {
        CloseAllPanels();
        pgPanel.SetActive(true);
        HighlightButton(pgButton);
    }

    // Show SG panel
    public void ShowSGPanel()
    {
        CloseAllPanels();
        sgPanel.SetActive(true);
        HighlightButton(sgButton);
    }

    // Show SF panel
    public void ShowSFPanel()
    {
        CloseAllPanels();
        sfPanel.SetActive(true);
        HighlightButton(sfButton);
    }

    // Show PF panel
    public void ShowPFPanel()
    {
        CloseAllPanels();
        pfPanel.SetActive(true);
        HighlightButton(pfButton);
    }

    // Show C panel
    public void ShowCPanel()
    {
        CloseAllPanels();
        cPanel.SetActive(true);
        HighlightButton(cButton);
    }

    // Helper method to hide all position panels
    private void CloseAllPanels()
    {
        pgPanel.SetActive(false);
        sgPanel.SetActive(false);
        sfPanel.SetActive(false);
        pfPanel.SetActive(false);
        cPanel.SetActive(false);
    }

    // Highlight clicked button and reset others
    private void HighlightButton(Button selectedBtn)
    {
        pgButton.image.color = defaultColor;
        sgButton.image.color = defaultColor;
        sfButton.image.color = defaultColor;
        pfButton.image.color = defaultColor;
        cButton.image.color = defaultColor;

        selectedBtn.image.color = selectedColor;
    }
}
