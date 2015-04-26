using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {

    // Use this for initialization
    public int column;
    public int row;
    public int inDex;
    public bool check;
    public bool destroyCollum = false;
    public bool destroyRow = false;
    public bool destroyColRow = false;
    public bool cucDacBiet = false;

    public Gem()
    {
        this.column = 0;
        this.row = 0;
        this.inDex = 0;
    }

    void Start()
    {

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
}
