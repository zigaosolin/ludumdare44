using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    void Awake()
    {
        
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
    }
}
