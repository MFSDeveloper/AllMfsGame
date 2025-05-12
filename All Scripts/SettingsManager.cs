using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle musicToggle;
    public Slider volumeSlider;
    public AudioSource backgroundMusic;

    private void Start()
    {
        settingsPanel.SetActive(false);

        musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

        UpdateMusic();
    }

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void UpdateMusic()
    {
        backgroundMusic.mute = !musicToggle.isOn;
        backgroundMusic.volume = volumeSlider.value;

        PlayerPrefs.SetInt("MusicOn", musicToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }
}