using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TreeController : MonoBehaviour {

    public int level;

    public List<Sprite> listImageByLevel;

    private Image image;

    public float timeScaleIn;
    public float timeDelay;
    public float timeScaleOut;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        //SetLevel(2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetLevel(int _level) 
    {
        level = _level;
        ScaleIn();
    }

    [ContextMenu("TestLevel")]
    public void TestLevel() 
    {
        SetLevel(3);
    }

    public void ScaleIn()
    {
        Debug.Log("Scale In");
        iTween.ScaleTo(gameObject, iTween.Hash(
            iT.ScaleTo.x, 0.5,
            iT.ScaleTo.y, 0.5,
            iT.ScaleTo.time, timeScaleIn,
            iT.ScaleTo.oncomplete, "ScaleOut",
            iT.ScaleTo.oncompletetarget, gameObject));
    }

    public void ScaleOut()
    {
        Debug.Log("Scale Out");
        image.sprite = listImageByLevel[level - 1];
        iTween.ScaleTo(gameObject, iTween.Hash(
            iT.ScaleTo.delay, timeDelay,
            iT.ScaleTo.x, 1.0,
            iT.ScaleTo.y, 1.0,
            iT.ScaleTo.time, timeScaleOut,
            iT.ScaleTo.easetype, iTween.EaseType.easeOutBack));
    }
}
