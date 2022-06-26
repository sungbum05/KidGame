using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMove : MonoBehaviour
{
    public IEnumerator MoveToObj(GameObject SelectObj, GameObject AnswerObj)
    {
        yield return null;

        float UserTime = 1;

        while (true)
        {
            yield return null;

            UserTime += (Time.deltaTime * 3);

            SelectObj.transform.position = Vector2.Lerp(SelectObj.transform.position,
                AnswerObj.transform.position, 1.5f * Time.deltaTime * UserTime);

            if (SelectObj.transform.position == AnswerObj.transform.position)
                break;
        }

        StopCoroutine(MoveToObj(SelectObj, AnswerObj));
    }
}
