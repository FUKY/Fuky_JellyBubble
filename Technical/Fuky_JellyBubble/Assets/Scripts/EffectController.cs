using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour {

    public GameObject imageHead;
    public GameObject imageTail;
    public float speed;
    public float angle;
    public Transform targetTrans;
    public Transform beginTrans;
    public Vector3 _pos;
	// Use this for initialization
	void Start () {
        SetPosition();
	}
	
	// Update is called once per frame
	void Update () {

        SwapImage(imageHead);
        //SwapImage(imageTail);

        Move(imageHead);
        //Move(imageTail);
	
	}

    void SwapImage(GameObject image)
    {
        if (image.transform.localPosition.x > targetTrans.transform.localPosition.x - 10 || image.transform.localPosition.y > targetTrans.transform.localPosition.y - 10)
        {
            SetPosition();
        }
    }


    public void SetRotation(GameObject image, float _rotation)
    {
        image.transform.rotation = Quaternion.Euler(0, 0, _rotation);
    }

    float AngleRotation(Transform transfBegin, Transform transfEnd)
    {
        Vector3 relative = transfEnd.localPosition - transfBegin.localPosition;//transfBegin.InverseTransformPoint(transfEnd.localPosition);
        float angle1 = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        return angle1;
    }
    void Move(GameObject image)
    {
        angle = AngleRotation(beginTrans, targetTrans);       
        SetRotation(image, angle);

        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * speed * Time.deltaTime;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * speed * Time.deltaTime;

        image.transform.localPosition += new Vector3(x, y, 0);

    }
    public void SetPosition()
    {
        imageHead.transform.position = beginTrans.position;
        //imageTail.transform.position = new Vector3(pos.position - 50, pos.position.y, 0);
    }
}
