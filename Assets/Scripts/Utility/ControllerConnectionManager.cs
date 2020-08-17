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
using Rewired;
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
        // instance = this;

        // DontDestroyOnLoad(this);
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
        // Listen for controller connection events
        ReInput.ControllerConnectedEvent += OnControllerConnected;

        // Assign each Joystick to a Player initially
        AssignAllJoySticksToPlayers();
    }

    public void AssignAllJoySticksToPlayers()
    {

        foreach (Rewired.Player player in ReInput.players.AllPlayers)
        {
            player.controllers.ClearControllersOfType(ControllerType.Joystick);//.ClearAllControllers();
        }
        int playerIndex = 0;
        foreach (Joystick j in ReInput.controllers.Joysticks)
        {
            //if (ReInput.controllers.IsJoystickAssigned(j)) continue; // Joystick is already assigned

            // Assign Joystick to first Player that doesn't have any assigned
            //ReInput.players.AllPlayers[3].controllers.hasKeyboard = false;
            //ReInput.players.AllPlayers[3].controllers.hasMouse = false;
            //AssignJoystickToNextOpenPlayer(j);
            if (playerIndex < ReInput.players.allPlayerCount - 1)
            {
                Debug.Log("Controller Testing" + j + "" + playerIndex);
                ReInput.players.GetPlayer(playerIndex).controllers.AddController(j, true);
            }
            else
            {
                break;
            }
        }

        //ReInput.players.AllPlayers[3].controllers.hasKeyboard = true;
        //ReInput.players.AllPlayers[3].controllers.hasMouse = true;

    }
    // This will be called when a controller is connected
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType != ControllerType.Joystick) return; // skip if this isn't a Joystick

        // Assign Joystick to first Player that doesn't have any assigned

        AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));

    }

    void AssignJoystickToNextOpenPlayer(Joystick j)
    {
        if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            Rewired.Player rewirePlayer;
            rewirePlayer = ReInput.players.GetPlayer(3);
            rewirePlayer.controllers.AddController(j, true); Debug.Log("Adding Mul");
        }
        else
        {
            foreach (Rewired.Player p in ReInput.players.Players)
            {
                Debug.Log("Adding Off?????????????");
                if (p.controllers.joystickCount > 0) continue; // player already has a joystick
                p.controllers.AddController(j, true); // assign joystick to player
                Debug.Log("Adding Off");
                return;
            }
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
                if (oldnames[i] != null && oldnames[i] != names[i])
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
                if (oldnames != null && oldnames[i] != null)
                {
                    if (oldnames[i].Length == 19)
                        Debug.LogWarning("PS4 Controller disconnected!");
                    else if (oldnames[i].Length == 33)
                        Debug.LogWarning("Xbox Controller disconnected!");
                    else
                        Debug.LogWarning("Controller disconnected!");
                    #endregion
                    GM.TogglePause();
                    Cursor.visible = false;
                }
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