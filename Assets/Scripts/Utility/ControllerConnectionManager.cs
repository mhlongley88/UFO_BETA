/// <summary>
/// Created 03/10/19 by Spectralle from Fiverr.
/// 
/// This script checks for changes in the list of connected control devices every UpdateFrequencyInSeconds seconds.
/// 
/// NOTE:
/// If you set Time.timescale to ZERO when a controller change is detected, it WILL stop checking, making the script non-functional in game.
/// So you'll have to work out an alternative if you need that capability.
/// 
/// ADDITIONAL NOTE:
/// I have limited experience working with controllers in Unity's input manager, so it is likely this script could be much better.
/// Given the requirements you asked of me, I would recommend upgrading to a more complete input system when possible,
/// since that will cover this functionality and add better controller and console support.
/// Systems I'd recommend are:
/// 1 Unity's new Input System, coming out soon.
/// 2 InControl, by Gallant Games (Paid asset).
/// 3 Rewired, by Guavaman Enterprises (More expensive paid asset).
/// 
/// This script may suit you for now, but I wouldn't recommend it as a long-term solution.
/// 
/// </summary> 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerConnectionManager : MonoBehaviour
{
    // Create a singleton that can be referenced from any script ( E.g. "ControllerConnectionManager.instance.StartChecking();" )
    public static ControllerConnectionManager instance;

    [Tooltip("Should the game start checking for connection changes automatically?")]
    public bool StartCheckingAutomatically = true;

    [Tooltip("How often (in seconds) this script checks for changes in the list of conencted control devices")]
    public int UpdateFrequencyInSeconds = 1;

    private bool firstconnectiondone = false;
    private string[] names = new string[20];
    private string[] oldnames = new string[20];

    public GameManager GM;
    //public GameObject PauseScreen;

    public void Awake()
    {
 
      //  DontDestroyOnLoad(this);
    }

    private void Start()
    {
        #region Create a singleton reference so this script can be references easily
        if (instance == null)
            instance = this;
        else
        {
            Debug.Log("Duplicate ControllerConnectionManager detected! Destroying " + gameObject + "'s component.");
            Destroy(this);
        }

        DontDestroyOnLoad(this);
        #endregion

        if (StartCheckingAutomatically)
        {
            // Start the coroutine so that it runs every UpdateFrequencyInSeconds seconds
            StartCoroutine("CheckConnections");
        }
    }

    private IEnumerator CheckConnections()
    {
        while (true)
        {
            // Get a string of all conencted control devices (This will likely include some empty entries but that is unavoidable)
            names = Input.GetJoystickNames();

            // Was a change detected this iteration?
            bool changed = false;
            // A list of names indexes that were changed since the last iteration
            List<int> index = new List<int>();

            for (int i = 0; i < names.Length; i++)
            {
                // If a change in the list was detected, set changed to true and add its index to the index List
                if (oldnames[i] != names[i])
                {
                    changed = true;
                    index.Add(i);
                }
            }
            
            // If a change occured, run the ControllerChangeDetected function to do something depending on the change type
            if (changed)
                ControllerChangeDetected(index);

            oldnames = names;

            // Wait for UpdateFrequencyInSeconds seconds before running CheckConnections() again
            yield return new WaitForSeconds(UpdateFrequencyInSeconds);
        }
    }

    private void ControllerChangeDetected(List<int> indexes)
    {
        // For each names array value that was changed in the latest check, identify its type and do things depending on that
        foreach (int i in indexes)
        {
            // If the names[index] (a string) length is 0, it means it was a disconnection, otherwise it was a connect or reconnect
            if (names[i].Length > 0)
            {
                // If this was nor the first connection/iteration, then it is a reconnection
                if (!firstconnectiondone)
                {
                    #region Debug Messages to console
                    if (names[i].Length == 19)
                        Debug.Log("PS4 Controller connected!");
                    else if (names[i].Length == 33)
                        Debug.Log("Xbox Controller connected!");
                    else
                        Debug.Log("Controller connected!");
                    #endregion

                    // Place code specific to the first time controllers connect OR when the game initially launches here:
                    // ...
                }
                else
                {
                    #region Debug Messages to console
                    if (names[i].Length == 19)
                        Debug.Log("PS4 Controller reconnected!");
                    else if (names[i].Length == 33)
                        Debug.Log("Xbox Controller reconnected!");
                    else
                        Debug.Log("Controller reconnected!");
                    #endregion

                    //GM.pauseScreen.SetActive(false);
                    GM.paused = !GM.paused;
                    Cursor.visible = false;
                    // GM.pauseScreen.SetActive(false);
                    // Place code specific for reconnections of controllers here:
                    // ...

                }
            }
            else
            {
                #region Debug Messages to console
                if (oldnames[i].Length == 19)
                    Debug.LogWarning("PS4 Controller disconnected!");
                else if (oldnames[i].Length == 33)
                    Debug.LogWarning("Xbox Controller disconnected!");
                else
                    Debug.LogWarning("Controller disconnected!");
                #endregion
                GM.TogglePause();
                Cursor.visible = false;
                // Place code specific for disconnections here (E.g. Automatically pause the game):
                // ...

            }
        }
        // Differentiate between initial startup connections and re-connections
        firstconnectiondone = true;

        // Place code for any controller changes (regardless of connect or disconnect) here:
        // ...
    }




    // PUBLIC METHODS THAT CAN BE CALLED FROM OTHER SCRIPTS IF NEEDED DURING RUNTIME
    public void StartChecking()
    {
        StartCoroutine("CheckConnections");
    }

    public void StopChecking()
    {
        StopCoroutine("CheckConnections");
    }
}