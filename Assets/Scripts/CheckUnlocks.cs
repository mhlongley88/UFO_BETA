using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUnlocks : MonoBehaviour
{
    public void Check()
    {
        if (UnlockSystem.instance.HasThresholdForCharacter())
            UnlockNotification.instance.SignalUnlockCharacter();

        if (UnlockSystem.instance.HasThresholdForLevels())
            UnlockNotification.instance.SignalUnlockLevel();
    }
}
