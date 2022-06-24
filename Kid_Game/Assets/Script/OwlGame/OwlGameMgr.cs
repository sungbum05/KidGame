using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class ObjsAttribute
{
    public GameObject Obj;
    public GameObject ShadowObjs;
    public bool SuccesObj = false;
}


public class OwlGameMgr : Mgr
{
    [Header("Owl_Mgr_attribute")]
    [SerializeField]
    GameObject FindCircle = null;
    [SerializeField]
    List<ObjsAttribute> Objs = null;
    [SerializeField]
    List<Transform> ObjSpawnPos = null;

    [Header("Owl_Mgr_Mouse")]
    [Space(10)]
    [SerializeField]
    LayerMask layerMask;
    Vector2 MousePos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            MouseClick();
    }

    IEnumerator StartGame()
    {
        yield return null;

        #region ���� ��
        if (StartChk == true) // ���� �ϴ� ��
        {

        }
        #endregion

        #region ���� ���� ��
        else if (StartChk == false) //���� �����ϱ� ��
        {
            StartChk = true;

            ObjRandomSpawn();
        }
        #endregion

        #region ���� ����
        if (ClearChk == true && CurGameCount > MaxGameCount) //���� ����
        {

        }
        #endregion
    }

    #region  ���콺 ��ȣ�ۿ� �Լ���
    void MouseClick() // ���콺�� ������ �ִµ��� ray����
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (ClearChk == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(MousePos, transform.forward, 10.0f, ClearLayer);
            if (hit)
            {

            }
        }

        else
        {
            FindCircle.transform.position = MousePos;
        }
    }
    #endregion

    private void ObjRandomSpawn()
    {
        GetShuffleList<Transform>(ObjSpawnPos);

        for (int i = 0; i < Objs.Count; i++)
        {
            Objs[i].Obj.transform.localPosition = ObjSpawnPos[i].transform.localPosition;
        }
    }

    #region ������Ʈ �̵� �� ���� ȿ��
    public IEnumerator ShowObj(int ObjNum)
    {
        yield return null;

        FindCircle.gameObject.transform.DOScale(Vector2.zero, ShowTime);
        yield return new WaitForSeconds(ShowTime);

        GameObject ChildObj = Objs[ObjNum].Obj.transform.GetChild(0).gameObject;
        ChildObj.GetComponent<SpriteMask>().enabled = true;
        ChildObj.gameObject.transform.DOScale(0.5f, ShowTime);
        yield return new WaitForSeconds(ShowTime);


        StartCoroutine(MoveToObj(ObjNum));
    }

    IEnumerator MoveToObj(int ObjNum)
    {
        yield return null;
        Debug.Log(ObjNum);

        float UserTime = 1;

        while (true)
        {
            yield return null;

            UserTime += (Time.deltaTime * 2);

            Objs[ObjNum].Obj.transform.position = Vector3.Slerp(Objs[ObjNum].Obj.transform.position,
                Objs[ObjNum].ShadowObjs.transform.position, 0.8f * Time.deltaTime * UserTime);

            if (Objs[ObjNum].Obj.transform.position == Objs[ObjNum].ShadowObjs.transform.position)
                break;
        }

        Debug.Log("End");
        Objs[ObjNum].Obj.transform.GetChild(0).gameObject.GetComponent<SpriteMask>().enabled = false;
        Objs[ObjNum].SuccesObj = true;

        FindCircle.gameObject.transform.DOScale(Vector2.one, ShowTime);
        yield return new WaitForSeconds(ShowTime);

        StartCoroutine(ClearShow());
    }

    protected override IEnumerator ClearShow()
    {
        yield return null;
        bool Chk = true;

        foreach (var obj in Objs)
        {
            if (obj.SuccesObj == false)
            {
                Chk = false;
            }
        }

        if (Chk)
        {
            Debug.Log("asd");
            FindCircle.gameObject.transform.DOScale(10, ShowTime * 5);

            //yield return base.ClearShow();
        }
    }
    #endregion
}