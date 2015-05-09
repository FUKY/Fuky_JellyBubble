using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

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
    float delayGameOver = 0;
    // Update is called once per frame
    void Update()
    {
        if (gameOver.active == false && gameCore.active == true)
        {
            if (timeDelay > 1)
            {
                timeGame -= 1;
                
                if (timeGame < 0)
                {
                    timeGame = 0;
                    gameControl.ListDelete.Clear();
                    textScore2.text = System.String.Format("Score : {0}", gameControl.score);
                    gameOver.active = true;
                    SaveScore();
                    MoveGameOver();                        
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
        gameOver.transform.position = new Vector3(0, 900, 0);

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
    void MoveGameOver()
    {
        iTween.MoveTo(gameOver,
            iTween.Hash(
            iT.MoveTo.position, new Vector3(0, 0, 0),
            iT.MoveTo.time, 0.5f
            )
            );
    }
    public GameObject timeSecond;
    [ContextMenu("Scale")]
    
    void Scale()
    {
        iTween.ValueTo(timeSecond, iTween.Hash(
                   iT.ValueTo.from, 0,
                   iT.ValueTo.to, 1,
                   iT.ValueTo.time, 2f,
                   iT.ValueTo.onupdate, "ChangleScale",
                   iT.MoveTo.oncompletetarget, gameObject
                   ));
        
    }
    [ContextMenu("Test")]
    void Test()
    {
        ChangleScale(1);
    }
    void ChangleScale(float percent)
    {
        Debug.Log("da vao");
        timeSecond.transform.localScale = new Vector3(2, 2, 0);
        //timeSecond.transform.localScale = new Vector3(
        //    1 + (0.5f - Math.Abs(percent - 0.5f)) * (1.5f - 1),
        //     1 + (0.5f - Math.Abs(percent - 0.5f)) * (1.5f - 1),
        //     1
        //    );
    }
    
}
