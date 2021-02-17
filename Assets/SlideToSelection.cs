using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideToSelection : MonoBehaviour
{
    public GameObject item;
    public float moveSpeed = 5f;
    private void Update()
    {
        if (item != null)
        {
            transform.position = Vector3.Lerp(transform.position, item.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void SetItem(GameObject _item)
    {
        item = _item;
    }
}
