using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class AntGameScene : Mgr
{
    [Header("AntScene_Mgr_attribute")]
    [SerializeField]
    int SelectNum;
    [SerializeField]
    int PickNum;

    [Space(10)]
    [SerializeField]
    private Vector2 EnterPos; // 개미 생성 지점
    [SerializeField]
    private Vector2 StayPos; // 개미 머무는 지점
    [SerializeField]
    private Vector2 ExitPos; // 개미 나가는 지점

    [Space(10)]
    [SerializeField]
    private GameObject Breads;
    [SerializeField]
    private Vector2 BreadStayPos;
    [SerializeField]
    private Vector2 BreadExitPos;
    [SerializeField]
    private List<GameObject> Bread;
    [SerializeField]
    private List<GameObject> BreadPos;
    [SerializeField]
    private float ShowTime = 1.0f;

    [Space(10)]
    [SerializeField]
    LayerMask layerMask;
    Vector2 MousePos;

    [Space(10)]
    [SerializeField]
    private List<GameObject> AntGroup;
    [SerializeField]
    GameObject Ants;
    [SerializeField]
    BoxCollider2D AntZone;
    [SerializeField]
    Sprite AntChangeImg;

    #region 게임 끝 연출
    [Space(10)]
    [SerializeField]
    Vector3 BallonSpawnPoint;
    [SerializeField]
    List<GameObject> SideClearBallon;
    [SerializeField]
    GameObject MainClearBallon;
    [SerializeField]
    GameObject balloonburst;
    [SerializeField]
    LayerMask ClearLayer;
    [SerializeField]
    Button HomeBtn;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        HomeBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SelectStageScene");
        });
        HomeBtn.gameObject.SetActive(false);

        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        ProgressSetting();

        if (Input.GetMouseButton(0))
        {
            MouseClick();
        }

        if(Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }

        if (CurGameCount > MaxGameCount && ClearChk == false)
        {
            ClearChk = true;
            StartCoroutine(ClearShow());
        }
    }

    protected override void ProgressSetting()
    {
        base.ProgressSetting();
    }

    IEnumerator StartGame()
    {
        yield return null;
        if (StartChk == false)
        {
            yield return null;
            FadePanel.DOFade(0, ShowTiem / 1.2f);
            yield return new WaitForSeconds(ShowTiem / 1.2f);
            FadePanel.gameObject.SetActive(false);
        }

        Destroy(Ants);
        Ants = null;

        StartCoroutine(EnterAnt());
        StartCoroutine(BreadPosChange());

        CurGameCount++;
    }

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
                PickNum = int.Parse(SpiltName[1]);
                hit.collider.gameObject.transform.position = MousePos;
            }
        }
    }

    void MouseUp()
    {
        if((Mathf.Abs(MousePos.x) < Mathf.Abs(AntZone.bounds.extents.x) && Mathf.Abs(MousePos.y) < Mathf.Abs(AntZone.bounds.extents.y)) && SelectNum == PickNum)
        {
            Debug.Log("Yes");
            foreach(Transform Child in Ants.transform)
            {
                Child.gameObject.GetComponent<SpriteRenderer>().sprite = AntChangeImg;
                Bread[PickNum - 1].SetActive(false);
            }

            StartCoroutine(ExitAnt());
        }

        else
        {
            Bread[PickNum - 1].transform.DOMove(BreadPos[PickNum - 1].transform.position, ShowTime / 2);
            PickNum = 0;
        }
    }
    #endregion

    #region 개미 연출
    IEnumerator EnterAnt()
    {
        yield return null;
        GetShuffleList<GameObject>(AntGroup);
        string[] SplitName = AntGroup[0].name.Split('_');
        SelectNum = int.Parse(SplitName[1]);

        Ants = Instantiate(AntGroup[0], EnterPos, Quaternion.identity);
        Ants.transform.DOMove(StayPos, ShowTime * 1.5f);

        yield return new WaitForSeconds(ShowTime * 2.0f);
    }

    IEnumerator ExitAnt()
    {
        yield return null;
        Ants.transform.DOMove(ExitPos, ShowTime * 2.0f);

        yield return new WaitForSeconds(ShowTime * 2);

        StartCoroutine(StartGame());
    }
    #endregion

    IEnumerator BreadPosChange() // 빵의 위치를 바꿔준다(연출 포함)
    {
        yield return null;

        if (StartChk == true)
        {
            Bread[PickNum - 1].transform.position = BreadPos[PickNum - 1].transform.position;
            Breads.transform.DOMove(BreadExitPos, ShowTime);

            yield return new WaitForSeconds(ShowTime);
        }

        int i = 0;

        GetShuffleList<GameObject>(BreadPos);

        foreach(Transform Child in Breads.transform)
        {
            Child.transform.position = new Vector2(BreadPos[i].transform.position.x, Child.transform.position.y);
            string[] SplitName = BreadPos[i].name.Split('_');
            Child.gameObject.GetComponent<Bread>().Pos = int.Parse(SplitName[1]);
            i++;
        }

        if (StartChk == true)
        {
            Debug.Log("asdad");
            Bread[PickNum - 1].SetActive(true);
            PickNum = 0;

            yield return new WaitForSeconds(ShowTime / 2);       
        }

        Breads.transform.DOMove(BreadStayPos, ShowTime);

        StartChk = true;
        i = 0;
    }

    IEnumerator ClearShow()
    {
        yield return null;
        FadePanel.gameObject.SetActive(true);

        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

            BallonSpawnPoint = new Vector3(Random.Range(-8.0f, 8.0f), -13.0f, 0f);
            Instantiate(SideClearBallon[Random.Range(0, SideClearBallon.Count)], BallonSpawnPoint, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.5f);
        Instantiate(MainClearBallon, new Vector3(0, -13.0f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        HomeBtn.gameObject.SetActive(true);
    }

   
}
