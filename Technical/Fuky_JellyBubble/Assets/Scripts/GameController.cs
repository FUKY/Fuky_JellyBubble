using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PathologicalGames;

public class GameController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Sprite[] gemImageStart;
    public Sprite[] gemImageChange;
    public GameObject gemPrefabs;
    public GameObject[] listGem;//list cac gem_Prefabs
    public GameObject[] cucDacBiet;//list cac gem_Prefabs
    public GameObject conect;
    public GameObject destroyGem;
    public Sprite[] image;
    public List<GameObject> ListDelete;//list Object de xoa

    public int countRow;//so hang cua mang
    public int countCollumn;//so cot cua mang
    public int iTwenPos;//vi tri in ra luc dau
    public float localScale = 1;//kich thuc gem
    public bool activeTimeHelp = false;
    public float timeHelp = 5;
    public bool activeDestroyGem = false;
    public int score = 0;
    public Transform tranfsIn;
    public Transform tranfsOut;
    public GameObject timeStar;
    public Transform gemContainer;


    private GameObject[][] arrGem;//list Game Object hien ra man hinh
    private RaycastHit2D rayHit;

    private List<GameObject> listConect;//tao lien ket cho cac cuc(sau nay thanh thanh Animation)
    private List<List<GameObject>> listLoangDau;//kiem tra con duong nao de an khong
    private List<GameObject> listMouse;
    //private int index;//so thu tu cac Prefabs   
    private float scale = 0.01f;
    private bool boolScale = false;
    private bool activeHelp;
    private float help;
    //private Gem gem;
    //private int vitriX, vitriY;
    public bool activeInstanDacBiet1 = false;
    public bool activeInstanDacBiet2 = false;
    public int indexRandom;

    public bool activeAddtime;

    private NoName noname;
	// Use this for initialization
	void Start () {
        noname = GameObject.Find("Canvas").GetComponentInChildren<NoName>();
        ListDelete = new List<GameObject>();
        listMouse = new List<GameObject>();
        listLoangDau = new List<List<GameObject>>();
        listConect = new List<GameObject>();
        activeAddtime = false;
        activeTimeHelp = true;
        RandomGem();// random hinh anh khi moi dua vao game o vi tri ItweenPos	
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
            if (activeAddtime == true)
            {
                InstantiateTimeStar();
                
            }
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

    void RandomGem()
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
                InstantiateGem(i, j,0 );//in ra cac Object o vi tri PosIT
            }
        }
    }

    void InstantiateGem(int row, int collumn, int ItPos)
    {
        int index = Random.Range(0, 5);
        
        //GameObject a = Instantiate(listGem[index], Vector3.zero, Quaternion.identity) as GameObject; //new Vector3(row * 0.75f - x, collumn * 0.75f - y + posItween, 0)
        //add vao Canvas
        GameObject gemObj = SpawnGem();

        gemObj.GetComponent<Gem>().spriteStart = gemImageStart[index];
        gemObj.GetComponent<Gem>().spriteChange = gemImageChange[index];

        

        Vector3 pos = new Vector3((row - 3.0f) * 80, (collumn - 3.5f) * 72 + ItPos, 1);
        gemObj.transform.SetParent(gemContainer);
        gemObj.transform.localScale = Vector3.one;
        gemObj.transform.localPosition = pos;
        arrGem[row][collumn] = gemObj;

        Gem gem = gemObj.GetComponent<Gem>();
        gem.SetProfile(collumn, row, index);

        gemObj.GetComponent<Gem>().ResetSprite();
        gemObj.GetComponent<Gem>().ResetSpriteStart();
        gemObj.GetComponent<Gem>().ResetActive();
        Vector3 posIT = new Vector3((row - 3.0f) * 80, (collumn - 3.5f)* 72, 1);

        arrGem[row][collumn].GetComponent<Gem>().MovePosition(posIT, 0.5f);
        arrGem[row][collumn].GetComponent<Gem>().timeAdd = false;
 
    }

    public GameObject SpawnGem() 
    {
        SpawnPool gemPool = PoolManager.Pools["gem1"];
        //Transform gemObj = gemPool.Spawn(listGem[index]);
        Transform gemObj = gemPool.Spawn(gemPrefabs);
        return gemObj.gameObject;
    }

    public void DespawnGem(Transform gemTrans) 
    {
        SpawnPool gemPool = PoolManager.Pools["gem1"];
        gemPool.Despawn(gemTrans);

    }


    void InstantiateTimeStar()
    {
        int row = Random.Range(0, 7);
        int collumn = Random.Range(0, 8);
        do
        {
            if (arrGem[row][collumn] == null)
            {
                row = Random.Range(0, 7);
                collumn = Random.Range(0, 8);
            }
        }while(arrGem[row][collumn] == null);
        GameObject a = Instantiate(timeStar, tranfsIn.position, Quaternion.identity) as GameObject;
        a.GetComponent<Gem>().MovePositionStar(arrGem[row][collumn].transform.position, 1.0f);
        a.transform.parent = arrGem[row][collumn].transform;
        //a.transform.parent = gemContainer;
        a.transform.localScale = Vector3.one;
        arrGem[row][collumn].GetComponent<Gem>().timeAdd = true;
        activeAddtime = false;
        
        
        
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
            for (int i = 0; i < 5; i++)
            {
                if (ListDelete[0].GetComponent<Gem>().inDex == i)
                {
                    noname.totalDelete[i] += ListDelete.Count;

                    if (ListDelete.Count >= 5 && ListDelete.Count < 10)
                    {
                        float total = noname.totalDelete[i] * 1.3f;
                        noname.totalDelete[i] = (int)total;
                    }
                    if (ListDelete.Count >= 10)
                    {
                        float total = noname.totalDelete[i] * 1.5f;
                        noname.totalDelete[i] = (int)total;
                    }

                    noname.Test(noname.totalDelete[i], i);
                }
            }
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
                if (ListDelete[i].GetComponent<Gem>().timeAdd == true && ListDelete[i] != null)
                {
                    if (ListDelete[i].transform.childCount == 1)
                        ListDelete[i].GetComponentInChildren<TimeController>().UpdateParent();
                    
                }
                Vector3 pos = new Vector3(ListDelete[i].transform.localPosition.x, ListDelete[i].transform.localPosition.y, -9000);
                GameObject a = Instantiate(destroyGem, Vector3.one, Quaternion.identity) as GameObject;
                a.transform.SetParent(gemContainer);
                a.transform.localScale = new Vector3(75, 75, 0);
                a.transform.localPosition = pos;

                DespawnGem(ListDelete[i].transform);

                Gem gem = ListDelete[i].GetComponent<Gem>();
                arrGem[gem.row][gem.collumn] = null;                
                score += 10;
            }
        }
        if (ListDelete.Count < 3 && ListDelete.Count >= 0)
        {
            for(int i = 0; i < ListDelete.Count; i++)
            {
                if (ListDelete[i].GetComponent<Gem>().cucDacBiet == true)
                {
                    ResetCucDacBiet(TimViTriX(ListDelete[i]), TimViTriY(ListDelete[i]));
                }
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
        posX = (int)(vitrClick.transform.position.x);

        return posX;
    }
    int TimViTriY(GameObject vitrClick)
    {
        int posY;
        posY = (int)(vitrClick.transform.position.y);
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
            
            if (ListDelete.Count == 0 && rayHit.collider.gameObject.GetComponent<Gem>().cucDacBiet == false)
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
            ListDelete[0].GetComponent<Gem>().ResetSprite();
        }
        if (ListDelete.Count == 2)
        {
            if (ListDelete[1].GetComponent<Gem>().cucDacBiet == true)
            {
                int a = ListDelete[1].gameObject.GetComponent<Gem>().PosX();
                int b = ListDelete[1].gameObject.GetComponent<Gem>().PosY();
                ResetCucDacBiet(a, b);
                
            }
            ListDelete[0].GetComponent<Gem>().ResetSprite();
            ListDelete[1].GetComponent<Gem>().ResetSprite();
        }
        
        Xoa();
        
        activeTimeHelp = true;
        activeInstanDacBiet2 = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log(System.String.Format("On Drag in = {0}", eventData.position));
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

        if (rayHit.collider.gameObject.GetComponent<Gem>().inDex == ListDelete[0].GetComponent<Gem>().inDex && KiemTraKhoangCach(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]) <= 1.2f)//kiem tra de dua vao listDelete
        {
            if (!listMouse.Contains(rayHit.collider.gameObject))
            {
                listMouse.Add(rayHit.collider.gameObject);
            }
            if (!ListDelete.Contains(rayHit.collider.gameObject) && ListDelete.Count >= 1)//kiem tra xem doi tuong chon da co trong ListDelete chua
            {

                InstantiateConect(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]);//xuat ket noi ra man hinh
                ListDelete.Add(rayHit.collider.gameObject);
                if (ListDelete[ListDelete.Count - 1] != null)
                {
                    ListDelete[ListDelete.Count - 1].GetComponent<Gem>().ChangSprite();
                }

                if (ListDelete.Contains(rayHit.collider.gameObject) && rayHit.collider.gameObject.GetComponent<Gem>().cucDacBiet == true)
                {
                    int a = rayHit.collider.gameObject.GetComponent<Gem>().PosX();
                    int b = rayHit.collider.gameObject.GetComponent<Gem>().PosY();
                    CucDacBiet(a, b);
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
                    GameObject m = listMouse[i];
                    if (!ListDelete.Contains(listMouse[i]))
                    {
                        int a = m.gameObject.GetComponent<Gem>().PosX();
                        int b = m.gameObject.GetComponent<Gem>().PosY();
                        ResetCucDacBiet(a, b);
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
                Vector3 pos = new Vector3((m - 3.0f) * 80, (n  - 3.5f) * 72, 0);
                arrGem[m][n + 1].GetComponent<Gem>().MovePosition(pos, 0.5f);
                arrGem[m][n + 1].GetComponent<Gem>().collumn -= 1 ;
                arrGem[m][n] = arrGem[m][n + 1];
                arrGem[m][n + 1] = null;
            }
        }
    }
    void CacCucRoiXuong()
    {
        for (int i = 0; i < 7; i++)
        {
            if (arrGem[i][7] == null)
            {
                InstantiateGem(i, 7, iTwenPos);
            }
            for (int j = 0; j < 7; j++)
            {                
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
                    if (arrGem[i][j].GetComponent<Gem>().inDex != null && arrGem[a][b] != null)
                    {
                        if (arrGem[i][j].GetComponent<Gem>().inDex == arrGem[a][b].GetComponent<Gem>().inDex && arrGem[a][b].GetComponent<Gem>().check == false)
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
                {
                    arrGem[i][j].GetComponent<Gem>().check = false;

                }
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
            if (!ListDelete.Contains(arrGem[vitri][i]))
                ListDelete.Add(arrGem[vitri][i]);
        }
    }
    //no cac cuc theo chieu ngang
    void NoTheoChieuDoc(int vitri)
    {
        for (int i = 0; i < 7; i++)
        {
            if (!ListDelete.Contains(arrGem[i][vitri]))
                ListDelete.Add(arrGem[i][vitri]);
        }
    }
    void ResetCucDacBiet(int i, int j)
    {
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++)
            {
                if (m >= 0 && n >= 0 && m<7 && n<8)
                {
                    arrGem[m][n].GetComponent<Gem>().ResetSpriteDacBiet(arrGem[m][n]);
                    //if (!ListDelete.Contains(arrGem[m][n]))
                    //{
                    //    arrGem[m][n].GetComponent<Gem>().ResetSpriteDacBiet(arrGem[m][n]);
                    //}
                    
                }
            }
        }
    }
    //cuc dac biet thu nhat lam cac cuc xung quanh giong nhu no    
    void CucDacBiet(int i, int j)
    {
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++ )
            {
                if (m >= 0 && n >= 0 && m<7 && n<8 && arrGem[m][n] != arrGem[i][j])
                {
                    if (!ListDelete.Contains(arrGem[m][n]))
                        arrGem[m][n].GetComponent<Gem>().ChangSpriteDacBiet(arrGem[i][j]);
                }
            }
        }
    }
    
    public void InstantiateItemDacBiet()
    {
    
        int vitriX = Random.Range(0, 7);
        int vitriY = Random.Range(0, 8);
        if (arrGem[vitriX][vitriY] != null && arrGem[vitriX][vitriY].GetComponent<Gem>().cucDacBiet != true)
        {
            if (indexRandom == 0)
            {
                arrGem[vitriX][vitriY].GetComponent<Gem>().destroyCollum = true;
                GameObject meomeo = Instantiate(cucDacBiet[0], arrGem[vitriX][vitriY].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[vitriX][vitriY].transform;
                meomeo.transform.localScale = Vector3.one;
            }
            if (indexRandom == 1)
            {
                arrGem[vitriX][vitriY].GetComponent<Gem>().destroyRow = true;
                GameObject meomeo = Instantiate(cucDacBiet[1], arrGem[vitriX][vitriY].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[vitriX][vitriY].transform;
                meomeo.transform.localScale = Vector3.one;
            }
            if (indexRandom == 2)
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
        if (Input.GetMouseButtonDown(1))
        {
            rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (rayHit.collider == null)
            {
                return;
            }
            else
            {
                ListDelete.Add(rayHit.collider.gameObject);
                Destroy(ListDelete[0]);
                Debug.Log(arrGem[0][0]);
                ListDelete.Clear();
            }

        }
    }
   
}
