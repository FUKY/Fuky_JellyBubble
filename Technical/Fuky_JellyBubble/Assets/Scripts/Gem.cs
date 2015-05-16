using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Gem : MonoBehaviour {


    public Sprite spriteChange;

    public Sprite spriteStart;
    // Use this for initialization
    public int collumn;
    public int row;
    public int collumn1;
    public int row1;
    public int inDex;
    public bool check;
    public bool destroyCollum = false;
    public bool destroyRow = false;
    public bool cucDacBiet = false;
    public bool timeAdd = false;
    public int disX;
    public int disY;
    private GameObject a;
    Sprite start;
    Sprite changle;
    int indexStart;
    public Gem()
    {
        this.collumn = 0;
        this.row = 0;
        this.inDex = 0;
    }
    
    void Start()
    {
        start = gameObject.GetComponent<Image>().sprite;
        gameObject.GetComponent<Image>().sprite = spriteStart;
        indexStart = inDex;
        changle = spriteChange;
        ResetSprite();
    }

    // Update is called once per frame
    void Update()
    {
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
    }
  
    public void ResetSprite()
    {
        gameObject.GetComponent<Image>().sprite = spriteStart;

    }
    public void ResetSpriteStart()
    {
        start = spriteStart;
        changle = spriteChange;
        indexStart = inDex;
    }
    public void Test(GameObject obj)
    {        
        spriteStart = obj.GetComponent<Gem>().spriteStart;
        spriteChange = obj.GetComponent<Gem>().spriteChange;
        inDex = obj.GetComponent<Gem>().inDex;
        start = spriteStart;
        changle = spriteChange;
        indexStart = inDex;
        
    }
    public void ChangSpriteDacBiet(GameObject obj)
    {
        if (cucDacBiet == false)
        {
            a = obj;
            inDex = obj.GetComponent<Gem>().inDex;
            gameObject.GetComponent<Image>().sprite = obj.GetComponent<Gem>().spriteStart;
            spriteChange = obj.GetComponent<Gem>().spriteChange;
            spriteStart = obj.GetComponent<Gem>().spriteStart;
            
        }
        
    }
    public void ResetSpriteDacBiet(GameObject obj)
    {
        if (cucDacBiet == false)
        {
            inDex = indexStart;
            gameObject.GetComponent<Image>().sprite = start;
            spriteStart = start;
            spriteChange = changle;
        }
        

        //ResetSprite();
   }

    public void ResetActive()
    {
        destroyCollum = false;
        destroyRow = false;
        cucDacBiet = false;
        timeAdd = false;
        for (int i = 0; i < gameObject.transform.childCount; i++ )
        {
            Transform child = gameObject.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void  SetSprite()
    {
        if (a == null)
        {
            Debug.Log("chua vao");
            return;
        }
        else
        {
            spriteChange = a.GetComponent<Gem>().spriteChange;
            spriteStart = a.GetComponent<Gem>().spriteStart;
        }
    }
    
    public int PosX()
    {
        int posX = (int)((gameObject.transform.localPosition.x ) / (80 + disX) + 3.0f);
        return posX;
    }
    public int PosY()
    {
        int posY = (int)((gameObject.transform.localPosition.y + 36f + 10)/ (72 + disY) + 3.5f);
        return posY; 
    }
    public void MovePositionStar(Vector3 pos, float movetime)
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            iT.MoveTo.position, pos,//toi vi tri cuoi
            iT.MoveTo.islocal, true,
            iT.MoveTo.time, movetime//thoi gian
            //iT.MoveTo.easetype, iTween.EaseType.easeOutBack,//hieu ung di chuyen
            //iT.MoveTo.oncomplete, "SetParent",
            //iT.MoveTo.oncompletetarget, gameObject
            ));
    }

    public void MovePosition(Vector3 pos, float movetime)
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            iT.MoveTo.position, pos,//toi vi tri cuoi
            iT.MoveTo.islocal, true,
            iT.MoveTo.time, movetime,//thoi gian
            iT.MoveTo.easetype, iTween.EaseType.easeOutBack//hieu ung di chuyen
            //iT.MoveTo.oncomplete, "ChangleScale",
            //iT.MoveTo.oncompletetarget, gameObject
            ));
    }
    [ContextMenu("ChangleScale")]
    public void ChangleScale()
    {        

        //iTween.ValueTo(gameObject, iTween.Hash(
        //           iT.ValueTo.from, 0,
        //           iT.ValueTo.to, 1,
        //           iT.ValueTo.time, 0.3f
        //           //iT.ValueTo.onupdate, "UpdateScale"
        //           ));
    }

    void UpdateScale(float percent)
    {
        gameObject.transform.localScale = new Vector3(
            1 + (0.5f - Math.Abs(percent - 0.5f)) * (1.2f - 1),
             1 + (0.5f - Math.Abs(percent - 0.5f)) * (1.2f - 1),
             1
            );
   }

}
