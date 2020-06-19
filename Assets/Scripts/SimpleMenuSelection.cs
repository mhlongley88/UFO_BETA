using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class SimpleMenuSelection : MonoBehaviour
{
    public static SimpleMenuSelection currentFocused;
    static SimpleMenuSelection previousFocused;
    static List<SimpleMenuSelection> allMenus = new List<SimpleMenuSelection>();

    public Button[] items;
    public SimpleMenuSelection goToMenuWhenBack;

    public string submitBtnName = "Submit";

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

        allMenus.Add(this);
    }

    private void OnDestroy()
    {
        allMenus.Remove(this);
    }

    private void OnEnable()
    {
        if (currentFocused != this)
        {
            previousFocused = currentFocused;
            currentFocused = this;
        }
    }

    private void OnDisable()
    {
        if (currentFocused == this)
        {
            currentFocused = previousFocused;

            if(currentFocused == null)
            {
               
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFocused == this && !PlayVideoWhileIdle.playingIdleVideo)
        {
            for (int i = 0; i < 4; i++)
            {
                var rewirePlayer = ReInput.players.GetPlayer(i);

                if (changeRate < Time.time)
                {
                    if (rewirePlayer.GetAxis("Vertical") > 0.1f)
                    {
                        //Debug.Log("HJEY!");
                        index--;
                        changeRate = Time.time + 0.35f;
                    }
                    else if (rewirePlayer.GetAxis("Vertical") < -0.1f)
                    {
                        index++;
                        changeRate = Time.time + 0.35f;
                    }

                }

                if (rewirePlayer.GetButtonDown(submitBtnName))
                    items[index].onClick.Invoke();


                if (goToMenuWhenBack != null && rewirePlayer.GetButtonDown("Back"))
                {
                    gameObject.SetActive(false);
                    goToMenuWhenBack.gameObject.SetActive(true);
                    FocusMenu(goToMenuWhenBack);
                }
            }
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

    public void FocusMenu(SimpleMenuSelection menu)
    {
        if (menu.gameObject.activeInHierarchy)
        {
            previousFocused = this;
            currentFocused = menu;
        }
    }
}
