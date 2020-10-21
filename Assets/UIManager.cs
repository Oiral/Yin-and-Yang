using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text moveCounter;

    public void UpdateMoveCount(int num)
    {
        moveCounter.text = num.ToString();

        StopAllCoroutines();
        StartCoroutine(MoveCounterUpdate(0.3f));
    }

    IEnumerator MoveCounterUpdate(float moveTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            Vector3 scale = moveCounter.transform.localScale;

            scale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, elapsedTime / moveTime);

            moveCounter.transform.localScale = scale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moveCounter.transform.localScale = Vector3.one;
    }
}
