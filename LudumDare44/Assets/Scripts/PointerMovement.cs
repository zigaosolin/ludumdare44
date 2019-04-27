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
    public PointerState State;
    public Vector2 Position;

    public override string ToString()
    {
        return $"({Position.x}, {Position.y}, {State})";
    }
}


public class PointerMovement : MonoBehaviour
{
    private const int NumberOfStatesKept = 120;
    private List<PointerMovementState> movements;
    private Camera camera;
    private float timeSinceLastJump = 100;

    [SerializeField] private float jumpMovementThreshold = 10;
    [SerializeField] private float jumpMovementThresholdHisteresisDown = 3;
    [SerializeField] private PointerView pointerView;
    [SerializeField] private int numberOfJumpMovementToConsiderJump = 3;
    [SerializeField] private float jumpCooldown = 0.5f;

    private void Awake()
    {
        movements = new List<PointerMovementState>(NumberOfStatesKept + 1);
        for (int i = 0; i < NumberOfStatesKept; ++i)
        {
            movements.Add(new PointerMovementState() { State = PointerState.NotPresent });
        }
    }

    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        var position = camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;

        var movementState = new PointerMovementState()
        {
            Position = position
        };

        var prevMovementState = movements[movements.Count - 1];
        var deltaPosition = movementState.Position - prevMovementState.Position;
        if (prevMovementState.State == PointerState.NotPresent)
        {
            movementState.State = PointerState.InNormalMovement;
        }
        else
        {
            float jumpMovementThresh = prevMovementState.State == PointerState.InJumpMovement ?
                jumpMovementThresholdHisteresisDown : jumpMovementThreshold;

            bool isFastMovement = deltaPosition.magnitude > jumpMovementThresh;
            bool canJump = timeSinceLastJump > jumpCooldown || prevMovementState.State == PointerState.InJumpMovement;

            movementState.State = (isFastMovement && canJump) ? PointerState.InJumpMovement : PointerState.InNormalMovement;
        }

        var viewportPosition = camera.ScreenToViewportPoint(Input.mousePosition);
        bool isOutOfBounds = viewportPosition.x < 0 || viewportPosition.x > 1
            || viewportPosition.y < 0 || viewportPosition.y > 1;

        if(isOutOfBounds)
        {
            movementState.State = PointerState.NotPresent;
        }

        // This can be implemented more efficiently by circular buffer
        // but there is no implementation directly in C#
        movements.Add(movementState);
        movements.RemoveAt(0);

        if(movementState.State == PointerState.InJumpMovement)
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
            if(movement.State != PointerState.InJumpMovement)
            {
                break;
            }
        }

        bool isConsideredInJump = movements.Count - index > numberOfJumpMovementToConsiderJump;

        // Last not jump position
        return (movements[index].Position, isConsideredInJump);
    }
}
