using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public List<Question> questions;
    public TMP_Text questionText;
    public Image questionImage;
    public List<Button> answerButtons;
    public Button checkButton;
    public Button nextButton;
    public float timeLimit = 10f;

    private Question currentQuestion;
    private bool canAnswer = true;


    void Start()
    {
        LoadQuestion();

        List<int> meineListe = new List<int>() { 1, 2, 3, 4, 5 };
        
        // Die List-Extension-Methode Shuffle aufrufen
        meineListe.Shuffle();
    }

    public void LoadQuestion()
    {
        if (questions.Count > 0)
        {
            currentQuestion = questions[Random.Range(0, questions.Count)];
            questionText.text = currentQuestion.question;
            questionImage.sprite = currentQuestion.image;

            ShuffleAnswers();

            StartCoroutine(StartTimer());
        }
        else
        {
            // End of quiz
        }
    }

    public void CheckAnswer(int answerIndex)
    {
        if (!canAnswer) return;

        canAnswer = false;
        checkButton.interactable = false;

        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }

        if (currentQuestion.CheckAnswer(answerIndex))
        {
            // Correct Answer
            answerButtons[answerIndex].image.color = Color.green;
            nextButton.interactable = true;
        }
        else
        {
            // Wrong Answer
            answerButtons[answerIndex].image.color = Color.red;
        }
    }

    public void NextQuestion()
    {
        foreach (Button button in answerButtons)
        {
            button.image.color = Color.white;
            button.interactable = true;
        }

        checkButton.interactable = true;
        canAnswer = true;
        nextButton.interactable = false;

        LoadQuestion();
    }

    IEnumerator StartTimer()
    {
        float timer = timeLimit;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // Time's up logic here
        NextQuestion();
    }

    void ShuffleAnswers()
    {
        List<string> answers = new List<string>(currentQuestion.answers);
        answers.Shuffle();

        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = answers[i];
        }
    }
}
