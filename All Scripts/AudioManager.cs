using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource backgroundMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void ToggleMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Pause();
        }
        else
        {
            backgroundMusic.Play();
        }
    }
}