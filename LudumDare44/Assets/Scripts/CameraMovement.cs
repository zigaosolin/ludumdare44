using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        var position = transform.localPosition;
        position.y += movementSpeed * Time.deltaTime;
        transform.localPosition = position;
    }
}
