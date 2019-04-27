
using UnityEngine;

public enum TraceCategory
{
    None = 0,
    PointerMovement = 1,
    PointerLocation = 2
}

public static class Trace 
{
    public static TraceCategory EnabledMask = TraceCategory.None;

    // TODO: strip this code in the end!
    public static void Info(TraceCategory category, string message)
    {
        if (!EnabledMask.HasFlag(category))
            return;

        Debug.Log("[" + category.ToString() + "]" + message);
    }
}
