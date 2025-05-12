using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokerPanels : MonoBehaviour
{
    public GameObject raiseButton;
    public GameObject callButton;
    public GameObject foldButton;

    public GameObject raisePanel;
    public Slider raiseSlider;
    public TMP_Text raiseAmountText;
    public Button raiseOkButton;


    public void EnableActionButtons()
    {
        raiseButton.SetActive(true);
        callButton.SetActive(true);
        foldButton.SetActive(true);
    }

    // ?? NEW METHOD: Disable buttons during bot's turn
    public void DisableActionButtons()
    {
        raiseButton.SetActive(false);
        callButton.SetActive(false);
        foldButton.SetActive(false);
    }

    public void ShowRaisePanel(int minRaise, int maxRaise)
    {
        raisePanel.SetActive(true);
        raiseSlider.minValue = minRaise;
        raiseSlider.maxValue = maxRaise;
        raiseSlider.value = minRaise;
        UpdateRaiseAmountText(minRaise);

        raiseSlider.onValueChanged.RemoveAllListeners();
        raiseSlider.onValueChanged.AddListener(value =>
        {
            UpdateRaiseAmountText((int)value);
        });
    }

    public void HideRaisePanel()
    {
        raisePanel.SetActive(false);
    }

    private void UpdateRaiseAmountText(int amount)
    {
        raiseAmountText.text = "" + amount.ToString();
    }
}