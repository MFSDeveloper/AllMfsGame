using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarController : MonoBehaviour
{
    public Image[] progressDots;       // Assign p-1 to p-8 in Inspector
    public TMP_Text progressText;      // Assign 8/8 text reference
    public Color selectedColor = Color.red;     // Red for selected
    public Color unselectedColor = Color.white; // White for unselected

    private int currentCount = 0;

    // Call this when player is selected/deselected
    public void UpdateProgress(int count)
    {
        currentCount = Mathf.Clamp(count, 0, progressDots.Length);

        // Update bar colors
        for (int i = 0; i < progressDots.Length; i++)
        {
            progressDots[i].color = (i < currentCount) ? selectedColor : unselectedColor;
        }

        // Update text
        if (progressText != null)
        {
            progressText.text = currentCount + "/8";
        }
    }

    // Call from ❌ close button
    public void DeselectLastPlayer()
    {
        if (currentCount > 0)
        {
            currentCount--;

            // Tell PlayerSelector to deselect the last selected one
            PlayerSelector last = PlayerSelector.GetLastSelectedPlayer();
            if (last != null)
            {
                last.ForceDeselect();
            }

            UpdateProgress(currentCount);
        }
    }

    // Call when resetting full selection
    public void ResetProgress()
    {
        currentCount = 0;
        UpdateProgress(currentCount);
    }
}
