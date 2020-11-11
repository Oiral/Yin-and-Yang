using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    public float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}
