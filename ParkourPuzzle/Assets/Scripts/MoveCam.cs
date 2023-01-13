using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{

    public Transform targetPosition;

    void Update()
    {
        transform.position = targetPosition.position;
    }
}
