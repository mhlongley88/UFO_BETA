using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBossTargetGroup : MonoBehaviour
{
    public AddToTargetGroup targetGroup;

    void OnDie()
    {
        targetGroup.RemoveFromTargetGroup();
    }
}
