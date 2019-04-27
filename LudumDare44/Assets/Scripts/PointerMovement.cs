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
    public Vector2 PositionScreen;

    public override string ToString()
    {
        return $"({PositionScreen.x}, {PositionScreen.y}, {State})";
    }
}


public class PointerMovement : MonoBehaviour
{
    private const int NumberOfStatesKept = 120;   
    private List<PointerMovementState> movements;

    [SerializeField] private float jumpMovementThreshold = 10;
    [SerializeField] private PointerView pointerView;

    private void Awake()
    {
        movements = new List<PointerMovementState>(NumberOfStatesKept + 1);
        for(int i = 0; i < NumberOfStatesKept; ++i)
        {
            movements.Add(new PointerMovementState() { State = PointerState.NotPresent });
        }
    }


    void FixedUpdate()
    {
        var movementState = new PointerMovementState()
        {
            PositionScreen = Input.mousePosition
        };

        var prevMovementState = movements[movements.Count - 1];
        var deltaPosition = movementState.PositionScreen - prevMovementState.PositionScreen;
        switch (prevMovementState.State)
        {
            case PointerState.NotPresent:
                movementState.State = PointerState.InNormalMovement;
                break;
            case PointerState.InNormalMovement:
            case PointerState.InJumpMovement:
                bool isFastMovement = deltaPosition.magnitude > jumpMovementThreshold;
                movementState.State = isFastMovement ? PointerState.InJumpMovement : PointerState.InNormalMovement;             
                break;
        }

        // This can be implemented more efficiently by circular buffer
        // but there is no implementation directly in C#
        movements.Add(movementState);
        movements.RemoveAt(0);

        Trace.Info(TraceCategory.PointerMovement, $"Movement added: {movementState}");

        pointerView.SetView(movementState);
    }
}
