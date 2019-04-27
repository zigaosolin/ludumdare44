using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointerState
{
    NotPresent,
    InJumpMovement,
    InNormalMovement
}

public struct PointerMovementState
{
    public PointerState SmoothState;
    public Vector2 Position;
    public PointerState ActualState;

    public override string ToString()
    {
        return $"({Position.x}, {Position.y}, {SmoothState})";
    }
}


public class PointerMovement : MonoBehaviour
{
    private const int NumberOfStatesKept = 360;
    private const int FramesInNormalToStopJump = 5;
    private List<PointerMovementState> movements;
    private Camera camera;
    private float timeSinceLastJump = 100;

    [SerializeField] private float jumpMovementThreshold = 10;
    [SerializeField] private float jumpMovementThresholdHisteresisDown = 3;
    [SerializeField] private PointerView pointerView;
    [SerializeField] private float jumpCooldown = 0.5f;

    private void Awake()
    {
        movements = new List<PointerMovementState>(NumberOfStatesKept + 1);
        for (int i = 0; i < NumberOfStatesKept; ++i)
        {
            movements.Add(new PointerMovementState() { SmoothState = PointerState.NotPresent });
        }
    }

    private void Start()
    {
        camera = Camera.main;       
    }

    void FixedUpdate()
    {
        var position = camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;

        var movementState = new PointerMovementState()
        {
            Position = position
        };

        var prevMovementState = movements[movements.Count - 1];
        var deltaPosition = movementState.Position - prevMovementState.Position;
        if (prevMovementState.SmoothState == PointerState.NotPresent)
        {
            movementState.SmoothState = PointerState.InNormalMovement;
            movementState.ActualState = PointerState.InNormalMovement;
        }
        else
        {
            // Needs to drop below some threshold to be considered normal again if already in jump
            float jumpMovementThresh = prevMovementState.SmoothState == PointerState.InJumpMovement ?
                jumpMovementThresholdHisteresisDown : jumpMovementThreshold;

            bool isFastMovement = deltaPosition.magnitude > jumpMovementThresh;
            bool canJump = timeSinceLastJump > jumpCooldown || 
                prevMovementState.SmoothState == PointerState.InJumpMovement;

            movementState.ActualState = (isFastMovement && canJump) ?
                PointerState.InJumpMovement : PointerState.InNormalMovement;

            if(movementState.ActualState == PointerState.InNormalMovement || 
                prevMovementState.SmoothState == PointerState.InJumpMovement)
            {
                bool stopJump = true;
                for(int i = 0; i < FramesInNormalToStopJump; ++i)
                {
                    var movement = movements[movements.Count - i - 1];
                    if(movement.ActualState == PointerState.InJumpMovement)
                    {
                        stopJump = false;
                        break;
                    }            
                }

                if(stopJump)
                {
                    movementState.SmoothState = PointerState.InNormalMovement;
                } else
                {
                    movementState.SmoothState = PointerState.InJumpMovement;
                }
            } else
            {
                movementState.SmoothState = movementState.ActualState;
            }
        }

        var viewportPosition = camera.ScreenToViewportPoint(Input.mousePosition);
        bool isOutOfBounds = viewportPosition.x < 0 || viewportPosition.x > 1
            || viewportPosition.y < 0 || viewportPosition.y > 1;

        if(isOutOfBounds)
        {
            viewportPosition.x = Mathf.Clamp01(viewportPosition.x);
            viewportPosition.y = Mathf.Clamp01(viewportPosition.y);

            movementState.Position = camera.ViewportToWorldPoint(viewportPosition);
            movementState.SmoothState = PointerState.NotPresent;
            movementState.ActualState = PointerState.NotPresent;
        }

        // This can be implemented more efficiently by circular buffer
        // but there is no implementation directly in C#
        movements.Add(movementState);
        movements.RemoveAt(0);

        if(movementState.SmoothState == PointerState.InJumpMovement)
        {
            timeSinceLastJump = 0;
        } else
        {
            timeSinceLastJump += Time.deltaTime;
        }

        Trace.Info(TraceCategory.PointerMovement, $"Movement added: {movementState}");

        pointerView.SetView(movementState);
    }

    public (Vector2 targetLocation, bool isInJumpMovement) GetTarget()
    {
        int index = movements.Count - 50;
        for(int i = 0; i < NumberOfStatesKept; ++i)
        {
            index = movements.Count - i - 1;
            var movement = movements[index];
            if(movement.SmoothState != PointerState.InJumpMovement)
            {
                break;
            }
        }

        bool isConsideredInJump = movements.Count - index > 1;

        // Last not jump position
        return (movements[index].Position, isConsideredInJump);
    }
}
