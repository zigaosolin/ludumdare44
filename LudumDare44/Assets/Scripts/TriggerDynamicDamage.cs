using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDynamicDamage : MonoBehaviour
{
    bool wasTrigerred = false;
    public GameObject trigerredObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || wasTrigerred)
            return;

        Trace.Info(TraceCategory.FallingObject, "Trigerred dynamic object");
        trigerredObject.SetActive(true);
        wasTrigerred = true;
    }
}
