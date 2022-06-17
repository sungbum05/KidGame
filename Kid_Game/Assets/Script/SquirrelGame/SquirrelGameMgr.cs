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

        if (StartChk == true) // 게임 하는 중
        {
            
        }

        else if (StartChk == false) //게임 시작하기 전
        {
            for (int i = 0; i < Objs.Count; i++)
            {
                StartCoroutine(ObjListProduce(Objs[i], ShowType.Spawn));
                yield return new WaitForSeconds(ShowTime);
            }
        }

        else if(StartChk == false && CurGameCount > MaxGameCount) //게임 끝남
        {
            
        }

        CurGameCount += 1;
    }

    #region 오브젝트연출(리스트 등장/삭제, 그림자 등장삭제)
    IEnumerator ShadowProduce(GameObject obj, ShowType type) // 그림자 출연 연출(매개변수: 연출 할 오브젝트, 연출 효과), (효과: 확대, 회전)
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
