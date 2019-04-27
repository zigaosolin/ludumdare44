using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpMode
{
    Normal,
    WaitingForJumpSequence,
    InJump
}

public class PlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetJumpMode(JumpMode jumpMode)
    {
        switch(jumpMode)
        {
            case JumpMode.InJump:
                spriteRenderer.color = Color.blue;
                break;
            case JumpMode.Normal:
                spriteRenderer.color = Color.white;
                break;
            case JumpMode.WaitingForJumpSequence:
                spriteRenderer.color = Color.yellow;
                break;
        }
    }
}
