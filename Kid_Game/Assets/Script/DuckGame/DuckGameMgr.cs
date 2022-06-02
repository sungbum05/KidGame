using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class DuckGameMgr : MonoBehaviour
{
    #region ���� ����
    [SerializeField]
    private string SelectColor; // ���õ� �÷� �޾ƿ� ����

    bool StartChk = false;
    [SerializeField]
    Image FadePanel;
    [SerializeField]
    float ShowTiem = 1.0f;

    [SerializeField]
    int MaxGameCount = 5;

    [SerializeField]
    int CurGameCount = 0;

    [SerializeField]
    private List<string> g_Color; // �÷� ê�ڽ��� ������ ������ �÷�

    [SerializeField]
    private SpriteRenderer ChatBox; // �����鿡�� ������ ������ �÷�

    [SerializeField]
    private List<Sprite> ColorChatBox; // ���� ������ �θ� �÷� �̹�����

    public Dictionary<string, Sprite> ColorChatBoxkDic = new Dictionary<string, Sprite>();


    [SerializeField]
    private List<Button> BabyDuckBtns; // �ֱ� ������ ��ȣ �ۿ� ��ư

    [Space(10)]
    [SerializeField]
    List<GameObject> ProgressPoint;
    #endregion

    #region ���� �� ����
    [Space(10)]
    [SerializeField]
    bool ClearChk = false;
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

    void MouseClick() // ���콺�� ������ �ִµ��� ray����
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

    void ProgressSetting()
    {
        if (CurGameCount > 0)
        {
            int i = 0;

            foreach (GameObject obj in ProgressPoint)
            {
                if (i <= CurGameCount - 1)
                {
                    obj.GetComponent<Image>().color = Color.white;
                }

                i++;
            }

            i = 0;
        }
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
    } // Dic����

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
    } // �Ʊ� ������ ���� ����

    private void SettingColorChatBox()
    {
        int SelectColorIdx = Random.Range(0, 3);
        SelectColor = g_Color[SelectColorIdx];

        ChatBox.sprite = ColorChatBoxkDic[SelectColor];
    } // ê�ڽ� ���� ����

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

    public List<T> GetShuffleList<T>(List<T> _list) // ���׸� ����Ʈ�� �̿��� ����Ʈ ���� ���� �Լ�
    {
        for (int i = _list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1); // 1 �߰� �ؼ� ù�� ° ������Ʈ�� ù��°�� ���� �� �ֵ��� ������

            T temp = _list[i];
            _list[i] = _list[rnd];
            _list[rnd] = temp;
        }

        return _list;
    }//(��ó : https://drehzr.tistory.com/802) // ����: sblim05
}
