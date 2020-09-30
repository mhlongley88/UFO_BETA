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
    public bool isOptionsMenu, isSubOptionsMenu, closeOptionsMenu;
    public SimpleMenuSelection BackFromOptionsMenu;
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
            if(isOptionsMenu && currentFocused && !currentFocused.isSubOptionsMenu)
                BackFromOptionsMenu = currentFocused;
            previousFocused = currentFocused;
            
            if (previousFocused != null && previousFocused.gameObject.activeInHierarchy) previousFocused.gameObject.SetActive(false);
             currentFocused = this;
            //Debug.Log(currentFocused.name);
        }
    }

    private void OnDisable()
    {
        if (currentFocused == this)
        {

            if (isOptionsMenu && closeOptionsMenu && BackFromOptionsMenu && MainMenuUIManager.Instance.currentMenu == MainMenuUIManager.Menu.Splash)
            {
                closeOptionsMenu = true;
                BackFromOptionsMenu.gameObject.SetActive(true);
                currentFocused = BackFromOptionsMenu;
                Debug.Log(currentFocused.name);
            }
            else
            {
                currentFocused = previousFocused;
                //Debug.Log(currentFocused.name + "?");
            }
            
            
            if(currentFocused == null)
            {
               
            }
        }
    }

    public void CloseOptionsMenu()
    {
        closeOptionsMenu = true;
        GameManager.Instance.paused = false;
        MainMenuUIManager.Instance.menuOpen = false;
        MainMenuUIManager.Instance.OptionsCanvas.SetActive(false);
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

                if (rewirePlayer.GetButtonDown(submitBtnName) && items[index].interactable)
                    items[index].onClick.Invoke();


                if (goToMenuWhenBack != null && rewirePlayer.GetButtonDown("Back"))
                {
                    if (isOptionsMenu)
                    {
                        CloseOptionsMenu();
                    }
                    else
                    {
                        gameObject.SetActive(false);
                        goToMenuWhenBack.gameObject.SetActive(true);
                        FocusMenu(goToMenuWhenBack);
                    }
                    
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
        if (items.Length > 0)
            items[index].transform.localScale = Vector3.Lerp(items[index].transform.localScale, originalScales[index] + Vector3.one * 0.2f, Time.deltaTime * 4.0f);
    }

    public void FocusMenu(SimpleMenuSelection menu)
    {
        if (menu.gameObject.activeInHierarchy)
        {
            previousFocused = this;
            currentFocused = menu;
            //Debug.Log(currentFocused.name + "?");
        }
    }
}
