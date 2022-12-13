using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
  //UI tooling
  [Header("Questions")]
  [SerializeField] TextMeshProUGUI screenText;
  [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
  QuestionSO currentQuestion;

  [Header("Answers")]
  [SerializeField] GameObject[] answerButtons;

  //Local Variables
  int correctAnswerIndex;
  bool hasAnsweredEarly = true;

  //Sprites 
  [Header("Button Colors")]
  [SerializeField] Sprite defaultAnswerSprite;
  [SerializeField] Sprite correctAnswerSprite;

  //Timer Controller
  [Header("Timer")]
  [SerializeField] Image timerImage;
  Timer timer;

  [Header("Scoring")]
  [SerializeField] TextMeshProUGUI scoreText;
  ScoreKeeper scoreKeeper;

  [Header("ProgressBar")]
  [SerializeField] Slider progressBar;
  public bool isComplete;

  void Awake()
  {
    timer = FindObjectOfType<Timer>();
    scoreKeeper = FindObjectOfType<ScoreKeeper>();
    progressBar.maxValue = questions.Count;
    progressBar.value = 0;
  }

  void Update()
  {
    timerImage.fillAmount = timer.fillFraction;
    if (timer.loadNextQuestion)
    {
      if (progressBar.value == progressBar.maxValue)
      {
        isComplete = true;
        return;
      }

      hasAnsweredEarly = false;
      GetNextQuestion();
      timer.loadNextQuestion = false;
    }
    else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
    {
      DisplayAnswer(-1);
      SetButtonState(false);
    }
  }

  void DisplayQuiz()
  {
    screenText.text = currentQuestion.GetQuestion();

    for (int i = 0; i < answerButtons.Length; i++)
    {
      TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
      buttonText.text = currentQuestion.GetAnswer(i);
    }
  }

  void GetNextQuestion()
  {
    if (questions.Count > 0)
    {
      SetButtonState(true);
      SetDefaultButtonSprites();
      GetRandomQuestion();
      DisplayQuiz();
      progressBar.value++;
      scoreKeeper.IncrementQuestionsSeen();
    }
  }

  void GetRandomQuestion()
  {
    int index = Random.Range(0, questions.Count);
    currentQuestion = questions[index];

    if (questions.Contains(currentQuestion))
    {
      questions.Remove(currentQuestion);
    }
  }

  public void onAnswerSelected(int index)
  {
    hasAnsweredEarly = true;
    DisplayAnswer(index);

    SetButtonState(false);
    timer.CancelTimer();

    scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";

  }

  void DisplayAnswer(int index)
  {
    Image buttonImage;

    if (index == currentQuestion.GetCorrectAnswerIndex())
    {
      //Text changing when the answer is correct
      screenText.text = "Correct";

      //Temporary variable changed to the correct Image component and then sprite changed to match
      buttonImage = answerButtons[index].GetComponent<Image>();
      buttonImage.sprite = correctAnswerSprite;

      scoreKeeper.IncrementCorrectAnswers();
    }
    else
    {
      //Text changing when the answer is incorrect and shows the correct answer
      correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
      screenText.text = "Incorrect: " + currentQuestion.GetAnswer(correctAnswerIndex);

      //Temporary variable changed as done above
      buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
      buttonImage.sprite = correctAnswerSprite;
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

  void SetDefaultButtonSprites()
  {
    for (int i = 0; i < answerButtons.Length; i++)
    {
      Image buttonImage = answerButtons[i].GetComponent<Image>();
      buttonImage.sprite = defaultAnswerSprite;
    }
  }
}
