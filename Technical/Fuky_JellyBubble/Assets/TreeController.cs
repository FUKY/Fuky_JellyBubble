using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TreeController : MonoBehaviour {

    public int level;

    public List<Sprite> listImageByLevel;

    public Transform appleContainer;
    public Transform effectContainer;

    public GameObject effectGroundUp;
    public GameObject prefabApple;

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
        GameObject effect = Instantiate(effectGroundUp, Vector3.one, Quaternion.identity) as GameObject;
        effect.transform.SetParent(effectContainer);
        effect.transform.localScale = Vector3.one;
        effect.transform.localPosition = Vector3.zero;

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
        if (level <= 5)
        {
            image.sprite = listImageByLevel[level];
        }
        else 
        {
            GameObject apple = Instantiate(prefabApple, Vector3.one, Quaternion.identity) as GameObject;
            apple.transform.SetParent(appleContainer);
            apple.transform.localScale = Vector3.one;
            apple.transform.localPosition = Vector3.zero;
        }
        
        iTween.ScaleTo(gameObject, iTween.Hash(
            iT.ScaleTo.delay, timeDelay,
            iT.ScaleTo.x, 1.0,
            iT.ScaleTo.y, 1.0,
            iT.ScaleTo.time, timeScaleOut,
            iT.ScaleTo.easetype, iTween.EaseType.easeOutBack));
    }
}
