using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

// attach to UI Text component (with the full text already there)

public class TypewriterText : MonoBehaviour 
{
    
	TextMeshPro txt;
    string story;
    public float typeInSeconds;

void Awake()
{
    
    txt = this.gameObject.GetComponent<TextMeshPro>();
        story = txt.text.ToString();
    txt.text = "Welcome Back! To the UFO Network..";

    // TODO: add optional delay when to start
    StartCoroutine("PlayText");
}

IEnumerator PlayText()
{
    foreach (char c in story)
    {
        txt.text += c;
        yield return new WaitForSeconds(typeInSeconds);
    }
}

}