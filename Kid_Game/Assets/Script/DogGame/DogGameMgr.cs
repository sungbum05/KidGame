using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGameMgr : Mgr   
{
    [Header("Dog_Mgr_attribute")]
    [SerializeField]
    GameObject FindCircle = null;
    [SerializeField]
    List<GameObject> Objs = null;
    [SerializeField]
    List<Transform> ObjSpawnPos = null;

    [Header("Dog_Mgr_Mouse")]
    [Space(10)]
    [SerializeField]
    LayerMask layerMask;
    Vector2 MousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
            MouseClick();
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
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (ClearChk == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(MousePos, transform.forward, 10.0f, ClearLayer);
            if (hit)
            {

            }
        }

        else
        {
            FindCircle.transform.position = MousePos;
        }
    }
    #endregion

    public void ObjRandomSpawn()
    {
        GetShuffleList<Transform>(ObjSpawnPos);
    }
}
