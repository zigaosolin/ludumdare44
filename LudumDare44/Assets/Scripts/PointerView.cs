﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetView(PointerMovementState state)
    {
        switch(state.State)
        {
            case PointerState.InJumpMovement:
                spriteRenderer.color = Color.yellow;
                break;
            case PointerState.InNormalMovement:
                spriteRenderer.color = Color.white;
                break;
            case PointerState.NotPresent:
                spriteRenderer.color = Color.red;
                break;
        }

        var position = Camera.main.ScreenToWorldPoint(state.PositionScreen);
        position.z = 0;
        gameObject.transform.position = position;

        Trace.Info(TraceCategory.PointerLocation, $"New loc: {gameObject.transform.position}");
    }
}
