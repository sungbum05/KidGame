using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

[System.Serializable]
public class ResultObjClass
{
    public Sprite ResultImg;
    public Sprite ShadowImg;
}

[System.Serializable]
public class ShowObj
{
    public GameObject Obj;
    public Sprite ObjImg;
    public Vector3 OriginalPos;
}

public enum ShowType
{
    Hide = 0,
    Spawn = 1,
}

public class SquirrelGameMgr : Mgr
{
    [Header("SquirrelScene_Mgr_attribute")]
    [SerializeField]
    GameObject Result = null;
    [SerializeField]
    GameObject Shadow = null;
    [SerializeField]
    List<ResultObjClass> ResultObjImgs = null;
    [SerializeField]
    List<ShowObj> Objs = null;

    [Header("SquirrelScene_Mgr_Mouse")]
    [Space(10)]
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    BoxCollider2D MouseUpZone;
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
        {
            MouseClick();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
    }

    IEnumerator StartGame()
    {
        yield return null;

        if (StartChk == true) // ���� �ϴ� ��
        {
            
        }

        else if (StartChk == false) //���� �����ϱ� ��
        {
            StartChk = true;
            GetShuffleList<ResultObjClass>(ResultObjImgs);

            ShuffleObj();

            StartCoroutine(ResultObjsProduce(Shadow, ShowType.Spawn));

            for (int i = 0; i < Objs.Count; i++)
            {
                StartCoroutine(ObjListProduce(Objs[i].Obj, ShowType.Spawn));
                yield return new WaitForSeconds(ShowTime/2);
            }
        }

        else if(StartChk == false && CurGameCount > MaxGameCount) //���� ����
        {
            
        }

        CurGameCount += 1;
    }

    #region ���� ���� �ý���
    void ShuffleObj() //shadow ���� �� Ư�� 3�� ������Ʈ �̹��� ����
    {
        Debug.Log(Objs.Count);
        Shadow.GetComponent<SpriteRenderer>().sprite = ResultObjImgs[Random.Range(0, Objs.Count)].ShadowImg;

        for (int i = 0; i < Objs.Count; i++)
        {
            Objs[i].ObjImg = ResultObjImgs[i].ResultImg;
            Objs[i].Obj.GetComponent<SpriteRenderer>().sprite = Objs[i].ObjImg;
        }
    }
    #endregion

    #region  ���콺 ��ȣ�ۿ� �Լ���
    void MouseClick() // ���콺�� ������ �ִµ��� ray����
    {
        if (ClearChk == true)
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(MousePos, transform.forward, 10.0f, ClearLayer);
            if (hit)
            {
                Instantiate(balloonburst, new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + hit.collider.gameObject.GetComponent<BoxCollider2D>().offset.y), Quaternion.identity);
                Destroy(hit.collider.gameObject);
            }
        }

        else
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(MousePos, transform.forward, 10.0f, layerMask);
            if (hit)
            {
                string[] SpiltName = hit.collider.name.Split('_');
                hit.collider.gameObject.transform.position = MousePos;
            }
        }
    }

    void MouseUp()
    {
        if ((Mathf.Abs(MousePos.x) < Mathf.Abs(MouseUpZone.bounds.extents.x) && Mathf.Abs(MousePos.y) < Mathf.Abs(MouseUpZone.bounds.extents.y)))
        {
            Debug.Log("Yes");
        }

        else
        {

        }
    }
    #endregion

    #region ������Ʈ����(����Ʈ ����/����, �׸��� �������)
    IEnumerator ResultObjsProduce(GameObject obj, ShowType type) // �׸��� �⿬ ����(�Ű�����: ���� �� ������Ʈ, ���� ȿ��), (ȿ��: Ȯ��, �� ����)
    {
        yield return null;

        obj.GetComponent<SpriteRenderer>().DOFade((int)type, ShowTime * 1.5f);
    }

    IEnumerator ObjListProduce(GameObject Obj, ShowType type) // ����Ʈ �� �峭�� �⿬ ����(�Ű�����: ���� �� ������Ʈ, ���� ȿ��), (ȿ��: Ȯ��, ȸ��)
    {
        yield return null;

        Obj.transform.DOScale((int)type, ShowTime * 1f);
        while (true)
        {
            yield return null;
            Obj.transform.Rotate(0, 0, (360 * (type == ShowType.Spawn ? -1 : 1)) * Time.deltaTime);

            if (Obj.transform.localScale.x == (int)type)
            {
                break;
            }
        }

        Obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        StopCoroutine(ObjListProduce(Obj, type));
    }
    #endregion
}
