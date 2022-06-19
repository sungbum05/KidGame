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
    public int PickNum;
}

[System.Serializable]
public class ShowObj
{
    public GameObject Obj;
    public Sprite ObjImg;
    public Vector3 OriginalPos;
    public int PickNum;
}

public enum StageResult
{
    Fail = 0,
    Succes = 1,
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
    int SelectNum = 0;
    [SerializeField]
    int ResultNum = 0;
    [SerializeField]
    GameObject Result = null;
    [SerializeField]
    GameObject Shadow = null;
    [SerializeField]
    GameObject SelectObj = null;

    [Space(10)]
    [SerializeField]
    List<ResultObjClass> ResultObjImgs = null;
    [SerializeField]
    List<ShowObj> Objs = null;

    [Header("SquirrelScene_Mgr_Mouse")]
    [Space(10)]
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    Vector2 MaxPos;
    [SerializeField]
    Vector2 MinPos;
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

    #region 내부 시스템
    void ShuffleObj() //shadow 랜덤 및 특정 3개 오브젝트 이미지 변경
    {
        int Ran = Random.Range(0, Objs.Count);
        Shadow.GetComponent<SpriteRenderer>().sprite = ResultObjImgs[Ran].ShadowImg;
        Result.GetComponent<SpriteRenderer>().sprite = ResultObjImgs[Ran].ResultImg;

        ResultNum = ResultObjImgs[Ran].PickNum;

        for (int i = 0; i < Objs.Count; i++)
        {
            Objs[i].ObjImg = ResultObjImgs[i].ResultImg;
            Objs[i].PickNum = ResultObjImgs[i].PickNum;
            Objs[i].Obj.GetComponent<SpriteRenderer>().sprite = Objs[i].ObjImg;
        }
    }

    void InfoReset()
    {
        SelectObj = null;
        SelectNum = 0;
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
                SelectNum = Objs[int.Parse(hit.transform.gameObject.name.Split('_')[1]) - 1].PickNum;
                SelectObj = hit.transform.gameObject;

                hit.collider.gameObject.transform.position = MousePos;
                hit.collider.gameObject.transform.localScale = Vector3.one * 2;
            }
        }
    }

    void MouseUp()
    {
        if (MousePos.x < MaxPos.x && MousePos.x > MinPos.x && MousePos.y < MaxPos.y && MousePos.y > MinPos.y && SelectNum == ResultNum)
        {
            Debug.Log("Yes");
            StartCoroutine(SuccesThisStage(SelectObj, StageResult.Succes));
        }

        else
        {
            Debug.Log("No");
            StartCoroutine(SuccesThisStage(SelectObj, StageResult.Fail));
        }
    }
    #endregion

    #region 오브젝트연출(리스트 등장/삭제, 그림자 등장삭제)
    IEnumerator SuccesThisStage(GameObject Obj ,StageResult Type) // 오브젝트가 맞거나 틀릴 때 연출(매개변수: 연출 할 오브젝트, 연출 효과 타입), (효과: 사이즈 조절, 위치 이동, 알파값 조절)
    {
        yield return null;

        if((int)Type == 0)
        {
            Obj.transform.DOScale(0, ShowTime / 2);

            yield return new WaitForSeconds(ShowTime / 2);

            Obj.transform.localPosition = Objs[int.Parse(Obj.name.Split('_')[1]) - 1].OriginalPos;
            Obj.transform.DOScale(1, ShowTime / 2);
            InfoReset();
        }

        else
        {
            SelectObj.SetActive(false);
            Result.GetComponent<SpriteRenderer>().DOFade(1, ShowTime);
        }
        
    }

    IEnumerator ResultObjsProduce(GameObject obj, ShowType type) // 그림자 출연 연출(매개변수: 연출 할 오브젝트, 연출 효과 타입), (효과: 확대, 색 알파)
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
