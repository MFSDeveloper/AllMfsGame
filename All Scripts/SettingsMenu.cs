
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsCanvas; 
    public GameObject quizCanvas; 

    void Start()
    {
        settingsCanvas.SetActive(false); 
    }

    public void ToggleCanvas()
    {
        bool isActive = !settingsCanvas.activeSelf; 

        settingsCanvas.SetActive(isActive); 
        quizCanvas.SetActive(!isActive); 
    }
    public void CloseSettings()
    {
        settingsCanvas.SetActive(false); 
        quizCanvas.SetActive(true); 
    }
}




