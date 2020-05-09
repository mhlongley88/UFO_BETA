using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMatchCutsceneRef : MonoBehaviour
{
    public static DoubleMatchCutsceneRef instance;
    public disableMe disableMe;
    public GameObject cutscene;

    private void Awake() {
        instance = this;
    }
}
