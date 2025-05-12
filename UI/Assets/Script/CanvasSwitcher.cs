using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;

    public void ShowCanvas2()
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        canvas3.SetActive(false);
    }

    public void ShowCanvas1()
    {
        canvas1.SetActive(true);
        canvas2.SetActive(false);
        canvas3.SetActive(false);
    }

    public void ShowCanvas3()
    {
        canvas1.SetActive(false);
        canvas2.SetActive(false);
        canvas3.SetActive(true);
    }

    public void BackToCanvas2()
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        canvas3.SetActive(false);
    }

    public void BackToStartScene()
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        canvas3.SetActive(false);
        SceneManager.LoadScene("EventBaseScene");
    }
}
