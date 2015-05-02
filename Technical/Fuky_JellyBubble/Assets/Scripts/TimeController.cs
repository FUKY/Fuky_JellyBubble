using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour {

    private Controller controllGUI;
    private GameController gameController;
    
	// Use this for initialization
    
	void Start () {
        gameController = GameObject.Find("Canvas").GetComponentInChildren<GameController>();
        controllGUI = gameObject.GetComponentInParent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void UpdatePositionItween()
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            iT.MoveTo.position, gameController.tranfsOut.position,//toi vi tri cuoi
            iT.MoveTo.time, 1.0f,//thoi gian
            //iT.MoveTo.easetype, iTween.EaseType.easeOutBack,//hieu ung di chuyen
            iT.MoveTo.oncomplete, "AddTime",
            iT.MoveTo.oncompletetarget, gameObject));

    }
    //[ContextMenu("UpdateParent")]
    public void UpdateParent()
    {
        gameObject.transform.parent = gameController.gameObject.transform;
        gameObject.transform.localScale = Vector3.one;
        UpdatePositionItween();
    }
    void AddTime()
    {
        controllGUI.AddTime();
        Destroy(gameObject);
    }
}
