using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFinishEndScreen : MonoBehaviour
{
    public BossCongratulationsBeat beat;

    public void Process()
    {
        beat.Continue();
    }
}
