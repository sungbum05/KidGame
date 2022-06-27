using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
class MoveObj
{
    public GameObject Obj;
    public Vector2 OrizinalPos;
    public Vector2 FixPos;
    public bool SuccesChk = false;
}

[System.Serializable]
class Stage6CanMoveObj
{
    public List<MoveObj> ObjType = null;
}

public class RabbitGameMgr : Mgr
{
    [Header("Rabbit_Mgr_attribute")]
    [SerializeField]
    GameObject SelectObj = null;
    [SerializeField]
    GameObject AnswerObj = null;
    [SerializeField]
    List<Stage6CanMoveObj> stage6CanMoveObjs = null;
    [SerializeField]
    List<GameObject> SlideObjs = null;

    [Header("Rabbit_Mgr_Mouse")]
    [Space(10)]
    [SerializeField]
    LayerMask Selectlayer;
    [SerializeField]
    LayerMask Answerlayer;
    Vector2 MousePos;

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

        #region 게임 중
        if (StartChk == true) // 게임 하는 중
        {

        }
        #endregion

        #region 게임 시작 전
        else if (StartChk == false) //게임 시작하기 전
        {
            FadePanel.DOFade(0, ShowTime / 1.2f);
            StartChk = true;

            SlideObj();
        }
        #endregion

        #region 게임 종료
        if (ClearChk == true && CurGameCount > MaxGameCount) //게임 끝남
        {

        }
        #endregion
    }

    #region  마우스 상호작용 함수들
    void MouseClick() // 마우스를 누르고 있는동안 ray실행
    {
        if (ClearChk == true)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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

            RaycastHit2D Objhit = Physics2D.Raycast(MousePos, transform.forward, 10.0f, Selectlayer);
            if (Objhit)
            {
                Objhit.collider.transform.position = MousePos;
                SelectObj = Objhit.collider.gameObject;
            }

            RaycastHit2D Answerhit = Physics2D.Raycast(MousePos, transform.forward, 10.0f, Answerlayer);
            if (Answerhit)
            {
                AnswerObj = Answerhit.collider.gameObject;
            }
        }
    }

    void MouseUp()
    {
        if (SelectObj != null)
        {
            #region Obj이동
            int ChkNum = 0;

            int SelectObjNum = int.Parse(SelectObj.name.Split('_')[1]);
            int AnswerObjNum = int.Parse(AnswerObj.name.Split('_')[1]);

            int ObjNumberSum = SelectObjNum - AnswerObjNum;

            if (ObjNumberSum == 0)
            {
                SelectObj.GetComponent<ObjShowMove>().StartObjShow();

                SelectObj.transform.position = stage6CanMoveObjs[CurGameCount].ObjType[SelectObjNum].FixPos;
                stage6CanMoveObjs[CurGameCount].ObjType[SelectObjNum].SuccesChk = true;
                
                SelectObj.GetComponent<BoxCollider2D>().enabled = false;
                SelectObj.GetComponent<SpriteRenderer>().sortingOrder = 2 + CurGameCount;

                SelectObj = null;
            }

            else
            {
                SelectObj.transform.position = stage6CanMoveObjs[CurGameCount].ObjType[SelectObjNum].OrizinalPos;
            }
            #endregion

            #region 같은타입 오브젝트 성공 확인
            foreach (var Obj in stage6CanMoveObjs[CurGameCount].ObjType)
            {
                if (Obj.SuccesChk)
                    ChkNum ++;
            }

            if(ChkNum >= stage6CanMoveObjs[CurGameCount].ObjType.Count)
            {
                CurGameCount++;
                SlideObj();
            }
            #endregion
        }
    }
    #endregion

    #region 오브젝트 이동 및 연출 효과
    void SlideObj()
    {
        if(CurGameCount < SlideObjs.Count)
            SlideObjs[CurGameCount].gameObject.transform.DOMove(Vector2.zero, ShowTime);

        else
        {
            StartCoroutine(ClearShow());
        }

    }
    #endregion

    protected override IEnumerator ClearShow()
    {
        yield return null;
        ClearChk = true;

        foreach (var obj in stage6CanMoveObjs[0].ObjType)
        {
            obj.Obj.transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return base.ClearShow();
    }
}
