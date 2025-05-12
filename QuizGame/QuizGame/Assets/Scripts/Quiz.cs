using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite wrongAnswerSprite;
    [SerializeField] Sprite selectedWrongAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Image[] progressSegments;
    [SerializeField] Sprite lightRedSprite;
    [SerializeField] Sprite darkRedSprite;

    [Header("Game Timer Display")]
    [SerializeField] TextMeshProUGUI gameTimerText;

    private float gameTime = 0f;
    private int currentProgressIndex = 0;
    public bool isComplete;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();

        foreach (Image segment in progressSegments)
        {
            segment.sprite = lightRedSprite;
        }
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        UpdateGameTimerUI();

        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            if (currentProgressIndex >= progressSegments.Length)
            {
                isComplete = true;
                return;
            }
            hasAnsweredEarly = false;
            UpdateProgressBar(); // **Question load thay tyare dark red thai jase**
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        StartCoroutine(DisplayAnswer(index));
        SetButtonState(false);
        timer.CancelTimer();
        //scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";

        correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();

        if (index == correctAnswerIndex)
        {
            scoreKeeper.IncrementCorrectAnswers(); // ✅ Correct answer hoy to score vadhse
        }

        scoreKeeper.IncrementQuestionsSeen(); // ✅ Total questions count vadhse

        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    IEnumerator DisplayAnswer(int index)
    {
        correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        Image selectedButtonImage = answerButtons[index].GetComponent<Image>();

        // Answer ni color white karva mate
        TextMeshProUGUI selectedButtonText = answerButtons[index].GetComponentInChildren<TextMeshProUGUI>();
        selectedButtonText.color = Color.white;

        if (index == correctAnswerIndex)
        {
            questionText.text = "Correct!";
            selectedButtonImage.sprite = correctAnswerSprite;
        }
        else
        {
            questionText.text = "Wrong!";
            selectedButtonImage.sprite = selectedWrongAnswerSprite;

            yield return new WaitForSeconds(0.5f);
            selectedButtonImage.sprite = wrongAnswerSprite;

            Image correctButtonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            correctButtonImage.sprite = correctAnswerSprite;

            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text += "\nCorrect answer: " + correctAnswer;
        }

        // Right answer no text white karva mate
        TextMeshProUGUI correctButtonText = answerButtons[correctAnswerIndex].GetComponentInChildren<TextMeshProUGUI>();
        correctButtonText.color = Color.white;
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
            buttonText.color = Color.black; // Default Color (Black)
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    private void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void UpdateProgressBar()
    {
        if (currentProgressIndex < progressSegments.Length)
        {
            progressSegments[currentProgressIndex].sprite = darkRedSprite; // **Next Question Load thay tyare dark red**
            //progressSegments[currentProgressIndex].SetNativeSize();
            currentProgressIndex++;
        }
    }

    void UpdateGameTimerUI()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        gameTimerText.text = $" {minutes:00}:{seconds:00}";
    }
}
