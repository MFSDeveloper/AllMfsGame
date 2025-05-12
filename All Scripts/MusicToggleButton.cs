using UnityEngine;
using UnityEngine.UI;

public class MusicToggleButton : MonoBehaviour
{
    private Button toggleButton;

    void Start()
    {
        toggleButton = GetComponent<Button>();

        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleMusic);
        }
    }

    void ToggleMusic()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleMusic();
        }
    }
}