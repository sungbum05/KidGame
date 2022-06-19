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

        if (StartChk == true) // 게임 하는 중
        {
            
        }

        else if (StartChk == false) //게임 시작하기 전
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

        else if(StartChk == false && CurGameCount > MaxGameCount) //게임 끝남
        {
            
        }

        CurGameCount += 1;
    }

    #region 내부 랜덤 시스템
    void ShuffleObj() //shadow 랜덤 및 특정 3개 오브젝트 이미지 변경
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

    #region  마우스 상호작용 함수들
    void MouseClick() // 마우스를 누르고 있는동안 ray실행
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

    #region 오브젝트연출(리스트 등장/삭제, 그림자 등장삭제)
    IEnumerator ResultObjsProduce(GameObject obj, ShowType type) // 그림자 출연 연출(매개변수: 연출 할 오브젝트, 연출 효과), (효과: 확대, 색 알파)
    {
        yield return null;

        obj.GetComponent<SpriteRenderer>().DOFade((int)type, ShowTime * 1.5f);
    }

    IEnumerator ObjListProduce(GameObject Obj, ShowType type) // 리스트 속 장난감 출연 연출(매개변수: 연출 할 오브젝트, 연출 효과), (효과: 확대, 회전)
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
