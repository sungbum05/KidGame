using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCircle : MonoBehaviour
{
    [SerializeField]
    Collider2D ObjCollider = null;
    [Range(0.0f, 5.0f), SerializeField]
    float ColliderRadius = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ObjCollider = Physics2D.OverlapCircle(transform.position, ColliderRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ColliderRadius);
    }
}
