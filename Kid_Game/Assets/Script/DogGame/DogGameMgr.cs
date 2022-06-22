using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGameMgr : Mgr   
{
    [Header("Dog_Mgr_attribute")]
    [SerializeField]
    GameObject FindCircle = null;

    [Header("SquirrelScene_Mgr_Mouse")]
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

    #region  ���콺 ��ȣ�ۿ� �Լ���
    void MouseClick() // ���콺�� ������ �ִµ��� ray����
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
}
