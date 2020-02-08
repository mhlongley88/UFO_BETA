using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using DG.Tweening;

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

    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(RunTutorial());
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

        yield return new WaitForSeconds(3);

        // LThumbSticks
        LThumbstick.SetActive(true);
        while(!canProgress)
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));
                if (playerInput.GetAxis("Horizontal") != 0.0f || playerInput.GetAxis("Vertical") != 0.0f)
                    canProgress = true;
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;

        //RThumbsticks
        LThumbstick.SetActive(false);
        RThumbstick.SetActive(true);
        while (!canProgress)
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));
                if (playerInput.GetAxis("AimHorizontal") != 0.0f || playerInput.GetAxis("AimVertical") != 0.0f)
                    canProgress = true;
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;

        // Shoot Button
        RThumbstick.SetActive(false);
        Shoot.SetActive(true);
        while (!canProgress)
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));
                if (playerInput.GetButtonDown("Shoot"))
                    canProgress = true;
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;

        //Dash Button
        Shoot.SetActive(false);
        Dash.SetActive(true);
        while (!canProgress)
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));
                if (playerInput.GetButtonDown("Dash"))
                    canProgress = true;
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;

        //Abduct button
        Dash.SetActive(false);
        Abduct.SetActive(true);
        city.SetActive(true);
        while (!canProgress)
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));
                if (playerInput.GetButtonDown("Abduct"))
                    canProgress = true;
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;

        // Special Weapons
        Abduct.SetActive(false);
        specialPt1.SetActive(true);
        specialPt2.SetActive(true);
        rotNull.transform.Rotate(0, 180, 0);
        while (!canProgress)
        {
            foreach(var player in PlayerController.playerControllerByGameObject)
            {
                if(player.Value.superWeaponActive) // This is true only when the super weapon is ready and is being used
                {
                    canProgress = true;
                }
            }

            //for (int i = 0; i < activePlayers.Count; i++)
            //{
            //    var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));
            //    if (playerInput.GetButton("ActivateSuperWeapon1") && playerInput.GetButton("ActivateSuperWeapon2"))
            //        canProgress = true;
            //}

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        canProgress = false;

        // Shoot button
        specialPt1.SetActive(false);
        specialPt2.SetActive(false);
        rotNull.transform.Rotate(0, -180, 0);
        Shoot.SetActive(true);
        while (!canProgress)
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                var playerInput = ReInput.players.GetPlayer(GetPlayerIndex(activePlayers[i]));
                if (playerInput.GetButtonDown("Shoot"))
                    canProgress = true;
            }

            activePlayers = GameManager.Instance.GetActivePlayers();
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);

        Shoot.SetActive(false);
        yield return new WaitForSeconds(1);

        Sequence seq = DOTween.Sequence();
        seq.Append(TutorialController.transform.DOScale(0.0f, 1.0f));
        seq.AppendCallback(() => Destroy(TutorialController));
        
        TutorialManager.instance.canGoToMenu = true;
    }
}
