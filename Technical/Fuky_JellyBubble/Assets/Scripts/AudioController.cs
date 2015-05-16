using UnityEngine;
using System.Collections;

public enum AudioType
{
    SOUND_BG_INGAME = 0,
    LEVEL_UP = 1,
    BUTTON_CLICK = 2,
    MENU_OFF = 3,
    REMOVE_LIGHT = 4,
    REMOVE_SOIL = 5,
    REMOVE_TRASH = 6,
    REMOVE_WATER = 7,
    REMOVE_WORM = 8,
    SG_MERGER1 = 9,
    SG_MERGER2 = 10,
    SG_MERGER3 = 11,
    SG_MERGER4 = 12,
    SG_MERGER5 = 13,
    SG_MERGER6 = 14,
    SG_MERGER7 = 15,
    SG_MERGER8 = 16,
    SG_MERGER9 = 17,
    SOUND_BG_OUT_GAME = 18
};


public class AudioController : MonoSingleton<AudioController> {

    public AudioClip[] audioClip;

    public bool isSoundBG;
    public bool isSoundGamePlay;

    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}   
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(AudioType audioType) 
    {
        if (isSoundGamePlay)
        {
            audioSource.PlayOneShot(audioClip[(int)audioType]);
        }
        
    }

    public void PressMute() 
    {
        isSoundGamePlay = !isSoundGamePlay;
    }
}
