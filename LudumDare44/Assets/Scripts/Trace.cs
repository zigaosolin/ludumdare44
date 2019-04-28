
using UnityEngine;

public enum TraceCategory
{
    None = 0,
    PointerMovement = 1,
    PointerLocation = 2,
    Damage = 4,
    Player = 8,
    FallingObject = 16
}

public static class Trace 
{
    public static TraceCategory EnabledMask = TraceCategory.Damage | 
        TraceCategory.Player | TraceCategory.FallingObject;

    // TODO: strip this code in the end!
    public static void Info(TraceCategory category, string message)
    {
        if (!EnabledMask.HasFlag(category))
            return;

        Debug.Log("[" + category.ToString() + "]" + message);
    }
}
