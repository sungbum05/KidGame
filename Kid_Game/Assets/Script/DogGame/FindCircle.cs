using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCircle : MonoBehaviour
{
    [SerializeField]
    Collider2D ObjCollider = null;
    [Range(0.0f, 5.0f), SerializeField]
    float ColliderRadius = 0;
    [Range(0, 10), SerializeField]
    float CoolTime = 0;
    [SerializeField]
    float CurCoolTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        CurCoolTime = CoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        ObjCollider = Physics2D.OverlapCircle(transform.position, ColliderRadius);

        SendObj();
    }

    private void SendObj()
    {
        if(ObjCollider != null && CurCoolTime <= 0)
        {
            Debug.Log("yes");
            //ObjCollider.gameObject
        }

        else if(ObjCollider != null)
        {
            CurCoolTime -= Time.deltaTime;
        }

        else if(ObjCollider == null)
        {
            CurCoolTime = CoolTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ColliderRadius);
    }
}
