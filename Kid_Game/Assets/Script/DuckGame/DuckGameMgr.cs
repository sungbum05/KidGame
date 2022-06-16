using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class DuckGameMgr : Mgr
{
    [Header("DuckScene_Mgr_attribute")]
    #region 게임 시작
    [SerializeField]
    private string SelectColor; // 선택된 컬러 받아올 변수

    [SerializeField]
    private List<string> g_Color; // 컬러 챗박스에 색깔을 배정할 컬러

    [SerializeField]
    private SpriteRenderer ChatBox; // 오리들에게 색깔을 배정할 컬러

    [SerializeField]
    private List<Sprite> ColorChatBox; // 엄마 오리가 부를 컬러 이미지들

    public Dictionary<string, Sprite> ColorChatBoxkDic = new Dictionary<string, Sprite>();


    [SerializeField]
    private List<Button> BabyDuckBtns; // 애기 오리들 상호 작용 버튼
    #endregion

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

    private void Awake()
    {
        DicSetting();
    }

    // Start is called before the first frame update
    void Start()
    {
        HomeBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SelectStageScene");
        });
        HomeBtn.gameObject.SetActive(false);

        StartCoroutine(StartGame());

        foreach (Button Btn in BabyDuckBtns)
        {
            Btn.onClick.AddListener(() =>
            {
                if (Btn.GetComponent<BabyDuckInfo>().BabyColor == SelectColor)
                {
                    StartCoroutine(StartGame());
                    Debug.Log(Btn.name);
                }
            });
        }  
    }

    // Update is called once per frame
    void Update()
    {
        ProgressSetting();

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Down");
            MouseClick();
        }

        if(CurGameCount > MaxGameCount && StartChk == true)
        {
            StartChk = false;
            ClearChk = true;
            StartCoroutine(ClearShow());
        }
    }

    void MouseClick() // 마우스를 누르고 있는동안 ray실행
    {
        if (ClearChk == true)
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(MousePos, transform.forward, 10.0f, ClearLayer);
            if (hit)
            {
                Instantiate(balloonburst, new Vector2 (hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + hit.collider.gameObject.GetComponent<BoxCollider2D>().offset.y), Quaternion.identity);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    protected override void ProgressSetting()
    {
        base.ProgressSetting();
    }

    IEnumerator StartGame()
    {
        yield return null;

        if(StartChk == true)
        {
            StartCoroutine(ExitDuck());
            yield return new WaitForSeconds(1.0f);
        }

        if (StartChk == false)
        {
            FadePanel.DOFade(0, ShowTiem / 1.2f);

            StartChk = true;

            GetShuffleList<string>(g_Color);
            SettingBabyDuck();
            SettingColorChatBox();
            yield return new WaitForSeconds(ShowTiem / 1.2f);
            FadePanel.gameObject.SetActive(false);
        }

        else
        {
            StartChk = true;

            GetShuffleList<string>(g_Color);
            SettingBabyDuck();
            SettingColorChatBox();
        }

        CurGameCount += 1;
    }

    IEnumerator ExitDuck()
    {
        yield return null;

        foreach (Button Btn in BabyDuckBtns)
        {
            Btn.gameObject.GetComponent<Image>().DOFade(0, 1);
        }

        yield return new WaitForSeconds(1.4f);

        StartCoroutine(EnterDuck());
    }

    IEnumerator EnterDuck()
    {
        yield return null;

        foreach (Button Btn in BabyDuckBtns)
        {
            Btn.gameObject.GetComponent<Image>().DOFade(1, 1);
        }
    }

    void DicSetting()
    {
        foreach (Sprite Img in ColorChatBox)
        {
            string[] NameSplit = Img.name.Split('_');
            ColorChatBoxkDic.Add(NameSplit[1], Img);
        }
    } // Dic세팅

    private void SettingBabyDuck()
    {
        int i = 0;

        foreach(Button Btn in BabyDuckBtns)
        {
            Btn.GetComponent<BabyDuckInfo>().BabyColor = g_Color[i];
            Btn.GetComponent<BabyDuckInfo>().ColorSetting();
            i++;
        }

        i = 0;
    } // 아기 오리들 색깔 변경

    private void SettingColorChatBox()
    {
        int SelectColorIdx = Random.Range(0, 3);
        SelectColor = g_Color[SelectColorIdx];

        ChatBox.sprite = ColorChatBoxkDic[SelectColor];
    } // 챗박스 색깔 변경

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
