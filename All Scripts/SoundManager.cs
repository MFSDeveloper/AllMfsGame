using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource backgroundMusic;  
    public Toggle soundToggle; 



    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        Debug.Log("SoundManager: Script Initialized");

        if (backgroundMusic == null)
        {
            Debug.LogError("SoundManager: AudioSource (backgroundMusic) is not assigned in the Inspector!");
            return;
        }

        if (soundToggle == null)
        {
            Debug.LogError("SoundManager: Toggle (soundToggle) is not assigned in the Inspector!");
            return;
        }

        if (!PlayerPrefs.HasKey("SoundState"))
        {
            Debug.Log("SoundManager: No previous sound settings found. Setting default ON.");
            PlayerPrefs.SetInt("SoundState", 1); 
            PlayerPrefs.Save(); 
        }

        bool isSoundOn = PlayerPrefs.GetInt("SoundState") == 1;
        backgroundMusic.mute = !isSoundOn;
        soundToggle.isOn = isSoundOn;

        Debug.Log($"SoundManager: Loaded sound state -> {(isSoundOn ? "ON" : "OFF")}");

        soundToggle.onValueChanged.AddListener(delegate { ToggleSound(); });
    }

    public void ToggleSound()
    {
        if (backgroundMusic == null || soundToggle == null)
        {
            Debug.LogError("SoundManager: Cannot toggle sound - Missing components!");
            return;
        }

        bool isSoundOn = soundToggle.isOn;
        backgroundMusic.mute = !isSoundOn;
        PlayerPrefs.SetInt("SoundState", isSoundOn ? 1 : 0);
        PlayerPrefs.Save(); 

        Debug.Log($"SoundManager: Toggled sound -> {(isSoundOn ? "ON" : "OFF")}");
    }
}
