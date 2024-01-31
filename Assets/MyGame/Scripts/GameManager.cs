using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text scoreText, numberText, timeText, endScoreText;
    public Button nextButton, restartButton, checkButton;
    public GameObject endScreen, timeUpScreen;
    public GameObject image;
    public Sprite pic1, pic2, pic3, pic4, pic5;
    public AudioSource wrong, correct;

    public float totalTime = 20f;
    private float timeLeft = 20f;
    private bool isCorrect;

    private string[] questions = {
        "Welches Element hat das chemische Symbol Hg?",
        "In welchem Jahr wurde die Unabhängigkeit der Vereinigten Staaten von Amerika erklärt?",
        "Wer schrieb das Drama Hamlet?",
        "Welches ist das größte Säugetier der Welt?",
        "Was ist die Hauptstadt von Japan?"
    };

    private string[][] answers = {
        new string[] { "Eisen", "Quecksilber", "Holmium" },
        new string[] { "1776", "1789", "1801" },
        new string[] { "Johann Wolfgang von Goethe", "Charles Dickens", "William Shakespeare" },
        new string[] { "Blauwal", "Elefant", "Giraffe" },
        new string[] { "Beijing", "Seoul", "Tokio" }
    };

    private List<int>[] correctAnswers = {
    new List<int> {1},
    new List<int> {0},
    new List<int> {2},
    new List<int> {0},
    new List<int> {2} };


    private List<int> questionOrder;
    private int currentQuestionIndex = 0, number = 1;
    private int score = 0;
    private bool isChecking = false;

    void Start()
    {
        nextButton.onClick.AddListener(NextButtonClicked);
        checkButton.onClick.AddListener(CheckButtonClicked);
        ShuffleQuestions();
        ShowQuestion();

        endScreen.SetActive(false);
        timeUpScreen.SetActive(false);
    }

    void Update()
    {
        if (!isChecking)
        {
            timeLeft -= Time.deltaTime;
            timeText.text = "Time: " + timeLeft.ToString("F0");
        }


        if (timeLeft <= 0f)
        {
            TimerEnded();
        }
    }

    void TimerEnded()
    {
        timeLeft = 0f;
        timeUpScreen.SetActive(true);
    }

    void ShuffleQuestions()
    {
        questionOrder = Enumerable.Range(0, questions.Length).ToList();
        System.Random rng = new System.Random();
        int n = questionOrder.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = questionOrder[k];
            questionOrder[k] = questionOrder[n];
            questionOrder[n] = value;
        }
    }

    void ShowQuestion()
    {
        int shuffledIndex = questionOrder[currentQuestionIndex];
        questionText.text = questions[shuffledIndex];

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answers[shuffledIndex][i];
            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerClick(index));
            answerButtons[i].interactable = true;
            answerButtons[i].GetComponent<Image>().color = Color.white;
        }

        nextButton.interactable = false;
        checkButton.interactable = false;
        isChecking = false;

        if (shuffledIndex == 0)
        {
            image.GetComponent<Image>().sprite = pic1;
        }
        else if (shuffledIndex == 1)
        {
            image.GetComponent<Image>().sprite = pic2;
        }
        else if (shuffledIndex == 2)
        {
            image.GetComponent<Image>().sprite = pic3;
        }
        else if (shuffledIndex == 3)
        {
            image.GetComponent<Image>().sprite = pic4;
        }
        else if (shuffledIndex == 4)
        {
            image.GetComponent<Image>().sprite = pic5;
        }
    }

    void OnAnswerClick(int answerIndex)
    {
        checkButton.interactable = true;

        string selectedAnswer = answers[questionOrder[currentQuestionIndex]][answerIndex];
        if (IsCorrectAnswer(answerIndex))
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }
    }

    bool IsCorrectAnswer(int answerIndex)
    {
        return correctAnswers[questionOrder[currentQuestionIndex]].Contains(answerIndex);
    }

    void CheckButtonClicked()
    {
        if (isCorrect)
        {
            correct.Play();
            score++;
        }
        else
        {
            wrong.Play();
        }

        scoreText.text = "Score: " + score;
        isChecking = true;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (IsCorrectAnswer(i))
            {
                answerButtons[i].GetComponent<Image>().color = Color.green;
            }
            else 
            {
                answerButtons[i].GetComponent<Image>().color = Color.red;
            }
        }

        nextButton.interactable = true;
        checkButton.interactable = false;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;
        }
    }
    void NextButtonClicked()
    {  
        if (!isChecking)
        {
            return;
        }
        currentQuestionIndex++;

        if (number < 5)
        {
            number++;
            numberText.text = number + "/5";
        }

        if (currentQuestionIndex == questions.Length - 1)
        {
            nextButton.interactable = false;
        }

        if (currentQuestionIndex < questions.Length)
        {
            ShowQuestion();
        }
        else
        {
            EndQuiz();
        }

        timeLeft = totalTime;
    }

    void EndQuiz()
    {
        Debug.Log("Quiz beendet. Score: " + score);
        scoreText.text = "Score: " + score;

        endScreen.SetActive(true);
        nextButton.interactable = false;
        checkButton.interactable = false;

        endScoreText.text = "Your Score: " + score + "/5";
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}



