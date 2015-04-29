using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Gem : MonoBehaviour {


    public Sprite spriteChange;

    private Sprite spriteStart;
    // Use this for initialization
    public int collumn;
    public int row;
    public int collumn1;
    public int row1;
    public int inDex;
    public bool check;
    public bool destroyCollum = false;
    public bool destroyRow = false;
    public bool destroyColRow = false;
    public bool cucDacBiet = false;
    public bool timeAdd = false;

    public bool activaChangeSprite = false;
    public bool activateChangeDacBiet = false;

    private GameObject a;
    List<GameObject> b = new List<GameObject>();
    Sprite start;
    public Gem()
    {
        this.collumn = 0;
        this.row = 0;
        this.inDex = 0;
    }

    void Start()
    {
        start = gameObject.GetComponent<Image>().sprite;
        activaChangeSprite = false;
        activateChangeDacBiet = false;
        spriteStart = gameObject.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
       this.row1 = (int)PosX();
       this.collumn1 = (int)PosY();

    }

    public void SetProfile(int col, int row, int index)
    {
        this.collumn = col;
        this.row = row;
        this.inDex = index;
    }
    public void ChangSprite()
    {

        gameObject.GetComponent<Image>().sprite = spriteChange;
        //if (activateChangeDacBiet == false)
        //    gameObject.GetComponent<Image>().sprite = spriteChange;
        //else
        //{
        //    gameObject.GetComponent<Image>().sprite = a.GetComponent<Gem>().spriteChange;
        //}
    }
    public void ResetSprite()
    {
        if (activateChangeDacBiet == false)
            gameObject.GetComponent<Image>().sprite = spriteStart;
        else
        {
            gameObject.GetComponent<Image>().sprite = spriteStart;
        }
        
    }
    bool m = false;
    public void ChangSpriteDacBiet(GameObject obj)
    {
        activateChangeDacBiet = true;
        a = obj;        
        if (cucDacBiet == false)
        {
            gameObject.tag = obj.tag;
            gameObject.GetComponent<Image>().sprite = obj.GetComponent<Gem>().spriteStart;
            spriteChange = obj.GetComponent<Gem>().spriteChange;
            spriteStart = obj.GetComponent<Gem>().spriteStart;
            //ChangSprite();
            if (m == true)
            {
                gameObject.GetComponent<Gem>().spriteChange = obj.GetComponent<Gem>().spriteChange;
                gameObject.GetComponent<Gem>().spriteStart = obj.GetComponent<Gem>().spriteStart;
            }

        }
        
    }
    public void ResetSpriteDacBiet(GameObject obj)
    {
        activateChangeDacBiet = false;
        gameObject.tag = obj.tag;
        gameObject.GetComponent<Image>().sprite = start;
        
   }
    public void ResetActive()
    {
        activateChangeDacBiet = false;
        activaChangeSprite = false;
    }
    public int PosX()
    {
        int posX = (int)(gameObject.transform.localPosition.x / 80 + 3.0f);
        return posX;
    }
    public int PosY()
    {
        int posY = (int)((gameObject.transform.localPosition.y+ 36f )/ 72 + 3.5f);
        return posY; 
    }

}
