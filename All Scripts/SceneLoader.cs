using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour

{
    private void PlayClickSound()
    {
    }

    public void LoadQuizScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("Quiz Game");
    }
    
    public void QuizBackBtn()
    {
        PlayClickSound();
        SceneManager.LoadScene("EducationalScene");
    }


    public void LoadSudokuScene()
    {
        DontDestroyOnLoad(gameObject);
        PlayClickSound();
        SceneManager.LoadScene("SudokuPlay");

    }

    public void BackToMainMenu()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToSkillScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("SkillScene");
    }

    public void BackToStartScene()
    {     
        PlayClickSound();
        SceneManager.LoadScene("StartScene");
    }

    public void BackQuiz()
    {
        PlayClickSound();
        SceneManager.LoadScene("MergeQuiz-Sudoku");
    }
    public void Educational()
    {
        PlayClickSound();
        SceneManager.LoadScene("EducationalScene");
    }



    public void Skill()
    {
        PlayClickSound();
        SceneManager.LoadScene("SkillScene");
    }



    public void Event()
    {
        PlayClickSound();
        SceneManager.LoadScene("EventBaseScene");
    }
    public void UpdateScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("UpdateGameScene");
    }

    public void ChessScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("ChessScene");
    }

    public void ChessBack()
    {
        PlayClickSound();
        SceneManager.LoadScene("StartScene");
    }

    public void SolitaireScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("Game 1");
    }

    public void SolitaireBackBtn()
    {
        PlayClickSound();
        SceneManager.LoadScene("SkillScene");
    }

    public void SolitaireBackBtn1()
    {
        PlayClickSound();
        SceneManager.LoadScene("SkillScene");
    }

    //public void MinesweeperBackBtn()
    //{
    //    PlayClickSound();
    //    SceneManager.LoadScene("SkillScene");
    //}

    public void MinesweeperScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("IntroScene");
    }

    public void PokerScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("PokerGame");
    }

    public void BackToEvent()
    {
        PlayClickSound();
        SceneManager.LoadScene("EventBaseScene");
    }

    public void FantasyScene()
    {
        PlayClickSound();
        SceneManager.LoadScene("FantasyGame");
    }
    

}
