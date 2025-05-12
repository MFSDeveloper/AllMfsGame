using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionButton : MonoBehaviour
{
    public Button spButton;
    public Button ppButton;

    public Color selectedColor = Color.red;
    public Color defaultColor = Color.white;

    private bool isSPSelected = false;
    private bool isPPSelected = false;

    // Static reference to track globally selected PP
    private static PlayerSelectionButton currentlySelectedPP;

    void Start()
    {
        spButton.onClick.AddListener(() => ToggleSelection(true));
        ppButton.onClick.AddListener(() => ToggleSelection(false));
        ResetColors(); // Start with default
    }

    void ToggleSelection(bool isSP)
    {
        if (isSP)
        {
            isSPSelected = !isSPSelected;
            isPPSelected = false;

            // If this was the currently selected PP, deselect it
            if (currentlySelectedPP == this)
                currentlySelectedPP = null;
        }
        else
        {
            // If already selected, deselect
            if (isPPSelected)
            {
                isPPSelected = false;
                currentlySelectedPP = null;
            }
            else
            {
                // Deselect previous PP selection if exists
                if (currentlySelectedPP != null)
                {
                    currentlySelectedPP.DeselectPP();
                }

                isPPSelected = true;
                isSPSelected = false;
                currentlySelectedPP = this;
            }
        }

        UpdateButtonColors();
    }

    void DeselectPP()
    {
        isPPSelected = false;
        UpdateButtonColors();
    }

    void UpdateButtonColors()
    {
        spButton.image.color = isSPSelected ? selectedColor : defaultColor;
        ppButton.image.color = isPPSelected ? selectedColor : defaultColor;
    }

    void ResetColors()
    {
        spButton.image.color = defaultColor;
        ppButton.image.color = defaultColor;
    }
}