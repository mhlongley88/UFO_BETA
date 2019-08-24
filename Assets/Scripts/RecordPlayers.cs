using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EZReplayManager.get.mark4Recording(gameObject, gameObject.name, ChildIdentificationMode.IDENTIFY_BY_NAME);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
