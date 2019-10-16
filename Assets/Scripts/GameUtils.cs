using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils
{
    public static bool LayerMaskContains(int layer, LayerMask lm)
    {
        return (lm == (lm | (1 << layer)));
    }

    public static void SetEulerAngles(Transform t, Vector3 euler)
    {
        Quaternion q = t.rotation;
        q.eulerAngles = euler;
        t.rotation = q;
    }

    public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static Vector3 MidPoint(Vector3 a, Vector3 b)
    {
        return new Vector3((a.x + b.x) / 2, (a.y + b.y) / 2, (a.z + b.z) / 2);
    }
}
