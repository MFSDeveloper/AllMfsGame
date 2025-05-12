using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    private static bool isMusicPlaying = false;

    private void Awake()
    {
        if (!isMusicPlaying)
        {
            DontDestroyOnLoad(gameObject);
            isMusicPlaying = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }
}