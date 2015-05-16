using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingBntController : MonoBehaviour {

    public AudioSource audioGameBG;
    public AudioSource audioGamePlay;

    public Sprite imgBntSound1;
    public Sprite imgBntSound2;

    public Sprite imgBntSoundPlay1;
    public Sprite imgBntSoundPlay2;


    public GameObject btnSetting;

    public GameObject[] item;
    public int[] posX;

    public float timeHide;
    public bool isShow;

    private float timeHideCur;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (isShow)
	    {
            timeHideCur += Time.deltaTime;
            if (timeHideCur >= timeHide)
            {
                Hide();
                timeHideCur = 0;
            }
        }
        else 
        {
            timeHideCur = 0;
        }
	    
	}

    public float timeMove;
    public float timeDelay;

    [ContextMenu("Show")]
    public void Show() 
    {
        isShow = true;
        timeHideCur = 0;
        for (int i = 0; i < item.Length; i++)
        {
            //item[i].transform.localPosition = new Vector3(posX[i], 0, 0);
            iTween.MoveTo(item[i], iTween.Hash(
                iT.MoveTo.x, posX[i],
                iT.MoveTo.islocal, true,
                iT.MoveTo.time, timeMove,
                iT.MoveTo.delay, timeDelay));
        }
        
    }

    [ContextMenu("Hide")]
    public void Hide() 
    {
        isShow = false;

        for (int i = 0; i < item.Length; i++)
        {
            //item[i].transform.localPosition = new Vector3(posX[i], 0, 0);
            iTween.MoveTo(item[i], iTween.Hash(
                iT.MoveTo.x, 0,
                iT.MoveTo.islocal, true,
                iT.MoveTo.time, timeMove,
                iT.MoveTo.delay, timeDelay));
        }
    }

    public void PressBntSetting() 
    {
        AudioController.Instance.PlaySound(AudioType.BUTTON_CLICK);
        if (isShow)
        {
            Hide();
            isShow = false;
        }
        else {
            isShow = true;
            Show();
        }
    }

    public void PressBntSoundBG() 
    {
        AudioController.Instance.PlaySound(AudioType.BUTTON_CLICK);
        timeHideCur = 0;
        AudioController.Instance.PressMuteSoundBG();

        audioGameBG.enabled = AudioController.Instance.isSoundBG;

        Image image = item[0].GetComponent<Image>();
        if (AudioController.Instance.isSoundBG)
        {
            image.sprite = imgBntSound1;
        }
        else 
        {
            image.sprite = imgBntSound2;
        }
        
    }

    public void PressBntSoundPlay() 
    {
        AudioController.Instance.PlaySound(AudioType.BUTTON_CLICK);
        timeHideCur = 0;
        AudioController.Instance.PressMute();
        audioGamePlay.enabled = AudioController.Instance.isSoundGamePlay;
        Image image = item[1].GetComponent<Image>();
        if (AudioController.Instance.isSoundGamePlay)
        {
            image.sprite = imgBntSoundPlay1;
        }
        else
        {
            image.sprite = imgBntSoundPlay2;
        }
    }
}
