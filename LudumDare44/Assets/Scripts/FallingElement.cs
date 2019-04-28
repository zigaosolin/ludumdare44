using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingElement : MonoBehaviour
{
    [SerializeField] private float FallSpeed = 0.3f;
    private float fallingTime = 0;

    private void Update()
    {
        var position = transform.position;
        position.y -= FallSpeed * Time.deltaTime;
        transform.position = position;

        fallingTime += Time.deltaTime;
        if(fallingTime > 10)
        {
            gameObject.SetActive(false);
        }
    }
}
