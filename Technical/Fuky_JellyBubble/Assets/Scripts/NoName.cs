using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class NoName : MonoBehaviour {

    public int countDelete = 10;
    public int[] totalDelete;
    public GameObject[] list;
    private GameController gameController;
	// Use this for initialization
	void Start () {
        totalDelete = new int[6];
        gameController = GameObject.Find("Canvas").GetComponentInChildren<GameController>();

	}
	
	// Update is called once per frame
	void Update () {
        
        ChangleFillAmount();
	}
    void ChangleFillAmount()
    {
        countDelete = gameController.ListDelete.Count;
        gameObject.GetComponent<Image>().fillAmount = (1 - countDelete * 0.1f);
        if (gameObject.GetComponent<Image>().fillAmount <= 0)
        {
            gameController.activeAddtime = true;
        }
        
    }
    public void ResetGame()
    {
        totalDelete = new int[6];
    }
    public void Test(int count, int i)
    {
        if (totalDelete[i] > 15)
        {
            int rand = Random.Range(60, 100);

            if (rand < 20)
            {
                gameController.indexRandom = 2;
            }
            if (rand > 20 && rand < 60)
            {
                gameController.indexRandom = 1;
            }
            if (rand > 60)
            {
                gameController.indexRandom = 0;
            }
            totalDelete[i] = 0;
            gameController.activeInstanDacBiet1 = true;
            
        }
    }
}
