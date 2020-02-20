using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class SimpleMenuSelection : MonoBehaviour
{
    public Button[] items;

    Vector3[] originalScales;

    int index = 0;

    float changeRate = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        originalScales = new Vector3[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            originalScales[i] = items[i].transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            var rewirePlayer = ReInput.players.GetPlayer(i);

            if (changeRate < Time.time)
            {
                if (rewirePlayer.GetAxis("Vertical") > 0.1f)
                {
                    Debug.Log("HJEY!");
                    index--;
                    changeRate = Time.time + 0.35f;
                }
                else if (rewirePlayer.GetAxis("Vertical") < -0.1f)
                {
                    index++;
                    changeRate = Time.time + 0.35f;
                }

            }

            if (rewirePlayer.GetButtonDown("Submit"))
                items[index].onClick.Invoke();
        }

        if (index < 0) index = items.Length - 1;
        else if (index >= items.Length) index = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if(i != index)
                items[i].transform.localScale = Vector3.Lerp(items[i].transform.localScale, originalScales[i], Time.deltaTime * 4.0f);
        }

        items[index].transform.localScale = Vector3.Lerp(items[index].transform.localScale, originalScales[index] + Vector3.one * 0.2f, Time.deltaTime * 4.0f);
    }
}
