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
        GetShuffleList<Transform>(ObjSpawnPos);

        for (int i = 0; i < Objs.Count; i++)
        {
            Objs[i].transform.localPosition = ObjSpawnPos[i].transform.localPosition;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
