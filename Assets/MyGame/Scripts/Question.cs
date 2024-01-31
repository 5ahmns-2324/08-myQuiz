using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public string question;
    public Sprite image;
    public List<string> answers;
    public List<int> correctAnswers;

    public bool CheckAnswer(int answerIndex)
    {
        return correctAnswers.Contains(answerIndex);
    }
}
