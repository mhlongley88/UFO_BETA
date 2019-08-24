using UnityEngine;
using System.Collections;

//source: http://twiik.net/articles/realtime-reflections-in-unity-5
public class RealtimeReflection : MonoBehaviour {

    ReflectionProbe probe;

    void Awake() {
        probe = GetComponent<ReflectionProbe>();

        if (PlayerPrefs.GetInt("EnableReflection", 1) == 0) {
            probe.enabled = false;
        }
    }

    void Update() {
        probe.transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y * -1,
            Camera.main.transform.position.z
        );

        if (probe.enabled) {
            probe.RenderProbe();
        }
    }
}