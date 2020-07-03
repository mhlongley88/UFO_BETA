using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using DG.Tweening;
using System.Linq;

public class TutorialAnimations : MonoBehaviour
{

    public GameObject TutorialController;
    public GameObject LThumbstick;
    public GameObject RThumbstick;
    public GameObject Shoot;
    public GameObject Dash;
    public GameObject Abduct;
    public GameObject city;
    public GameObject specialPt1;
    public GameObject specialPt2;
    public GameObject rotNull;

    Dictionary<Player, bool> performedAction = new Dictionary<Player, bool>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunTutorial());

        performedAction.Add(Player.One, false);
        performedAction.Add(Player.Two, false);
        performedAction.Add(Player.Three, false);
        performedAction.Add(Player.Four, false);
    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(RunTutorial());
        }
#endif
    }

    int GetPlayerIndex(Player p)
    {
        int rewirePlayerId = 0;
        switch (p)
        {
            case Player.One: rewirePlayerId = 0; break;
            case Player.Two: rewirePlayerId = 1; break;
            case Player.Three: rewirePlayerId = 2; break;
            case Player.Four: rewirePlayerId = 3; break;
        }

        return rewirePlayerId;
    }

    public IEnumerator RunTutorial()
    {
        var activePlayers = GameManager.Instance.GetActivePlayers();
        bool canProgress = false;
        foreach (var actionP in performedAction) performedAction[actionP.Key] = false;

        yield return new WaitForSeconds(4.0f);
        // LThumbSticks
        LThumbstick.SetActive(true);
        while(!canProgress)
        {
            canProgress = true;

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!performedAction[activePlayers[i]])
                {
                    canProgress = false;
                    var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));

                    if (playerInput.GetAxis("Horizontal") != 0.0f || playerInput.GetAxis("Vertical") != 0.0f)
                        performedAction[activePlayers[i]] = true;
                }
            }

            activePlayers = GameManager.Instance.GetActivePlayers();

            yield return null;
        }
        yield return new WaitForSeconds(0.20f);
        canProgress = false;
        for (int i = 0; i < performedAction.Count; i++)
        {
            var k = performedAction.ElementAt(i);
            performedAction[k.Key] = false;
        }

        //RThumbsticks
        LThumbstick.SetActive(false);
        RThumbstick.SetActive(true);
        while (!canProgress)
        {
            canProgress = true;

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!performedAction[activePlayers[i]])
                {
                    canProgress = false;
                    var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));

                   
                    if (playerInput.GetAxis("AimHorizontal") != 0.0f || playerInput.GetAxis("AimVertical") != 0.0f)
                        performedAction[activePlayers[i]] = true;
                    
                    // Also verifiy mouse movement for player4
                    if (activePlayers[i] == Player.Four || activePlayers[i] == Player.One)
                    {
                        if(Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f)
                            performedAction[activePlayers[i]] = true;
                    }
                }
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;
        for (int i = 0; i < performedAction.Count; i++)
        {
            var k = performedAction.ElementAt(i);
            performedAction[k.Key] = false;
        }

        // Shoot Button
        RThumbstick.SetActive(false);
        Shoot.SetActive(true);
        while (!canProgress)
        {
            canProgress = true;

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!performedAction[activePlayers[i]])
                {
                    canProgress = false;
                    var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));

                    if (playerInput.GetButtonDown("Shoot"))
                        performedAction[activePlayers[i]] = true;
                }
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.20f);
        canProgress = false;
        for (int i = 0; i < performedAction.Count; i++)
        {
            var k = performedAction.ElementAt(i);
            performedAction[k.Key] = false;
        }

        //Dash Button
        Shoot.SetActive(false);
        Dash.SetActive(true);
        while (!canProgress)
        {
            canProgress = true;

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!performedAction[activePlayers[i]])
                {
                    canProgress = false;
                    var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));

                    if (playerInput.GetButtonDown("Dash"))
                        performedAction[activePlayers[i]] = true;
                }
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.20f);
        canProgress = false;
        for (int i = 0; i < performedAction.Count; i++)
        {
            var k = performedAction.ElementAt(i);
            performedAction[k.Key] = false;
        }

        //Abduct button
        Dash.SetActive(false);
        Abduct.SetActive(true);
        city.SetActive(true);
        while (!canProgress)
        {
            canProgress = true;

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!performedAction[activePlayers[i]])
                {
                    canProgress = false;
                    var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));

                    if (playerInput.GetButtonDown("Abduct"))
                    {
                        canProgress = true;
                        break;
                        //performedAction[activePlayers[i]] = true;
                    }
                }
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;
        for (int i = 0; i < performedAction.Count; i++)
        {
            var k = performedAction.ElementAt(i);
            performedAction[k.Key] = false;
        }

        while (!canProgress)
        {
           // canProgress = true;

            foreach (var player in PlayerController.playerControllerByGameObject)
            {
                if (!performedAction[player.Value.player])
                {
                   // canProgress = false;

                    if (player.Value.IsSuperWeaponReady()) // Wait for one of the players to have special weapon ready
                    {
                        //performedAction[player.Value.player] = true;
                        canProgress = true;
                    }
                }
            }

            yield return null;
        }
        canProgress = false;
        for (int i = 0; i < performedAction.Count; i++)
        {
            var k = performedAction.ElementAt(i);
            performedAction[k.Key] = false;
        }

        // Special Weapons
        Abduct.SetActive(false);
        specialPt1.SetActive(true);
        specialPt2.SetActive(true);
        rotNull.transform.Rotate(0, 180, 0);
        while (!canProgress)
        {
           // canProgress = true;

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!performedAction[activePlayers[i]])
                {
                    //canProgress = false;
                    var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));

                    
                    if (playerInput.GetButton("ActivateSuperWeapon1") && playerInput.GetButton("ActivateSuperWeapon2"))
                    {
                        //performedAction[activePlayers[i]] = true;
                        canProgress = true;
                    }
                }
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;
        for (int i = 0; i < performedAction.Count; i++)
        {
            var k = performedAction.ElementAt(i);
            performedAction[k.Key] = false;
        }

        // Shoot button
        specialPt1.SetActive(false);
        specialPt2.SetActive(false);
        rotNull.transform.Rotate(0, -180, 0);
        Shoot.SetActive(true);
        while (!canProgress)
        {
           // canProgress = true;

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (!performedAction[activePlayers[i]])
                {
                    //canProgress = false;
                    var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));

                    if (playerInput.GetButtonDown("Shoot"))
                    {
                        //performedAction[activePlayers[i]] = true;
                        canProgress = true;
                        break;
                    }
                }
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);

        Shoot.SetActive(false);
        yield return new WaitForSeconds(0.2f);

        Sequence seq = DOTween.Sequence();
        seq.Append(TutorialController.transform.DOScale(0.0f, 1.0f));
        seq.AppendCallback(() => Destroy(TutorialController));
        
        TutorialManager.instance.canGoToMenu = true;
    }
}
