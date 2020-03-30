using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockNotify : MonoBehaviour
{
    void Start()
    {
        // UnlockSystem.instance.unlockedLevelNotification.Add(0);
        UnlockSystem.instance.TickleNotifications();
    }

}
