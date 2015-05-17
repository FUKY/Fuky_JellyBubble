using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HightScore : MonoBehaviour {

    public GameController gameController;
    public Text score;
    public Text highScoreText;
    int highScore = 0;
	// Use this for initialization
	void Start () {

        SaveScore1();
        
	}
	
	// Update is called once per frame
	void Update () {
        SaveScore1();
        LoadScore();
	}
    void SaveScore()
    {
        if (gameController == null)
        {
            return;
        }
        else

        if (gameController.score > highScore)
        {
            //highScore = int.Parse(score.text);

            PlayerPrefs.SetInt("score", highScore);
            PlayerPrefs.Save();
        }
    }
    public void SaveScore1()
    {
        if (gameController == null)
        {
            return;
        }
        else
        {
            if (gameController.score > PlayerPrefs.GetInt("Score"))
            {
                PlayerPrefs.SetInt("Score", gameController.score);
                PlayerPrefs.Save();
            }
            highScoreText.text = "Hight Score: " + PlayerPrefs.GetInt("Score");
        }

    }
    void LoadScore()
    {
        if (gameController == null)
        {
            return;
        }
        else
            score.text = gameController.score.ToString();
    }
}
