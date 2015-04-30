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
    public bool destroyColRow = false;
    public bool cucDacBiet = false;
    public bool timeAdd = false;

    public bool activaChangeSprite = false;
    public bool activateChangeDacBiet = false;

    private GameObject a;
    Sprite start;
    public Gem()
    {
        this.collumn = 0;
        this.row = 0;
        this.inDex = 0;
    }

    void Start()
    {
        //
        activaChangeSprite = false;
        activateChangeDacBiet = false;
        //spriteStart = gameObject.GetComponent<Image>().sprite;
        gameObject.GetComponent<Image>().sprite = spriteStart;
        start = gameObject.GetComponent<Image>().sprite;
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
    public void ChangSpriteDacBiet(GameObject obj)
    {
        activateChangeDacBiet = true;
        a = obj;        
        if (cucDacBiet == false)
        {
            inDex = obj.GetComponent<Gem>().inDex;
            gameObject.GetComponent<Image>().sprite = obj.GetComponent<Gem>().spriteStart;
            spriteChange = obj.GetComponent<Gem>().spriteChange;
            spriteStart = obj.GetComponent<Gem>().spriteStart;
        }
        
    }
    public void ResetSpriteDacBiet(GameObject obj)
    {
        activateChangeDacBiet = false;
        inDex = obj.GetComponent<Gem>().inDex;
        gameObject.GetComponent<Image>().sprite = start;
        
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
            iT.MoveTo.easetype, iTween.EaseType.easeOutBack,//hieu ung di chuyen
            iT.MoveTo.oncomplete, "ChangleScale",
            iT.MoveTo.oncompletetarget, gameObject));
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
