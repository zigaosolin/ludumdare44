using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private PointerMovement pointerMovement;

    [SerializeField]
    private float normalInterpolateSpeed = 3;

    void Update()
    {
        var (targetPosition, isInJumpMovement) = pointerMovement.GetTarget();

        var currentPosition = transform.position;
        transform.position = Vector2.Lerp(currentPosition, targetPosition, Time.deltaTime * normalInterpolateSpeed);
    }
}
