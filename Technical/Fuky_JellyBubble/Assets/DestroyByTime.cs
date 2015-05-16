using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
    public float timeDestroy;

    private float timeDestroyCur;
	// Use this for initialization
	void Start () {
        timeDestroyCur = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timeDestroyCur += Time.deltaTime;
        if (timeDestroyCur >= timeDestroy)
        {
            Destroy(gameObject);
        }
        
	}
}
