﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUnlocks : MonoBehaviour
{
    public void Check()
    {
        UnlockSystem.instance.TickleNotifications();
    }
}
