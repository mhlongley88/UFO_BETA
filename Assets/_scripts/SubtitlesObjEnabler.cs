using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitlesObjEnabler : MonoBehaviour
{
    public GameObject SubtitlesTextObj;
    // Start is called before the first frame update
    void OnEnable()
    {
        SubtitlesTextObj.SetActive(GameManager.Instance.selectedLanguage != "English");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
