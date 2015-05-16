using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



public class SetScore : MonoBehaviour {

    public Text score;
    public Text highScoreText;
    int highScore = 0;

    void Start()
    {

    }

    void SetPoint()
    {
        if (int.Parse(score.text) > highScore)
        {
            highScore = int.Parse(score.text);
            PlayerPrefs.SetInt("score", highScore);
            PlayerPrefs.Save();
        }
    }

    void LoadScore()
    {
        highScoreText.text = PlayerPrefs.GetInt("score").ToString();
    }
}
