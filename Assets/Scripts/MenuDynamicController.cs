using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.Events;

public class MenuDynamicController : MonoBehaviour
{
    public enum SelectableType
    {
        TOGGLE,
        BUTTON,
        SLIDER,
        DROPDOWN
    }

    [System.Serializable]
    public class MenuItem
    {
        public SelectableType type;
        public Selectable selectable;
        public bool forceSliderInteger = false;

        [HideInInspector]
        public Transform transform;
        [HideInInspector]
        public Vector3 originalScale;
    }

    public float scalingSpeed = 4.0f;
    public float increaseOnScale = 0.2f;
    public MenuItem[] items;

    int index = 0;
    int dropDownIndex = 0;
    float changeRate = 0.0f;

    float horizontalRateSlider = 0.0f;

    public UnityEvent onEnable = new UnityEvent();
    public UnityEvent onDisable = new UnityEvent();
    bool isShowingDropdownOptions;
    // Start is called before the first frame update
    void Start()
    {
        if (items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                MenuItem item = items[i];

                item.transform = item.selectable.gameObject.transform;
                item.originalScale = item.transform.localScale;
            }
        }
    }

    private void OnEnable()
    {
        onEnable.Invoke();
        index = 0;
    }

    private void OnDisable()
    {
        onDisable.Invoke();
    }
    bool verticalMoveDetected = false;
    // Update is called once per frame
    void Update()
    {
        if (!PlayVideoWhileIdle.playingIdleVideo)
        {
            for (int i = 0; i < 4; i++)
            {
                var rewirePlayer = ReInput.players.GetPlayer(i);

                if (items != null && items.Length > 0)
                {
                    if (!isShowingDropdownOptions && changeRate < Time.time)
                    {
                        if (rewirePlayer.GetAxis("Vertical") > 0.1f)
                        {
                            //Debug.Log("HJEY!");
                            if (items[index].type == SelectableType.DROPDOWN)
                            {
                                var dropDown = (TMPro.TMP_Dropdown)items[index].selectable;
                                if (dropDown.IsExpanded)
                                {
                                    ScrollRect optionsScrollRect = dropDown.GetComponentInChildren<ScrollRect>();
                                    int count = optionsScrollRect.content.GetComponentsInChildren<Toggle>().Length;
                                    //Debug.Log((dropDownIndex - 1) % count);
                                    dropDownIndex--;
                                    dropDownIndex = dropDownIndex < 0 ? count - 1 : dropDownIndex;
                                }
                                else
                                {
                                    index--;
                                }

                            }
                            else
                            {
                                index--;
                            }

                            changeRate = Time.time + 0.35f;
                            verticalMoveDetected = true;
                        }
                        else if (rewirePlayer.GetAxis("Vertical") < -0.1f)
                        {
                            if (items[index].type == SelectableType.DROPDOWN)
                            {
                                var dropDown = (TMPro.TMP_Dropdown)items[index].selectable;
                                if (dropDown.IsExpanded)
                                {
                                    ScrollRect optionsScrollRect = dropDown.GetComponentInChildren<ScrollRect>();
                                    int count = optionsScrollRect.content.GetComponentsInChildren<Toggle>().Length;
                                    dropDownIndex = (dropDownIndex + 1) % count;
                                }
                                else
                                {
                                    index++;
                                }

                            }
                            else
                            {
                                index++;
                            }
                            changeRate = Time.time + 0.35f;
                            verticalMoveDetected = true;
                        }

                        if (index < 0) index = items.Length - 1;
                        else if (index >= items.Length) index = 0;
                    }

                    //items[index].onClick.Invoke();
                    {
                        MenuItem highightedItemLocal = items[index];
                        switch (highightedItemLocal.type)
                        {
                            case SelectableType.BUTTON:
                                {
                                    if (rewirePlayer.GetButtonDown("Submit"))
                                    {
                                        var button = (Button)highightedItemLocal.selectable;
                                        button.onClick.Invoke();
                                    }
                                }
                                break;
                            case SelectableType.SLIDER:
                                {
                                    var axis = rewirePlayer.GetAxis("Horizontal");
                                    var slider = (Slider)highightedItemLocal.selectable;

                                    if (!highightedItemLocal.forceSliderInteger)
                                        slider.value += axis * Time.deltaTime * 1.5f;
                                    else
                                    {
                                        if (horizontalRateSlider < Time.time && axis != 0.0f)
                                        {
                                            slider.value += axis > 0 ? 1 : (axis < 0 ? -1 : 0);
                                            horizontalRateSlider = Time.time + 0.3f;
                                        }
                                    }
                                }
                                break;
                            case SelectableType.TOGGLE:
                                if (rewirePlayer.GetButtonDown("Submit"))
                                {
                                    var toggle = (Toggle)highightedItemLocal.selectable;
                                    toggle.isOn = !toggle.isOn;
                                }
                                break;
                            case SelectableType.DROPDOWN:
                                var dropDown = (TMPro.TMP_Dropdown)highightedItemLocal.selectable;

                                if (rewirePlayer.GetButtonDown("Submit"))
                                {

                                    if (!dropDown.IsExpanded)
                                    {
                                        dropDownIndex = dropDown.value;
                                        dropDown.Show();
                                    }
                                    else
                                    {
                                        ScrollRect optionsScrollRect = dropDown.GetComponentInChildren<ScrollRect>();

                                        Toggle[] toggles = optionsScrollRect.content.GetComponentsInChildren<Toggle>();
                                        //toggles[dropDownIndex].Select();
                                        dropDown.value = dropDownIndex;
                                        dropDown.SetValueWithoutNotify(dropDownIndex);
                                        Debug.Log(dropDownIndex + " is selected");
                                        //dropDown.Hide();
                                    }
                                }

                                if (dropDown.IsExpanded)
                                {
                                    ScrollRect optionsScrollRect = dropDown.GetComponentInChildren<ScrollRect>();

                                    Toggle[] toggles = optionsScrollRect.content.GetComponentsInChildren<Toggle>();

                                    for (int j = 0; j < toggles.Length; j++)
                                    {

                                        if (j != dropDownIndex)
                                        {
                                            //Debug.Log(j + "---" + dropDownIndex);
                                            LanguageItemStats stats = toggles[j].GetComponent<LanguageItemStats>();
                                            //stats.Highlighted.gameObject.SetActive(false);
                                            stats.transform.localScale = Vector3.Lerp(stats.transform.localScale, new Vector3(0.9f, 0.9f, 0.9f), Time.deltaTime * scalingSpeed);
                                        }
                                    }
                                    //toggles[dropDownIndex].GetComponent<LanguageItemStats>().Highlighted.gameObject.SetActive(true);
                                    //float div = ((float)(dropDownIndex + 1) / (float)(toggles.Length));
                                    //if(dropDownIndex%2==0)
                                    toggles[dropDownIndex].transform.localScale = Vector3.Lerp(toggles[dropDownIndex].transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * scalingSpeed);
                                    if (verticalMoveDetected)
                                    {
                                        verticalMoveDetected = false;
                                        optionsScrollRect.verticalNormalizedPosition = (1 - ((float)(dropDownIndex) / (float)(toggles.Length - 1)));
                                    }

                                }

                                break;
                        }

                    }
                }

                //if (goToMenuWhenBack != null && rewirePlayer.GetButtonDown("Back"))
                //{
                //gameObject.SetActive(false);
                //goToMenuWhenBack.gameObject.SetActive(true);
                //FocusMenu(goToMenuWhenBack);
                //}
            }
        }

        if (items != null && items.Length > 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                MenuItem item = items[i];
                if (i != index)
                    item.transform.localScale = Vector3.Lerp(item.transform.localScale, item.originalScale, Time.deltaTime * scalingSpeed);
            }

            MenuItem highightedItem = items[index];
            highightedItem.transform.localScale = Vector3.Lerp(highightedItem.transform.localScale, highightedItem.originalScale + Vector3.one * increaseOnScale, Time.deltaTime * scalingSpeed);
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                MenuItem item = items[i];

                if (item.selectable != null)
                {
                    var itemType = item.selectable.GetType();

                    if (itemType == typeof(Button)) item.type = SelectableType.BUTTON;
                    if (itemType == typeof(Slider)) item.type = SelectableType.SLIDER;
                    if (itemType == typeof(Toggle)) item.type = SelectableType.TOGGLE;
                    if (itemType == typeof(TMPro.TMP_Dropdown)) item.type = SelectableType.DROPDOWN;
                }
            }
        }
    }
#endif

}