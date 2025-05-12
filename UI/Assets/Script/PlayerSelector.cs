using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSelector : MonoBehaviour
{
    public Button selectButton;
    public Text buttonText;
    public Image background;

    private bool isSelected = false;

    public TeamPanelController teamPanelController; // Assign in Inspector

    private static List<PlayerSelector> selectedPlayers = new List<PlayerSelector>();

    void Start()
    {
        selectButton.onClick.AddListener(ToggleSelection);
    }

    void ToggleSelection()
    {
        if (isSelected)
        {
            Deselect();
        }
        else
        {
            Select();
        }
    }

    public void Select()
    {
        if (isSelected || selectedPlayers.Count >= 8)
            return;

        isSelected = true;
        background.color = new Color32(255, 25, 63, 117);
        buttonText.text = "x";

        if (!selectedPlayers.Contains(this))
            selectedPlayers.Add(this);

        if (teamPanelController != null)
            teamPanelController.UpdateSelectedCount(1);
    }

    public void Deselect()
    {
        if (!isSelected)
            return;

        isSelected = false;
        background.color = Color.white;
        buttonText.text = "+";

        if (selectedPlayers.Contains(this))
            selectedPlayers.Remove(this);

        if (teamPanelController != null)
            teamPanelController.UpdateSelectedCount(-1);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    // ✅ Called from ProgressBarController when ❌ is clicked
    public static PlayerSelector GetLastSelectedPlayer()
    {
        if (selectedPlayers.Count > 0)
            return selectedPlayers[selectedPlayers.Count - 1];
        return null;
    }

    // ✅ Force deselection (for external control)
    public void ForceDeselect()
    {
        if (isSelected)
        {
            isSelected = false;
            background.color = Color.white;
            buttonText.text = "+";

            if (selectedPlayers.Contains(this))
                selectedPlayers.Remove(this);

            if (teamPanelController != null)
                teamPanelController.UpdateSelectedCount(-1);
        }
    }

    // ✅ Called when full reset is needed (like on Close button)
    public void ResetSelection()
    {
        isSelected = false;
        background.color = Color.white;
        buttonText.text = "+";

        if (selectedPlayers.Contains(this))
            selectedPlayers.Remove(this);
    }
}
