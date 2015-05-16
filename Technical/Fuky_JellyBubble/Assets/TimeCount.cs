using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour {

    public Image image;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (image.GetComponent<Image>().fillAmount > 0)
        {
            TimeCountDown();
        }
	}

    void TimeCountDown()
    {
        image.GetComponent<Image>().fillAmount -= (float)1 / (float)60 * Time.deltaTime;
    }
}
