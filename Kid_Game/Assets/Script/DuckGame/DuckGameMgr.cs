using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DuckGameMgr : MonoBehaviour
{
    [SerializeField]
    private string SelectColor; // ���õ� �÷� �޾ƿ� ����

    bool StartChk = false;

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

    private void Awake()
    {
        DicSetting();
    }

    // Start is called before the first frame update
    void Start()
    {
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

        StartChk = true;
        GetShuffleList<string>(g_Color);
        SettingBabyDuck();
        SettingColorChatBox();

        CurGameCount+=1;
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
