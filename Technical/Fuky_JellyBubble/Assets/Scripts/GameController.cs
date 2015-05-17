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
    private GameObject[] listGem;//list cac gem_Prefabs
    public GameObject[] cucDacBiet;//list cac gem_Prefabs
    public GameObject conect;
    public GameObject destroyGem;
    public Sprite[] image;
    public RectTransform ouRectGem;

    [HideInInspector]
    public List<GameObject> ListDelete;//list Object de xoa

    public RectTransform canvasRectTransform;

    public int countRow;//so hang cua mang
    public int countCollumn;//so cot cua mang
    public int iTwenPos;//vi tri in ra luc dau
    private float localScale = 1;//kich thuc gem

    public int GemHeight;
    public int GemWitdh;

    [HideInInspector]
    public bool activeTimeHelp = false;

    public float timeHelp = 5;

    [HideInInspector]
    public bool activeDestroyGem = false;

    [HideInInspector]
    public int score = 0;
    private Transform tranfsIn;
    public Transform tranfsOut;
    public GameObject timeStar;
    public Transform gemContainer;
    public Transform conectContainer;


    private GameObject[][] arrGem;//list Game Object hien ra man hinh
    //private RaycastHit2D rayHit;

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

    [HideInInspector]
    public bool activeInstanDacBiet1 = false;
    private bool activeInstanDacBiet2 = false;

    [HideInInspector]
    public int indexRandom;
    
    [HideInInspector]
    public bool activeAddtime;

    private NoName noname;

    public float disX;
    public float disY;

    public int level;
    private int countWarter = 0;
    private int countSum = 0;
    private int countWorm = 0;
    private int countGround = 0;
    private int countGarbage = 0;

    public TreeController treeController;
    
	// Use this for initialization
	void Start () {
        level = 0;
        LoadLevel();
        SetTextGUI();
        SetFillAmuontGarbage();
        noname = GameObject.Find("Canvas").GetComponentInChildren<NoName>();
        if (noname == null)
        {
            Debug.Log("Khong the tim thay NoName");
        }
        ListDelete = new List<GameObject>();
        listMouse = new List<GameObject>();
        listLoangDau = new List<List<GameObject>>();
        listConect = new List<GameObject>();
        activeAddtime = false;
        activeTimeHelp = true;
        RandomGem();// random hinh anh khi moi dua vao game o vi tri ItweenPos	
        CheckListInvalid();

        _countGround = 20;
	}
	
	// Update is called once per frame
    //void Update () {
        
    //}
    void Update()
    {
        
        if (uplevel == true)
        {
            LoadLevel();
            uplevel = false;
        }
        
        if(activeDestroyGem == true)
        {

            if (activeInstanDacBiet1 == true && activeInstanDacBiet2 == true)
            {
                InstantiateItemDacBiet(); 
            }
            CacCucRoiXuong();
            if (activeAddtime == true)
            {
                //InstantiateTimeStar();
                
            }
            if (listLoangDau.Count == 0)//neu k con duong nao de an Ramdom lai map
            {
                RandomMap();
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
    public int[] maxGem;
    void InstantiateGem(int row, int collumn, int ItPos)
    {

        int index;
        do
        {
            index = Random.Range(0, 5);

        } while (totalGemColor[index] >= maxGem[index]);
        totalGem(index);
        //GameObject a = Instantiate(listGem[index], Vector3.zero, Quaternion.identity) as GameObject; //new Vector3(row * 0.75f - x, collumn * 0.75f - y + posItween, 0)
        //add vao Canvas        
        GameObject gemObj = SpawnGem(gemPrefabs, "gem");

        gemObj.GetComponent<Gem>().spriteStart = gemImageStart[index];
        gemObj.GetComponent<Gem>().spriteChange = gemImageChange[index];        

        Vector3 pos = new Vector3((row - 3.0f) * (80 + disX), (collumn - 3.5f) * (72 + disY) + ItPos, 1);
        
        gemObj.transform.SetParent(gemContainer);
        gemObj.transform.localScale = Vector3.one;
        gemObj.transform.localPosition = pos;
        arrGem[row][collumn] = gemObj;

        Gem gem = gemObj.GetComponent<Gem>();
        gem.SetProfile(collumn, row, index);

        gemObj.GetComponent<Gem>().ResetSprite();
       // gemObj.GetComponent<Gem>().ResetSpriteStart();
        gemObj.GetComponent<Gem>().ResetActive();
        

        Vector3 posIT = new Vector3((row - 3.0f) * (80 + disX), (collumn - 3.5f) * (72 + disY), 1);
        arrGem[row][collumn].GetComponent<Gem>().MovePosition(posIT, 0.5f);
         
    }

    public GameObject SpawnGem( GameObject obj, string nameSpawnPool) 
    {
        SpawnPool gemPool = PoolManager.Pools[nameSpawnPool];
        //Transform gemObj = gemPool.Spawn(listGem[index]);
        Transform gemObj = gemPool.Spawn(obj);
        return gemObj.gameObject;
    }

    public void DespawnGem(Transform gemTrans, string nameSpawnPool) 
    {
        
        SpawnPool gemPool = PoolManager.Pools[nameSpawnPool];
        gemPool.Despawn(gemTrans);
    }


    void InstantiateTimeStar()
    {
        int row = Random.Range(0, countCollumn);
        int collumn = Random.Range(0, countRow);
        do
        {
            if (arrGem[row][collumn] == null || arrGem[row][collumn].GetComponent<Gem>().timeAdd == true)
            {
                row = Random.Range(0, countCollumn);
                collumn = Random.Range(0, countRow);
            }
        } while (arrGem[row][collumn] == null || arrGem[row][collumn].GetComponent<Gem>().timeAdd == true);
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

        GameObject a = SpawnGem(conect, "conect");

        a.transform.SetParent(conectContainer);
        Vector3 pos = new Vector3(x, y, 0);
        a.transform.localScale = new Vector3(0.45f, 0.45f, 0);

        EffectController effect = a.GetComponent<EffectController>();
        
        if (gameObj1.transform.localPosition.x < gameObj2.transform.localPosition.x || gameObj1.transform.localPosition.y < gameObj2.transform.localPosition.y)
        {
            effect.SetPositionBetweenTwoGem(gameObj1.transform, gameObj2.transform);
        }

        else
        {
            effect.SetPositionBetweenTwoGem(gameObj2.transform, gameObj1.transform);
        }        
        
        listConect.Add(a);
    }
    private List<GameObject> listItween = new List<GameObject>();
    void Xoa()
    {
        //xoa cac cuc        
        if (ListDelete.Count >= 3)
        {
            
            Gem _gem = ListDelete[0].GetComponent<Gem>();
            //if (_gem == null)
            //{
            //    return;
            //}
            //if (_gem.inDex == 0)
            //{
            //    countWarter += ListDelete.Count;
            //}
            //if (_gem.inDex == 1)
            //{
            //    countSum += ListDelete.Count;
            //}
            //if (_gem.inDex == 2)
            //{
            //    countWorm += ListDelete.Count;
            //}
            //if (_gem.inDex == 3)
            //{
            //    countGround += ListDelete.Count;
            //}
            //if (_gem.inDex == 4)
            //{
            //    countGarbage += ListDelete.Count;
            //}

            SubTotalGem(ListDelete.Count, _gem.inDex);

            noname.totalDelete[_gem.inDex] += ListDelete.Count;
            noname.Test(noname.totalDelete[_gem.inDex], _gem.inDex);
           
           
            //xoa cac Gem trong listDelete
            for (int i = 0; i < ListDelete.Count; i++)
            {
                Gem _gemDel = ListDelete[i].GetComponent<Gem>();
                if (_gemDel.cucDacBiet == true)
                {
                    for (int m = 0; m < listDacBiet.Count; m++)
                    {
                        listDacBiet[m].GetComponent<Gem>().Test(ListDelete[i]);
                    }
                }
                SetSoundDelete(ListDelete[0].GetComponent<Gem>().inDex);
                Gem gem = ListDelete[i].GetComponent<Gem>();
                arrGem[gem.row][gem.collumn] = null;                
                score += 10;
                
                DespawnGem(ListDelete[i].transform, "gem");
            }
        }

        SetFillAmuontGarbage();
        UpdateLevel();
        ListDelete.Clear();
        listConect.Clear();
        listMouse.Clear();
        GameOver();
    }
    float KiemTraKhoangCach(GameObject a, GameObject b)
    {
        float khoangCach = Vector3.Distance(a.transform.position, b.transform.position);
        return khoangCach;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        listItween.Clear();
        //GetGemTouchPosition(GetPositionTouch(eventData));
        //return;
        Vector2 pos = GetPositionTouch(eventData);
        int i = GetIndexGemX(pos);
        int j = GetIndexGemY(pos);
        if (i < 0 || j < 0 || i > countCollumn - 1 || j > countRow - 1)
        {
            return;
        }
        activeTimeHelp = false;
        activeDestroyGem = false;
        listDacBiet.Clear();
        if (arrGem[i][j] == null)
        {
            return;
        }
        if (ListDelete.Count == 0 && arrGem[i][j].GetComponent<Gem>().cucDacBiet == false)
        {
            ListDelete.Add(arrGem[i][j]);
            listItween.Add(arrGem[i][j]);
            listMouse.Add(arrGem[i][j]);
            ListDelete[ListDelete.Count - 1].GetComponent<Gem>().ChangSprite();
        }
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < listConect.Count; i++)
        {
            DespawnGem(listConect[i].transform, "conect");
        }
        if (ListDelete.Count == 1)
        {
            ListDelete[0].GetComponent<Gem>().ResetSprite();
            ListDelete.Clear();
            listConect.Clear();
            listMouse.Clear();
            listLoangDau.Clear();
            listDacBiet.Clear();
            //activeDestroyGem = true;
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
            ListDelete.Clear();
            listConect.Clear();
            listMouse.Clear();
            listLoangDau.Clear();
            listDacBiet.Clear();
        }
        //xoa listConect
        

        if (ListDelete.Count >= 3)
        {
            Gem _g = ListDelete[1].GetComponent<Gem>();
            if (_g == null)
            {
                return;
            }
            if (_g.inDex == 0)
            {
                countWarter += ListDelete.Count;
            }
            if (_g.inDex == 1)
            {
                countSum += ListDelete.Count;
            }
            if (_g.inDex == 2)
            {
                countWorm += ListDelete.Count;
            }
            if (_g.inDex == 3)
            {
                countGround += ListDelete.Count;
                _countGround += ListDelete.Count;
                SetFillAmuontGarbage();
            }
            if (_g.inDex == 4)
            {
                countGarbage += ListDelete.Count;
                _countGarbage += ListDelete.Count;
                SetFillAmuontGarbage();
            }
            //kiem tra xem cac cuc dac biet co o trong listDelete khong
            for (int i = 0; i < ListDelete.Count; i++)
            {
                Gem _gemDacBiet = ListDelete[i].GetComponent<Gem>();
                if (_gemDacBiet.destroyCollum == true)
                {
                    NoTheoChieuNgang(_gemDacBiet.row);
                }
                if (_gemDacBiet.destroyRow == true)
                {
                    NoTheoChieuDoc(_gemDacBiet.collumn);
                }
            }
           
            for (int i = 0; i < ListDelete.Count; i++)
            {
                Vector3 pos = new Vector3(ListDelete[i].transform.localPosition.x, ListDelete[i].transform.localPosition.y, -9000);

                GameObject _destroy = Instantiate(destroyGem, Vector3.one, Quaternion.identity) as GameObject;
                _destroy.transform.SetParent(gemContainer);
                _destroy.transform.localScale = new Vector3(75, 75, 0);
                _destroy.transform.localPosition = pos;
            }
            
            for (int i = 0; i < ListDelete.Count; i++)
            {
                Gem _gem = ListDelete[0].GetComponent<Gem>();
                if (_gem.inDex ==0)
                {
                    MoveItween(ListDelete[i], sumTransform.localPosition, 0.4f);
                }
                if (_gem.inDex == 1)
                {
                    MoveItween(ListDelete[i],  warterTranform.localPosition , 0.4f);
                }
                if (_gem.inDex == 2)
                {
                    MoveItween(ListDelete[i], wormTransform.localPosition, 0.4f);
                }
                if (_gem.inDex == 3)
                {
                    MoveItween(ListDelete[i], groundTransform.localPosition, 0.4f);
                }
                if (_gem.inDex == 4)
                {
                    MoveItween(ListDelete[i], garbageTransform.localPosition, 0.4f);
                }                
            }
        }
        
        
        
        //Xoa();
        
        activeTimeHelp = true;
        activeInstanDacBiet2 = true;
    }
    public void MoveItween(GameObject obj, Vector3 pos, float movetime)
    {
        iTween.MoveTo(obj, iTween.Hash(
            iT.MoveTo.position, pos,//toi vi tri cuoi
            iT.MoveTo.islocal, true,
            iT.MoveTo.time, movetime,//thoi gian
            iT.MoveTo.oncomplete, "Xoa",
            iT.MoveTo.oncompletetarget, gameObject
            ));
    }
    public void OnDrag(PointerEventData eventData)
    {
        //GetGemTouchPosition(GetPositionTouch(eventData));
        //return;
        Vector2 pos = GetPositionTouch(eventData);
        int x = GetIndexGemX(pos);
        int y = GetIndexGemY(pos);
        if (x < 0 || y < 0 || x > countCollumn - 1 || y > countRow - 1)
        {
            return;
        }
        if (ListDelete == null )
        {
            return;
        }       

        if (ListDelete.Count <= 0 || ListDelete[0] == null)
        {
            return;
        }

        if (arrGem[x][y].GetComponent<Gem>().inDex == ListDelete[0].GetComponent<Gem>().inDex && KiemTraKhoangCach(arrGem[x][y], ListDelete[ListDelete.Count - 1]) <= 1.2f)//kiem tra de dua vao listDelete
        {
            if (!listMouse.Contains(arrGem[x][y]))
            {
                listMouse.Add(arrGem[x][y]);
            }
            if (!ListDelete.Contains(arrGem[x][y]) && ListDelete.Count >= 1)//kiem tra xem doi tuong chon da co trong ListDelete chua
            {

                InstantiateConect (ListDelete[ListDelete.Count - 1], arrGem[x][y]);//xuat ket noi ra man hinh

                ListDelete.Add(arrGem[x][y]);
                listItween.Add(arrGem[x][y]);
                SetSoundDrag(ListDelete);
                if (ListDelete[ListDelete.Count - 1] != null)
                {
                    ListDelete[ListDelete.Count - 1].GetComponent<Gem>().ChangSprite();
                }

                if (ListDelete.Contains(arrGem[x][y]) && arrGem[x][y].GetComponent<Gem>().cucDacBiet == true)
                {
                    int a = arrGem[x][y].GetComponent<Gem>().PosX();
                    int b = arrGem[x][y].GetComponent<Gem>().PosY();
                    CucDacBiet(a, b);
                }

            }
            if (ListDelete.Count >= 2)
            {
                if (arrGem[x][y] == ListDelete[ListDelete.Count - 2] && listConect.Count >= 1)//neu nguoi choi quay lai cuc phia trc co
                {
                    //listItween.Add(ListDelete[ListDelete.Count - 1]);
                    ListDelete[ListDelete.Count - 1].GetComponent<Gem>().ResetSprite();
                    ListDelete.RemoveAt(ListDelete.Count - 1);
                    
                    Destroy(listConect[listConect.Count - 1]);
                    listConect.RemoveAt(listConect.Count - 1);
                    SetSoundDrag(ListDelete);
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
        
        //Move(ListDelete, arrGem[x][y]);
    }
   
    void SetSoundDelete(int index)
    {
        if (index == 0)
        {
            AudioController.Instance.PlaySound(AudioType.REMOVE_LIGHT);
        }
        if (index == 1)
        {
            AudioController.Instance.PlaySound(AudioType.REMOVE_SOIL);
        }
        if (index == 2)
        {
            AudioController.Instance.PlaySound(AudioType.REMOVE_TRASH);
        }
        if (index == 3)
        {
            AudioController.Instance.PlaySound(AudioType.REMOVE_WATER);
        }
        if (index == 4)
        {
            AudioController.Instance.PlaySound(AudioType.REMOVE_WORM);
        }
    }
    void SetSoundDrag(List<GameObject> listObj)
    {
        if (listObj.Count == 2)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER1);
        }
        if (listObj.Count == 3)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER2);
        }
        if (listObj.Count == 4)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER3);
        }
        if (listObj.Count == 5)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER4);
        }
        if (listObj.Count == 6)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER5);
        }
        if (listObj.Count == 7)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER6);
        }
        if (listObj.Count == 8)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER7);
        }
        if (listObj.Count == 9)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER8);
        }
        if (listObj.Count >= 10)
        {
            AudioController.Instance.PlaySound(AudioType.SG_MERGER9);
        }
    }
    //public void OnDrag(PointerEventData eventData)
    //{      

    //    //Debug.Log(System.String.Format("On Drag in = {0}", eventData.position));
    //    //GetPositionTouch(eventData);
    //    rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    //    if (ListDelete == null)
    //    {
    //        return;
    //    }

    //    if (rayHit.collider == null)
    //    {
    //        return;
    //    }

    //    if (ListDelete.Count <= 0 || ListDelete[0] == null)
    //    {
    //        return;
    //    }

    //    if (rayHit.collider.gameObject.GetComponent<Gem>().inDex == ListDelete[0].GetComponent<Gem>().inDex && KiemTraKhoangCach(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]) <= 1.2f)//kiem tra de dua vao listDelete
    //    {
    //        if (!listMouse.Contains(rayHit.collider.gameObject))
    //        {
    //            listMouse.Add(rayHit.collider.gameObject);
    //        }
    //        if (!ListDelete.Contains(rayHit.collider.gameObject) && ListDelete.Count >= 1)//kiem tra xem doi tuong chon da co trong ListDelete chua
    //        {

    //            InstantiateConect(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]);//xuat ket noi ra man hinh
    //            ListDelete.Add(rayHit.collider.gameObject);
    //            if (ListDelete[ListDelete.Count - 1] != null)
    //            {
    //                ListDelete[ListDelete.Count - 1].GetComponent<Gem>().ChangSprite();
    //            }

    //            if (ListDelete.Contains(rayHit.collider.gameObject) && rayHit.collider.gameObject.GetComponent<Gem>().cucDacBiet == true)
    //            {
    //                int a = rayHit.collider.gameObject.GetComponent<Gem>().PosX();
    //                int b = rayHit.collider.gameObject.GetComponent<Gem>().PosY();
    //                CucDacBiet(a, b);
    //            }                
    //        }
    //        if (ListDelete.Count >= 2)
    //        {
    //            if (rayHit.collider.gameObject == ListDelete[ListDelete.Count - 2] && listConect.Count >= 1)//neu nguoi choi quay lai cuc phia trc co
    //            {
    //                ListDelete[ListDelete.Count - 1].GetComponent<Gem>().ResetSprite();
    //                ListDelete.RemoveAt(ListDelete.Count - 1);                    
    //                Destroy(listConect[listConect.Count - 1]);
    //                listConect.RemoveAt(listConect.Count - 1);
    //            }
    //        }
    //        for (int i = 0; i < listMouse.Count; i++)
    //        {
    //            if (listMouse[i].GetComponent<Gem>().cucDacBiet == true)
    //            {
    //                GameObject m = listMouse[i];
    //                if (!ListDelete.Contains(listMouse[i]))
    //                {
    //                    int a = m.gameObject.GetComponent<Gem>().PosX();
    //                    int b = m.gameObject.GetComponent<Gem>().PosY();
    //                    ResetCucDacBiet(a, b);
    //                }
    //            }
    //        }
    //    }
    //}

    void GetGemTouchPosition(Vector2 pos)
    {

        int i, j;
        i = (int)((pos.x + 40)/ 80 + 3.0f);
        j = (int)((pos.y + 36) / 72 + 3.5f);
        Debug.Log(System.String.Format("X = {0}, Y = {1}", i, j));
    }
    int GetIndexGemX(Vector2 pos)
    {
       
        int  indexX = (int)((pos.x + 40) / 80 + 3.0f);
        return indexX;
    }
    int GetIndexGemY(Vector2 pos)
    {
        int indexY = (int)((pos.y + 36) / 72 + 3.5f);

        return indexY;
    }
    Vector2 GetPositionTouch(PointerEventData eventData)
    {
        Vector2 pointerPostion = eventData.position;//ClampToWindow(eventData);
        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, pointerPostion, eventData.pressEventCamera, out localPointerPosition);
        return localPointerPosition;
    }
    void DiChuyenCacCuc(int m, int n)
    {
        if (arrGem[m][n] == null)
        {
            if (arrGem[m][n + 1] != null)
            {
                Vector3 pos = new Vector3((m - 3.0f) * (80 + disX), (n  - 3.5f) * (72 + disY), 0);
                Gem _gem = arrGem[m][n + 1].GetComponent<Gem>();
                _gem.GetComponent<Gem>().collumn -= 1;
                _gem.MovePosition(pos, 0.5f);
                
                arrGem[m][n] = arrGem[m][n + 1];
                arrGem[m][n + 1] = null;
            }
        }
    }
    void CacCucRoiXuong()
    {        
        for (int i = 0; i < countCollumn; i++)
        {
            if (arrGem[i][countRow - 1] == null)
            {
                InstantiateGem(i, countRow - 1, iTwenPos);
            }
            for (int j = 0; j < countRow - 1 ; j++)
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
                if (a >= 0 && b >= 0 && a <= countCollumn - 1 && b <= countRow - 1)
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
        for (int j = 0; j < countRow; j++)
        {
            for (int i = 0; i < countCollumn; i++)
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
        for (int i = 0; i < countCollumn; i++)
        {
            for (int j = 0; j < countRow; j++)
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

        for (int i = 0; i < 5; i++)
        {
            totalGemColor[i] = 0;
        }
        for (int i = 0; i < countCollumn; i++)
        {
            for (int j = 0; j < countRow; j++)
            {
                DespawnGem(arrGem[i][j].gameObject.transform,"gem");
                InstantiateGem(i, j, 200);//in ra cac Object o vi tri PosIT        
            }
        }
        ReStart();
        CheckListInvalid();
    }
    public void ReStart()
    {
        activeAddtime = false;
        activeTimeHelp = true;
        activeHelp = false;
        activeDestroyGem = false;
        boolScale = false;
        score = 0;
        move = 31;
        SetTextGUI();

        _countGarbage = 20;
        _countSum = 0;
        _countGround = 0;
        _countWorm = 0;
        _countWarter = 0;

        level = 0;
        UpdateLevel();

        help = 0;
        
        

        activeInstanDacBiet2 = false;
        activeInstanDacBiet1 = false;
        activeAddtime = false;
        for (int i = 0; i < listConect.Count; i++)
        {
            Destroy(listConect[i]);
        }
        listConect.Clear();
        ListDelete.Clear();
        listDacBiet.Clear();
        listLoangDau.Clear();
        listMouse.Clear();
        noname.ResetGame();
    }
    //no cac cuc theo chieu doc
    void NoTheoChieuNgang(int vitri)
    {
        for (int i = 0; i < countRow; i++)
        {
            if (!ListDelete.Contains(arrGem[vitri][i]))
            {
                if (arrGem[vitri][i] != null)
                    ListDelete.Add(arrGem[vitri][i]);
            }
                
        }
    }
    //no cac cuc theo chieu ngang
    void NoTheoChieuDoc(int vitri)
    {
        for (int i = 0; i < countCollumn; i++)
        {
            if (!ListDelete.Contains(arrGem[i][vitri]))
            {
                if (arrGem[i][vitri] != null)
                    ListDelete.Add(arrGem[i][vitri]);
            }
        }
    }
    void ResetCucDacBiet(int i, int j)
    {
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++)
            {
                if (m >= 0 && n >= 0 && m<countCollumn && n<countRow)
                {
                    //arrGem[m][n].GetComponent<Gem>().ResetSpriteDacBiet(arrGem[m][n]);
                    if (!ListDelete.Contains(arrGem[m][n]))
                    {
                        SubTotalGem(1, arrGem[m][n].GetComponent<Gem>().inDex);
                        arrGem[m][n].GetComponent<Gem>().ResetSpriteDacBiet(arrGem[m][n]);
                        totalGem(arrGem[m][n].GetComponent<Gem>().inDex);
                    }
                    
                }
            }
        }
    }
    //cuc dac biet thu nhat lam cac cuc xung quanh giong nhu no  
    List<GameObject> listDacBiet = new List<GameObject>();
    void CucDacBiet(int i, int j)
    {
        listDacBiet.Clear();
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++ )
            {
                if (m >= 0 && n >= 0 && m < countCollumn && n < countRow && arrGem[m][n] != arrGem[i][j])
                {
                    //arrGem[m][n].GetComponent<Gem>().ChangSpriteDacBiet(arrGem[i][j]);  
                    if (!ListDelete.Contains(arrGem[m][n]))
                    {
                        SubTotalGem(1, arrGem[m][n].GetComponent<Gem>().inDex);

                        arrGem[m][n].GetComponent<Gem>().ChangSpriteDacBiet(arrGem[i][j]);

                        totalGem(arrGem[m][n].GetComponent<Gem>().inDex);
                        if (arrGem[m][n].GetComponent<Gem>().cucDacBiet == false)
                            listDacBiet.Add(arrGem[m][n]);
                    }
                    
                }
            }
        }
    }
    
    void InstantiateItemDacBiet()
    {
        //int indexRandom;
        int vitriX = Random.Range(0, countCollumn - 1);
        int vitriY = Random.Range(0, countRow - 1);
        if(arrGem[vitriX][vitriY] == null)
        {
            return;
        }
        if (arrGem[vitriX][vitriY] != null && arrGem[vitriX][vitriY].GetComponent<Gem>().cucDacBiet != true)
        {
            if (indexRandom == 0)
            {
                arrGem[vitriX][vitriY].GetComponent<Gem>().destroyCollum = true;
                GameObject dacbiet = Instantiate(cucDacBiet[0], arrGem[vitriX][vitriY].transform.position, Quaternion.identity) as GameObject;
                dacbiet.transform.parent = arrGem[vitriX][vitriY].transform;
                dacbiet.transform.localScale = Vector3.one;
            }
            if (indexRandom == 1 )
            {
                arrGem[vitriX][vitriY].GetComponent<Gem>().destroyRow = true;
                GameObject dacbiet = Instantiate(cucDacBiet[1], arrGem[vitriX][vitriY].transform.position, Quaternion.identity) as GameObject;
                dacbiet.transform.parent = arrGem[vitriX][vitriY].transform;
                dacbiet.transform.localScale = Vector3.one;
            }
            if (indexRandom == 2 )
            {
                arrGem[vitriX][vitriY].GetComponent<Gem>().cucDacBiet = true;
                GameObject dacbiet = Instantiate(cucDacBiet[2], arrGem[vitriX][vitriY].transform.position, Quaternion.identity) as GameObject;
                dacbiet.transform.parent = arrGem[vitriX][vitriY].transform;
                dacbiet.transform.localScale = Vector3.one;
            }
            activeInstanDacBiet1 = false;
            activeInstanDacBiet2 = false;
        }

    }
    void DestroyButtonMouse()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //    if (rayHit.collider == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        ListDelete.Add(rayHit.collider.gameObject);
        //        Destroy(ListDelete[0]);
        //        Debug.Log(arrGem[0][0]);
        //        ListDelete.Clear();
        //    }

        //}
    }

    int[] totalGemColor = new int[5];
    
    void  totalGem(int index)
    {
        totalGemColor[index] += 1;
    }
    [ContextMenu("Test")]
    void Test()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(totalGemColor[i]);
        }
    }

    void SubTotalGem(int totalGemDelete, int indexTotalGemDelete)
    {
        totalGemColor[indexTotalGemDelete] -= totalGemDelete;
        totalGemDelete = 0;
    }
    void Move(List<GameObject> lDeleTe, GameObject obj)
    {

        for (int i = 0; i < lDeleTe.Count; i++)
        {
            if (lDeleTe[i].GetComponent<Gem>().destroyRow == true )
            {                
                GameObject dacbiet;
                dacbiet = lDeleTe[i].transform.GetChild(0).gameObject;

                dacbiet.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);

                dacbiet.transform.SetParent(obj.transform);

                dacbiet.transform.localScale = Vector3.one;
                lDeleTe[i].GetComponent<Gem>().destroyRow = false;
                obj.GetComponent<Gem>().destroyRow = true;
            }
        }
    }

    public RectTransform sumTransform;
    public RectTransform warterTranform;
    public RectTransform wormTransform;
    public RectTransform groundTransform;
    public RectTransform garbageTransform;

    private float _countWarter;
    private float _countSum;
    private float _countWorm;
    private float _countGround;
    private float _countGarbage;
    private bool uplevel = false;
    void LoadLevel()
    {
        //Dictionary<GemType, int> levelConfig = LevelConfig.Instance.GetLevelConfigByLevel(level);

        DataLevel dataLevel = LevelConfig.Instance.GetDataLevelByLevel(level);

        _countWarter = dataLevel.water;//levelConfig[GemType.WATER];
        _countSum = dataLevel.sun;//levelConfig[GemType.SUN];
        _countWorm = dataLevel.worm;//levelConfig[GemType.WORM];
        Debug.Log("Level = "+ level);
        Debug.Log(System.String.Format("Warter = {0}, Sum = {1}, Worm = {2}, Ground = {3}, Garbage = {4}", _countWarter, _countSum, _countWorm, _countGround, _countGarbage));
    }
    void UpdateLevel()
    {
        _countWarter -= countWarter;
        _countSum -= countSum;
        _countWorm -= countWorm;

        countWarter = 0;
        countSum = 0;
        countWorm = 0;
        CheckMoveTouch();
        
        if (_countWarter <= 0 && _countSum <= 0 && _countWorm<= 0 )
        {
            level++;
            uplevel = true;            
            Debug.Log("Up Level");
            treeController.SetLevel(level);
            ResetCount();
        }
        
        SetTextGUI();
    }
    void ResetCount()
    {
        countWarter = 0;
        countSum = 0;
        countWorm = 0;
    }
    public int GetCountGround()
    {
        return countGround;
    }
    public int GetGarbage()
    {
        return countGarbage;
    }
    public GameObject garbage;
    public Text textWarter;
    public Text textSum;
    public Text textWorm;
    public Text textMove;
    public Text textScore;
    public int move;
    void SetFillAmuontGarbage()
    {
        Image _imageGarbage = garbage.GetComponent<Image>();
        if (_countGarbage == 0)
        {
            _imageGarbage.fillAmount = 0;
        }
        else
        {
            if (_countGround == 0 && _countGarbage > 0)
            {
                GameOver();
            }
            else 
            {
                Debug.Log("Ground = " + _countGround + " - counr garbage = " + _countGarbage);
                float fillAmount = (float)(_countGarbage / _countGround);
                _imageGarbage.fillAmount = fillAmount;
                
                if (fillAmount > 1.0)
                {
                    GameOver();
                }
            }
            
        }

        //_imageGarbage.fillAmount = 0.5f;
    }
   void CheckMoveTouch()
   {
       move -= 1;
   }
    void SetTextGUI()
   {
       textScore.text = score.ToString();
       
       textMove.text = move.ToString();
       textWarter.text = _countWarter.ToString();
       textSum.text = _countSum.ToString();       
       textWorm.text = _countWorm.ToString();
   }
    void GameOver()
    {
        if (move <= 0 || garbage.GetComponent<Image>().fillAmount == 1)
        {
            GameObject.Find("Canvas").GetComponent<ButtonController>().CheckGameOver();
        }
    }
}
