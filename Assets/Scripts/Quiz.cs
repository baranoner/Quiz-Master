using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questiontext;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
     QuestionSO currentQuestion;  

    
    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    
    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    
    
    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Timer")]
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
    
    void Update(){
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion){

            if(progressBar.value == progressBar.maxValue){
            isComplete = true;
            return;
        }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion){
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void GetNextQuestion(){
        if(questions.Count > 0){
        SetButtonState(true);
        SetDefaultButtonSprites();
        GetRandomQuestion();
        DisplayQuestion();
        progressBar.value++;
        scoreKeeper.IncrementQuestionsSeen();
        }
        
        
    }
    void GetRandomQuestion(){
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        questions.Remove(currentQuestion);
    }
    public void OnAnswerSelected(int index){
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + " %" ;

        
    }
    void DisplayAnswer(int index){
         if(index == currentQuestion.GetCorrectAnswerIndex()){
            questiontext.text = "Correct!";
            Image buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite; 
            scoreKeeper.IncrementCorrectAnswers();
        }
        else{
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            questiontext.text = "The correct answer was\n " + currentQuestion.GetAnswer(correctAnswerIndex); 
            Image buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite; 
        }
    }
    void DisplayQuestion(){
        questiontext.text = currentQuestion.GetQuestion();
        for(int i = 0; i < answerButtons.Length; i++){
        TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = currentQuestion.GetAnswer(i);
        }
}
    void SetButtonState(bool state){
        for(int i = 0; i < answerButtons.Length; i++){
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
    void SetDefaultButtonSprites(){
        for(int i = 0; i < answerButtons.Length; i++){
        Image buttonImage = answerButtons[i].GetComponent<Image>();
        buttonImage.sprite = defaultAnswerSprite;
        }
        
    }
}

