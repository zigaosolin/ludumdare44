using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 0.1f;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        var speedup = Input.GetMouseButton(0) || Input.GetMouseButton(1) ? 2.5f : 1f;
        if (player.PreparingToJump)
            speedup = 1;

        var position = transform.localPosition;
        position.y += movementSpeed * Time.deltaTime * speedup;
        transform.localPosition = position;
    }
}
