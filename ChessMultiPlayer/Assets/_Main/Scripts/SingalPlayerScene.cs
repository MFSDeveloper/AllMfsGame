using UnityEngine;
using UnityEngine.SceneManagement; 

public class SingalPlayerScene : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Actual Chess"); 
    }

    public void ChangeMulti()
    {
        SceneManager.LoadScene("MultiPlayerScene");
    }

    public void ChangeChess()
    {
        SceneManager.LoadScene("SkillScene");
    }
}
