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
        GameStart.SetActive(false);
        GamePlay.SetActive(true);
        PlayButtonClick();
    }

    public void ExitButton()
    {
        PlayButtonClick();
        Application.Quit();
    }

    public void Replay()
    {
        PlayButtonClick();
        GamePlay.SetActive(true);
        GameOver.SetActive(false);
        GamePlay.GetComponentInChildren<GameController>().RandomMap();
    }

    public void ToStartMenu()
    {

        PlayButtonClick();
        
        HighScore.SetActive(false);
        GameStart.SetActive(true);
    }

    public void ToHighScore()
    {
        PlayButtonClick();
        GameStart.SetActive(false);
        HighScore.SetActive(true);
    }

    public void MutePress()
    {
        isMute = true;
        AudioController.Instance.PressMute();
    }

    public void PlayButtonClick() 
    {
        AudioController.Instance.PlaySound(AudioType.BUTTON_CLICK);
    }
    public void CheckGameOver()
    {
        GameOver.SetActive(true);
        //GamePlay.SetActive(false);
    }
}