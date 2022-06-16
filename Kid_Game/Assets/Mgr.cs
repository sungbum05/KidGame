using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mgr : MonoBehaviour //�� �� �Ŵ����� ���
{
    [Header("Mgr_Basic_Setting")]
    [SerializeField]
    protected int MaxGameCount = 5;
    [SerializeField]
    protected int CurGameCount = 0;
    [SerializeField]
    protected bool StartChk = false;
    [SerializeField]
    protected bool ClearChk = false;
    [SerializeField]
    protected float ShowTiem = 1.0f;
    [SerializeField]
    protected Image FadePanel;
    [Space(10)]
    [SerializeField]
    List<GameObject> ProgressPoint;

    protected virtual void ProgressSetting()
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
    } // �����Ȳ �� ������Ʈ

    protected List<T> GetShuffleList<T>(List<T> _list) // ���׸� ����Ʈ�� �̿��� ����Ʈ ���� ���� �Լ�
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
