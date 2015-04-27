using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public GameObject[] listGem;//list cac gem_Prefabs
    public GameObject[] cucDacBiet;//list cac gem_Prefabs
    public GameObject conect;
    public GameObject destroyGem;
    public Sprite[] image;

    public float x, y;//vi tri camera
    public int countRow;//so hang cua mang
    public int countCollumn;//so cot cua mang
    public float iTwenPos;//vi tri in ra luc dau
    public float localScale = 1;//kich thuc gem
    public bool activeTimeHelp = false;
    public float timeHelp = 5;
    public bool activeDestroyGem = false;



    private GameObject[][] arrGem;//list Game Object hien ra man hinh
    private RaycastHit2D rayHit;
    private List<GameObject> ListDelete = new List<GameObject>();//list Object de xoa
    private List<GameObject> listConect = new List<GameObject>();//tao lien ket cho cac cuc(sau nay thanh thanh Animation)
    private List<List<GameObject>> listLoangDau = new List<List<GameObject>>();//kiem tra con duong nao de an khong
    private List<GameObject> listMouse = new List<GameObject>();
    private int index;//so thu tu cac Prefabs   
    private float scale = 0.01f;
    private bool boolScale = false;
    private bool activeHelp;
    private float help;
    private Gem gem;
    private int vitriX, vitriY;
    private bool activeInstanDacBiet1 = false;
    private bool activeInstanDacBiet2 = false;

	// Use this for initialization
	void Start () {
        
        activeTimeHelp = true;
        RandomGem(iTwenPos);// random hinh anh khi moi dua vao game o vi tri ItweenPos	
        CheckListInvalid();
	}
	
	// Update is called once per frame
	void Update () {
        DestroyButtonMouse();
	}
    void FixedUpdate()
    {
        if(activeDestroyGem == true)
        {
            if (activeInstanDacBiet1 == true && activeInstanDacBiet2 == true)
            {
                InstantiateItemDacBiet(); 
            }            
            CacCucRoiXuong();
        }
        
        if (activeTimeHelp == true)
        {
            localScale += scale;
            if (localScale > 1.2)
            {
                scale = -0.01f;
            }
            if (localScale < 0.8)
            {
                scale = 0.01f;
            }

            if (help > timeHelp)
            {
                activeHelp = true;
            }
            help += Time.deltaTime;
        }
        if (activeTimeHelp == false)
        {
            activeHelp = false;
            help = 0;
        }
        if (activeHelp == true)
        {
            ScaleGem();
        }
        if (activeHelp == false)
        {
            ResetScaleGem();
        }
        if (listLoangDau.Count == 0)//neu k con duong nao de an Ramdom lai map
        {
            RandomMap();
        }        
    }

    void RandomGem(float posIT)
    {
        arrGem = new GameObject[countCollumn][];

        for (int i = 0; i < countCollumn; i++)
        {
            arrGem[i] = new GameObject[countRow];

        }
        for (int i = 0; i < countCollumn; i++)
        {
            for (int j = 0; j < countRow; j++)
            {
                InstantiateGem(i, j, posIT);//in ra cac Object o vi tri PosIT
            }
        }
    }
    void InstantiateGem(int row, int collumn, float posItween)
    {
        index = Random.Range(0, 5);
        GameObject a = Instantiate(listGem[index], new Vector3(row * 0.75f - x, collumn * 0.75f - y + posItween, 0), Quaternion.identity) as GameObject;
        arrGem[row][collumn] = a;
        //add vao Canvas
        a.transform.SetParent(transform);
        a.transform.localScale = Vector3.one;
        gem = a.GetComponent<Gem>();
        gem.SetProfile(collumn, row, index);
 
    }
    void InstantiateConect(GameObject gameObj1, GameObject gameObj2)
    {
        float x, y;
        x = (gameObj1.transform.position.x + gameObj2.transform.position.x) / 2;
        y = (gameObj1.transform.position.y + gameObj2.transform.position.y) / 2;

        GameObject a = Instantiate(conect, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        a.transform.SetParent(transform);
        a.transform.localScale = Vector3.one;
        listConect.Add(a);
        Vector3 relative = gameObj1.transform.InverseTransformPoint(gameObj2.transform.position);
        float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        a.transform.Rotate(0, 0, -angle);
    }
    void Xoa()
    {
        //xoa cac cuc        
        if (ListDelete.Count >= 3)
        {
            //kiem tra xem cac cuc dac biet co o trong listDelete khong
            for (int i = 0; i < countCollumn; i++)
            {
                for (int j = 0; j < countRow; j++)
                {
                    if (arrGem[i][j].GetComponent<Gem>().destroyCollum == true && ListDelete.Contains(arrGem[i][j]))
                    {
                        NoTheoChieuNgang(i);
                    }
                    if (arrGem[i][j].GetComponent<Gem>().destroyRow == true && ListDelete.Contains(arrGem[i][j]))
                    {
                        NoTheoChieuDoc(j);
                    }

                }
            }
            //xoa cac Gem trong listDelete
            for (int i = 0; i < ListDelete.Count; i++)
            {
                Destroy(ListDelete[i]);
                Instantiate(destroyGem, ListDelete[i].transform.position, Quaternion.identity);
                //score += 100;
            }
        }
        //xoa listConect
        for (int i = 0; i < listConect.Count; i++)
        {
            Destroy(listConect[i]);
        }

        ListDelete.Clear();
        listConect.Clear();
        listMouse.Clear();
    }
    float KiemTraKhoangCach(GameObject a, GameObject b)
    {
        float khoangCach = Vector3.Distance(a.transform.position, b.transform.position);
        return khoangCach;
    }
    int TimViTriX(GameObject vitrClick)
    {
        int posX;
        posX = (int)(vitrClick.transform.position.x / 0.75 + x + 1.2f);

        return posX;
    }
    int TimViTriY(GameObject vitrClick)
    {
        int posY;
        posY = (int)(vitrClick.transform.position.y / 0.75 + y + 1.2f);
        return posY;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        activeTimeHelp = false;
        activeDestroyGem = false;
        rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (rayHit.collider == null)
        {
            return;
        }
        else
        {
            
            if (ListDelete.Count == 0)
            {
                ListDelete.Add(rayHit.collider.gameObject);
                listMouse.Add(rayHit.collider.gameObject);
                ListDelete[ListDelete.Count -1].GetComponent<Gem>().ChangSprite();
            }
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (ListDelete.Count == 1)
        {
            if (ListDelete[0] != null)
                ListDelete[0].GetComponent<Gem>().ResetSprite();
        }
        Xoa();
        
        activeTimeHelp = true;
        activeInstanDacBiet2 = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (ListDelete == null)
        {
            return;
        }

        if (rayHit.collider == null)
        {
            return;
        }

        if (ListDelete.Count <= 0 || ListDelete[0] == null)
        {
            return;
        }

        if (rayHit.collider.gameObject.tag == ListDelete[0].tag && KiemTraKhoangCach(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]) <= 1.2f)//kiem tra de dua vao listDelete
        {
            if (!listMouse.Contains(rayHit.collider.gameObject))
            {
                listMouse.Add(rayHit.collider.gameObject);
            }
            if (!ListDelete.Contains(rayHit.collider.gameObject) && ListDelete.Count >= 1)//kiem tra xem doi tuong chon da co trong ListDelete chua
            {
                
                InstantiateConect(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]);//xuat ket noi ra man hinh
                ListDelete.Add(rayHit.collider.gameObject);
                ListDelete[ListDelete.Count - 1].GetComponent<Gem>().ChangSprite();
                
                if (ListDelete.Count == 5)
                {
                    activeInstanDacBiet1 = true;
                }
                if (ListDelete.Contains(rayHit.collider.gameObject) && rayHit.collider.gameObject.GetComponent<Gem>().cucDacBiet == true)
                {
                    CucDacBiet(TimViTriX(rayHit.collider.gameObject), TimViTriY(rayHit.collider.gameObject));
                }

                
            }
            if (ListDelete.Count >= 2)
            {
                if (rayHit.collider.gameObject == ListDelete[ListDelete.Count - 2] && listConect.Count >= 1)//neu nguoi choi quay lai cuc phia trc co
                {
                    ListDelete[ListDelete.Count - 1].GetComponent<Gem>().ResetSprite();
                    ListDelete.RemoveAt(ListDelete.Count - 1);                    
                    Destroy(listConect[listConect.Count - 1]);
                    listConect.RemoveAt(listConect.Count - 1);
                }
            }

            for (int i = 0; i < listMouse.Count; i++)
            {
                if (listMouse[i].GetComponent<Gem>().cucDacBiet == true)
                {
                    GameObject a = listMouse[i];
                    if (!ListDelete.Contains(listMouse[i]))
                    {
                        ResetCucDacBiet(TimViTriX(listMouse[i]), TimViTriY(listMouse[i]));
                    }
                }
            }
                
            
        }
    }
    void DiChuyenCacCuc(int m, int n)
    {
        if (arrGem[m][n] == null)
        {           
            if (arrGem[m][n + 1] != null)
            {
                arrGem[m][n + 1].transform.position = new Vector3(arrGem[m][n + 1].transform.position.x, arrGem[m][n + 1].transform.position.y - (float)(0.75f), arrGem[m][n + 1].transform.position.z);
                arrGem[m][n] = arrGem[m][n + 1];
                arrGem[m][n + 1] = null;
            }
        }
    }
    void CacCucRoiXuong()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (arrGem[i][7] == null)
                {
                    InstantiateGem(i, 7, 0);
                }
                DiChuyenCacCuc(i, j);
            }
        }
        CheckListInvalid();
    }
    //kiem tra xem con duong de an khong
    List<GameObject> LoangDau(List<GameObject> list, int i, int j)
    {

        for (int b = j - 1; b <= j + 1; b++)
        {
            for (int a = i - 1; a <= i + 1; a++)
            {
                if (a >= 0 && b >= 0 && a <= 6 && b <= 7)
                {
                    if (arrGem[i][j].tag != null && arrGem[a][b] != null)
                    {
                        if (arrGem[i][j].tag == arrGem[a][b].tag && arrGem[a][b].GetComponent<Gem>().check == false)
                        {
                            //Debug.Log(arrGem[i][j]);
                            arrGem[i][j].GetComponent<Gem>().check = true;
                            arrGem[a][b].GetComponent<Gem>().check = true;

                            if (!list.Contains(arrGem[i][j]))
                            {
                                list.Add(arrGem[i][j]);
                            }
                            if (!list.Contains(arrGem[a][b]))
                            {
                                list.Add(arrGem[a][b]);
                            }
                            LoangDau(list, a, b);
                        }
                    }
                }

            }
        }
        return list;
    }
    //dua cac cac dung de an vao list
    void CheckListInvalid()
    {
        listLoangDau.Clear();
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 7; i++)
            {
                if (arrGem[i][j] != null)
                {
                    List<GameObject> list = new List<GameObject>();
                    LoangDau(list, i, j);
                    if (list.Count >= 3)
                    {
                        listLoangDau.Add(list);
                    }
                }
            }
        }
        ResetCheckGem();
    }
    void ResetCheckGem()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (arrGem[i][j] != null)
                    arrGem[i][j].GetComponent<Gem>().check = false;
            }
        }

    }
    //phong to va nho  kich thuoc cac cuc Gem
    void ScaleGem()
    {
        for (int i = 0; i < 3; i++)
        {
            if (listLoangDau[0][i] != null)
                listLoangDau[0][i].transform.localScale = new Vector3(localScale, localScale, 1);
        }
        boolScale = true;
    }
    //reset kich thuoc cuc Gem ve ban dau
    void ResetScaleGem()
    {
        if (boolScale == true)
        {
            for (int i = 0; i < 3; i++)
            {
                if (listLoangDau[0][i] != null)
                {
                    listLoangDau[0][i].transform.localScale = new Vector3(1, 1, 1);
                }
            }
            boolScale = false;
        }
    }
    //Random lai Map
    public void RandomMap()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Destroy(arrGem[i][j]);
            }
        }
        listConect.Clear();
        ListDelete.Clear();
        listLoangDau.Clear();
    }
    //no cac cuc theo chieu doc
    void NoTheoChieuNgang(int vitri)
    {
        for (int i = 0; i < 8; i++)
        {
            ListDelete.Add(arrGem[vitri][i]);
        }
    }
    //no cac cuc theo chieu ngang
    void NoTheoChieuDoc(int vitri)
    {
        for (int i = 0; i < 7; i++)
        {
            ListDelete.Add(arrGem[i][vitri]);
        }
    }
    void ResetCucDacBiet(int i, int j)
    {
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++)
            {
                if (m >= 0 && n >= 0 && m<=7 && n<=8)
                {
                    arrGem[m][n].GetComponent<Gem>().ResetSpriteDacBiet(listGem[arrGem[m][n].GetComponent<Gem>().inDex]);
                }
            }
        }
    }
    //cuc dac biet thu nhat lam cac cuc xung quanh giong nhu no    
    void CucDacBiet(int i, int j)
    {
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++)
            {
                if (m >= 0 && n >= 0 && m<=7 && n<=8 && arrGem[m][n] != arrGem[i][j])
                {
                    if (!ListDelete.Contains(arrGem[m][n]))
                        arrGem[m][n].GetComponent<Gem>().ChangSpriteDacBiet(arrGem[i][j]);
                }
            }
        }
    }
    void InstantiateItemDacBiet()
    {
    
        vitriX = Random.Range(0, 7);
        vitriY = Random.Range(0, 8);
        if (arrGem[vitriX][vitriY] != null && arrGem[vitriX][vitriY].GetComponent<Gem>().cucDacBiet != true)
        {
            //arrGem[m][n].GetComponent<Image>().color = Color.Lerp(arrGem[m][n].GetComponent<Image>().color, Color.black, 0.5f);
            int a = Random.Range(0, 3);
            if (a == 0)
            {
                arrGem[vitriX][vitriY].GetComponent<Gem>().destroyCollum = true;
                GameObject meomeo = Instantiate(cucDacBiet[0], arrGem[vitriX][vitriY].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[vitriX][vitriY].transform;
                meomeo.transform.localScale = Vector3.one;
            }
            if (a == 1)
            {
                arrGem[vitriX][vitriY].GetComponent<Gem>().destroyRow = true;
                GameObject meomeo = Instantiate(cucDacBiet[1], arrGem[vitriX][vitriY].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[vitriX][vitriY].transform;
                meomeo.transform.localScale = Vector3.one;
            }
            if (a == 2)
            {
                arrGem[vitriX][vitriY].GetComponent<Gem>().cucDacBiet = true;
                GameObject meomeo = Instantiate(cucDacBiet[2], arrGem[vitriX][vitriY].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[vitriX][vitriY].transform;
                meomeo.transform.localScale = Vector3.one;
            }
            activeInstanDacBiet1 = false;
            activeInstanDacBiet2 = false;
        }

    }
    void DestroyButtonMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (rayHit.collider == null)
            {
                return;
            }
            else
            {
                Debug.Log(System.String.Format("X = {0}", TimViTriX(rayHit.collider.gameObject)));
                Debug.Log(System.String.Format("Y = {0}", TimViTriY(rayHit.collider.gameObject)));
            }

        }
    }

}
