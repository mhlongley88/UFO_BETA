using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostTutorialInstructionsPlay : MonoBehaviour
{
    public void LetPlayersExitGame()
    {
        TutorialManager.instance.canGoToMenu = true;
    }
}
