using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
  //UI tooling
  [SerializeField] TextMeshProUGUI screenText;
  [SerializeField] QuestionSO[] questionLogic;
  [SerializeField] GameObject[] answerButtons;
  
  //Local Variables
  int correctAnswerIndex;

  //Sprites 
  [SerializeField] Sprite defaultAnswerSprite;
  [SerializeField] Sprite correctAnswerSprite;

  void Start()
  {
    GetNextQuestion();
  }

  void DisplayQuiz()
  {
    screenText.text = questionLogic[0].GetQuestion();

    for (int i = 0; i < answerButtons.Length; i++)
    {
      TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
      buttonText.text = questionLogic[0].GetAnswer(i);
    }
  }

  void GetNextQuestion ()
  {
    SetButtonState(true);
    SetDefaultButtonSprites();
    DisplayQuiz();
  }

  public void onAnswerSelected(int index)
  {
    Image buttonImage;

    if (index == questionLogic[0].GetCorrectAnswerIndex())
    {
      //Text changing when the answer is correct
      screenText.text = "Correct";

      //Temporary variable changed to the correct Image component and then sprite changed to match
      buttonImage = answerButtons[index].GetComponent<Image>();
      buttonImage.sprite = correctAnswerSprite;
    }
    else
    {
      //Text changing when the answer is incorrect and shows the correct answer
      correctAnswerIndex = questionLogic[0].GetCorrectAnswerIndex();
      screenText.text = "Incorrect: " + questionLogic[0].GetAnswer(correctAnswerIndex);

      //Temporary variable changed as done above
      buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
      buttonImage.sprite = correctAnswerSprite;
    }

    SetButtonState(false);
  }

  void SetButtonState(bool state)
  {
    for (int i = 0; i < answerButtons.Length; i++)
    {
      Button button = answerButtons[i].GetComponent<Button>();
      button.interactable = state;
    }
  }

  void SetDefaultButtonSprites()
  {
    for (int i = 0; i < answerButtons.Length; i++)
    {
      Image buttonImage = answerButtons[i].GetComponent<Image>();
      buttonImage.sprite = defaultAnswerSprite;
    }
  }
}
