using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemSingleton : MonoBehaviour
{
    private void Awake()
    {
        var existing = FindObjectsOfType<EventSystem>();
        if (existing.Length > 1)
        {
            Destroy(gameObject); // Destroy this duplicate
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Make this one persistent
        }
    }
}
