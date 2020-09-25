using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FontSwap : MonoBehaviour
{
    TextMeshProUGUI myText;

    // Start is called before the first frame update
    void Awake()
    {
        //myText = this.GetComponent<TextMeshProUGUI>();
        //MainMenuUIManager.Instance.UnusualTexts.Add(this);
    }

    public void swapFont(TMP_FontAsset newFont)
    {
        myText.font = newFont;
    }
}
