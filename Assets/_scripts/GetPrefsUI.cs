using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GetPrefsUI : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public string key;
    public PrefType type;
    UnityAction setValue;
    public enum PrefType
    {
        _bool, _int, _string, _float
    }
    // Start is called before the first frame update
    void Start()
    {
        AddActionType();
        UpdateUI();
    }

    public void UpdateUI()
    {
        setValue.Invoke();
    }

    // Update is called once per frame
    void UpdateUIBool()
    {
        textUI.text = UserPrefs.instance.GetBool(key).ToString();
    }

    void UpdateUIString()
    {
        textUI.text = UserPrefs.instance.GetString(key).ToString();
    }

    void UpdateUIfloat()
    {
        textUI.text = UserPrefs.instance.GetFloat(key).ToString();
    }

    void UpdateUIInt()
    {
        textUI.text = UserPrefs.instance.GetInt(key).ToString();
    }

    void AddActionType()
    {
        switch (type)
        {
            case PrefType._bool:
                setValue += UpdateUIBool;
                break;
            case PrefType._float:
                setValue += UpdateUIfloat;
                break;
            case PrefType._int:
                setValue += UpdateUIInt;
                break;
            case PrefType._string:
                setValue += UpdateUIString;
                break;
        }
    }
}
