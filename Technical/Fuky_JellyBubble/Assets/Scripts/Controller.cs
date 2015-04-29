using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject uI;
    public GameObject gameCore;
     public GameObject timeImgage;

    public float timeGame = 60;
    public Text textTimeSecond;
    public Text textScore;
    public Text textScore2;
   

    private float timeDelay = 0;
    private GameController gameControl;

    // Use this for initialization
    void Start()
    {
        gameControl = gameObject.GetComponentInChildren<GameController>();
        timeDelay = 0;
        timeGame = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver.active == false && uI.active == true && gameCore.active == true)
        {
            if (timeDelay > 1)
            {
                timeGame -= 1;
                if (timeGame <= 0)
                {
                    textScore2.text = System.String.Format("Score : {0}", gameControl.score);
                    gameOver.active = true;
                    uI.active = false;
                    gameCore.active = false;

                }
                timeDelay = 0;
            }
            timeDelay += Time.deltaTime;
            textTimeSecond.text = timeGame.ToString();
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
        gameOver.active = false;
        uI.active = true;
        gameCore.active = true;
        Application.LoadLevel(Application.loadedLevel);
    }
    public void AddTime()
    {
        timeGame += 10;
    }
}
