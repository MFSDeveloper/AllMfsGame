using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerNew : MonoBehaviour
{
    public static GameManagerNew instance; 

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
}
