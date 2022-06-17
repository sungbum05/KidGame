using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum ShowType
{
    Hide = 0,
    Spawn = 1,
}

public class SquirrelGameMgr : Mgr
{
    [Header("SquirrelScene_Mgr_attribute")]
    [SerializeField]
    GameObject Shadow = null;
    [SerializeField]
    List<Sprite> ShadowImgs = null;
    [SerializeField]
    List<GameObject> Objs = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartGame()
    {
        yield return null;

        if (StartChk == true) // ���� �ϴ� ��
        {
            
        }

        else if (StartChk == false) //���� �����ϱ� ��
        {
            for (int i = 0; i < Objs.Count; i++)
            {
                StartCoroutine(ObjListProduce(Objs[i], ShowType.Spawn));
                yield return new WaitForSeconds(ShowTime);
            }
        }

        else if(StartChk == false && CurGameCount > MaxGameCount) //���� ����
        {
            
        }

        CurGameCount += 1;
    }

    #region ������Ʈ����(����Ʈ ����/����, �׸��� �������)
    IEnumerator ShadowProduce(GameObject obj, ShowType type) // �׸��� �⿬ ����(�Ű�����: ���� �� ������Ʈ, ���� ȿ��), (ȿ��: Ȯ��, ȸ��)
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
            Debug.Log(Obj.transform.localScale.x);

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
