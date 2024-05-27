using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - new Vector3(0f, -2f);
    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
