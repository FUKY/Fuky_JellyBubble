using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TreeController : MonoBehaviour {

    private int level;

    public int score_level;

    public int disX;
    public int disY;

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
    [ContextMenu("Test")]
    void Test()
    {
        SetLevel(5);
    }

    public void SetLevel(int _level) 
    {
        level = _level;
        
        ScaleIn();
        AudioController.Instance.PlaySound(AudioType.LEVEL_UP);
    }

    [ContextMenu("TestLevel")]
    public void TestLevel() 
    {
        SetLevel(6);
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
        if (level < 5)
        {
            image.sprite = listImageByLevel[level];
        }
        else 
        {
            image.sprite = listImageByLevel[4];
            GameObject apple = Instantiate(prefabApple, Vector3.one, Quaternion.identity) as GameObject;
            apple.transform.SetParent(appleContainer);
            apple.transform.localScale = Vector3.one;

            int x = Random.Range(-disX, disX);
            int y = Random.Range(-disY, disY);

            apple.transform.localPosition = new Vector3(x, y, 0);
        }
        
        iTween.ScaleTo(gameObject, iTween.Hash(
            iT.ScaleTo.delay, timeDelay,
            iT.ScaleTo.x, 1.0,
            iT.ScaleTo.y, 1.0,
            iT.ScaleTo.time, timeScaleOut,
            iT.ScaleTo.easetype, iTween.EaseType.easeOutBack));
    }
}
