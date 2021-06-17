using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    //GAMEOVERPART normal parttan fazla olursa crash!!

    public int best;
    public int score;
    public int currentStage = 0;

    public UIManager uiManager;

    public static GameManager singleton;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Advertisement.Initialize("4153433");

        int a = PlayerPrefs.GetInt("Highscore");
        uiManager.textBest.text = "Best: " + a;
    }

    private void Update()
    {
        
    }

    public void NextLevel()
    {
        currentStage++;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void RestartLevel()
    {
        AddScore(-score);
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);

        //SHOW ADS
        Advertisement.Show();
    }

    public void AddScore(int scoreToAdd)
    {
        int a = PlayerPrefs.GetInt("Highscore");
        score = score + scoreToAdd;
        uiManager.textScore.text = score.ToString();
        if (score > a)
        {
            PlayerPrefs.SetInt("Highscore", score);
            uiManager.textBest.text = "Best: " + score;
        }
    }

}
