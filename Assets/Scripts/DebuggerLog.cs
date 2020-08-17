using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerLog : MonoBehaviour
{
    public string output = "";
    public string stack = "";

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
    }
}