using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour {

    public int distance;

    public GameObject imageHead;
    public GameObject imageTail;
    public float speed;
    public float angle;
    //public Transform targetTrans;
    //public Transform beginTrans;

    public Vector3 posBegin;
    //public Vector3 _pos;
	// Use this for initialization
	void Start () {
        //SetPosition();
	}
	
	// Update is called once per frame
	void Update () {

        SwapImage(imageHead);
        SwapImage(imageTail);

        Move(imageHead);
        Move(imageTail);
	
	}

    void SwapImage(GameObject image)
    {
        if (image.transform.localPosition.x > posBegin.x + distance)
        {
            SetPosBegin(image);
            //SetPosition();
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
        //angle = AngleRotation(beginTrans, targetTrans);       
        //SetRotation(image, angle);

        //float x = Mathf.Cos(Mathf.Deg2Rad * angle) * speed * Time.deltaTime;
        //float y = Mathf.Sin(Mathf.Deg2Rad * angle) * speed * Time.deltaTime;

        float x = speed * Time.deltaTime;


        image.transform.localPosition += new Vector3(x, 0, 0);

    }

    public void SetPosBegin(GameObject image) 
    {
        image.transform.localPosition -= new Vector3(distance * 2, 0, 0); ;
    }

    public void SetPositionBetweenTwoGem(Transform gem1, Transform gem2) 
    {
        transform.position = (gem1.position + gem2.position) / 2;
        float angleConnect = AngleRotation(gem1, gem2);
        transform.rotation = Quaternion.Euler(0, 0, angleConnect);
    }

    //public void SetPosition()
    //{
    //    imageHead.transform.position = beginTrans.position;
    //    //imageTail.transform.position = new Vector3(pos.position - 50, pos.position.y, 0);
    //}
}
