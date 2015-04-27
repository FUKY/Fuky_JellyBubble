using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Gem : MonoBehaviour {


    public Sprite spriteChange;

    private Sprite spriteStart;
    // Use this for initialization
    public int column;
    public int row;
    public int inDex;
    public bool check;
    public bool destroyCollum = false;
    public bool destroyRow = false;
    public bool destroyColRow = false;
    public bool cucDacBiet = false;
    private bool activaChangeSprite;
    private bool activateChangeDacBiet;

    private GameObject a;
    List<GameObject> b = new List<GameObject>();
    public Gem()
    {
        this.column = 0;
        this.row = 0;
        this.inDex = 0;
    }

    void Start()
    {
        activaChangeSprite = false;
        activateChangeDacBiet = false;
        spriteStart = gameObject.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetProfile(int col, int row, int index)
    {
        this.column = col;
        this.row = row;
        this.inDex = index;
    }
    public void ChangSprite()
    {
        if (activateChangeDacBiet == false)
            gameObject.GetComponent<Image>().sprite = spriteChange;
        else
        {
            gameObject.GetComponent<Image>().sprite = a.GetComponent<Gem>().spriteChange;
        }
        activaChangeSprite = true;
    }
    public void ResetSprite()
    {
        if (activateChangeDacBiet == false)
            gameObject.GetComponent<Image>().sprite = spriteStart;
        else
        {
            gameObject.GetComponent<Image>().sprite = a.GetComponent<Gem>().spriteStart;
        }
        activaChangeSprite = false;
    }

    public void ChangSpriteDacBiet(GameObject obj)
    {
        activateChangeDacBiet = true;
        a = obj;
        
        if (cucDacBiet == false)
        {
            gameObject.tag = obj.tag;
            gameObject.GetComponent<Image>().sprite = obj.GetComponent<Gem>().spriteStart;
            if (activaChangeSprite == false)
                gameObject.GetComponent<Gem>().spriteChange = obj.GetComponent<Gem>().spriteChange;

        }
        
    }
    public void ResetSpriteDacBiet(GameObject obj)
    {
        gameObject.tag = obj.tag;
        gameObject.GetComponent<Image>().sprite = spriteStart;
        activateChangeDacBiet = false;
    }
    public void ResetActive()
    {
        activateChangeDacBiet = false;
        activaChangeSprite = false;
    }
}
