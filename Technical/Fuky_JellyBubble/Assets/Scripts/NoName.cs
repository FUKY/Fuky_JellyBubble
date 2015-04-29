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
        if (gameObject.tag == "Time")
        {
            gameObject.GetComponent<Image>().fillAmount = (1 - countDelete * 0.1f);
            if (gameObject.GetComponent<Image>().fillAmount <= 0)
            {
                gameController.activeAddtime = true;
            }
        }
    }
    public void Test(int count, int i)
    {
        if (list[i].GetComponent<Image>() == null)
        {
            return;
        }
        list[i].GetComponent<Image>().fillAmount = (1 - count * 0.05f);
        if (list[i].GetComponent<Image>().fillAmount <= 0)
        {
            gameController.indexRandom = i;
            gameController.activeInstanDacBiet1 = true;
            list[i].GetComponent<Image>().fillAmount = 1;             
            totalDelete[i] = 0;
        } 
    }
}
