using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Quiz quiz;
    EndScreen endscreen;

    void Awake(){
        quiz = FindObjectOfType<Quiz>();
        endscreen = FindObjectOfType<EndScreen>();
    }
    void Start()
    {
        quiz.gameObject.SetActive(true);
        endscreen.gameObject.SetActive(false);
    }

    
    void Update()
    {
        if(quiz.isComplete){
        quiz.gameObject.SetActive(false);
        endscreen.gameObject.SetActive(true);
        endscreen.ShowFinalScore();
        }
    }
    public void OnReplayLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
