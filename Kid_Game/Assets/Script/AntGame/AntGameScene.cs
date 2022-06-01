using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AntGameScene : MonoBehaviour
{
    [SerializeField]
    int SelectNum;
    [SerializeField]
    int PickNum;
    [SerializeField]
    bool StartChk = false;
    [SerializeField]
    int MaxCount = 5;
    [SerializeField]
    int CurCount = 0;

    [Space(10)]
    [SerializeField]
    private Vector2 EnterPos; // ���� ���� ����
    [SerializeField]
    private Vector2 StayPos; // ���� �ӹ��� ����
    [SerializeField]
    private Vector2 ExitPos; // ���� ������ ����

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            MouseClick();
        }

        if(Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }
    }

    IEnumerator StartGame()
    {
        yield return null;
        Destroy(Ants);
        Ants = null;

        StartCoroutine(EnterAnt());
        StartCoroutine(BreadPosChange());

        CurCount++;
    }

    #region  ���콺 ��ȣ�ۿ� �Լ���
    void MouseClick() // ���콺�� ������ �ִµ��� ray����
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

    #region ���� ����
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

    IEnumerator BreadPosChange() // ���� ��ġ�� �ٲ��ش�(���� ����)
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
    }//(��ó : https://drehzr.tistory.com/802)
}
