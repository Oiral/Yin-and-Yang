using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UISlider : MonoBehaviour
{
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startingPos = rectTransform.anchoredPosition;
    }

    public float time;
    public Vector3 endPos;
    Vector3 startingPos;

    public bool slide;

    public void Toggle(bool toggle)
    {
        slide = toggle;
        StopAllCoroutines();
        if (slide)
        {
            StartCoroutine(MovePos(endPos));
        }
        else
        {
            StartCoroutine(MovePos(startingPos));
        }
    }

    IEnumerator MovePos(Vector3 targetPos)
    {
        float elapsedTime = 0;
        Vector3 originalPos = rectTransform.anchoredPosition;
        while (elapsedTime < time)
        {
            rectTransform.anchoredPosition = Vector3.Slerp(originalPos, targetPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

        rectTransform.anchoredPosition = targetPos;


        
    }
}
