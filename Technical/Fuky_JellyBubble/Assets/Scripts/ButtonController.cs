using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    public GameObject GameStart;
    public GameObject GamePlay;
    public GameObject GameOver;
    public GameObject HighScore;
    public bool isMute = false;
    public Button buttonMute;

    void Start()
    {

    }

    void Update()
    {

    }

    public void StartButton()
    {
        gameObject.SetActive(false);
        GamePlay.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void Replay()
    {
        gameObject.SetActive(false);
        GameStart.SetActive(true);
    }

    public void ToStartMenu()
    {
        gameObject.SetActive(false);
        GameStart.SetActive(true);
    }

    public void ToHighScore()
    {
        gameObject.SetActive(false);
        HighScore.SetActive(true);
    }

    public void MutePress()
    {
        isMute = true;

    }
}