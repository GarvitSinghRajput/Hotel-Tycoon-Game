using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoin : MonoBehaviour
{
    public GameObject objectToRotate;
    public float speed = 10f;

    void Update()
    {
        objectToRotate.transform.Rotate(0, Time.deltaTime * speed, 0, Space.Self);
    }
}
