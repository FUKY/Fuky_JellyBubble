using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject gameCore;
    public GameObject timeImgage;
    public GameObject gameStart;

    public float timeGame = 60;
    public Text textTimeSecond;
    public Text textScore;
    public Text textScore2;
    public Text hightScore;
   

    private float timeDelay = 0;
    private GameController gameControl;
    void ReStart()
    {
        timeGame = 60;
        timeDelay = 0;
    }

    // Use this for initialization
    void Start()
    {
        timeDelay = 0;
        timeGame = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver.active == false && gameCore.active == true)
        {
            if (timeDelay > 1)
            {
                timeGame -= 1;
                if (timeGame <= 0)
                {
                    textScore2.text = System.String.Format("Score : {0}", gameControl.score);
                    
                    if (gameControl.activeDestroyGem == true)
                    {
                        gameCore.active = false;
                        gameOver.active = true;
                    }
                    
                    SaveScore();
                }
                timeDelay = 0;
            }
            timeDelay += Time.deltaTime;
            textTimeSecond.text = timeGame.ToString();
            if (gameControl != null)
                textScore.text = System.String.Format("Score : {0}", gameControl.score);
             
            UpdateTime();
        }
        
    }
    void UpdateTime()
    {

        timeImgage.transform.localScale = new Vector3((timeGame * 0.0166f), 1f, 1f);
        timeImgage.GetComponent<Image>().color = Color.Lerp(Color.green, Color.red, 1 - (timeGame * 0.0166f));
    }
    public void ReLayGame()
    {
        gameStart.active = false;
        gameOver.active = false;
        gameCore.active = true;
        ReStart();
        gameControl.RandomMap();

    }
    public void AddTime()
    {
        timeGame += 10;
    }
    public void SaveScore()
    {
        if(gameControl.score > PlayerPrefs.GetInt("Score"))
        {
            PlayerPrefs.SetInt("Score", gameControl.score);
            PlayerPrefs.Save();
        }
        hightScore.text = "Hight Score: " + PlayerPrefs.GetInt("Score");
    }
    public void GameStart()
    {
        gameStart.active = false;
        gameCore.active = true;
        gameControl = gameObject.GetComponentInChildren<GameController>();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
