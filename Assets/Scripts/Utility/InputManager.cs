using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RotaryHeart.Lib.SerializableDictionary;

public class InputManager : MonoBehaviour
{
    public enum ButtonEnum
    {
        Submit,
        Back,
        Fire,
        Beam,
        Dash
    }

    public enum AxisEnum
    {
        
        LeftStickHorizontal,
        LeftStickVertical,
        RightStickHorizontal,
        RightStickVertical,
        ActivateSuperWeapon1,
        ActivateSuperWeapon2
    }

    [Serializable]
    public class ButtonStrings
    {
        [SerializeField]
        public string[] buttonNames;

        public int Length 
        {
            get{
                return buttonNames.Length;
            }
        }
    }

    [Serializable]
    public class ButtonDictionary : SerializableDictionaryBase<ButtonEnum, ButtonStrings> {}
 
    public ButtonDictionary buttonDictionary = new ButtonDictionary();

    [Serializable]
    public class ButtonDictionaryKB : SerializableDictionaryBase<ButtonEnum, ButtonStrings> { }

    public ButtonDictionary buttonDictionaryKB = new ButtonDictionary();

    [Serializable]
    public class AxisDictionary : SerializableDictionaryBase<AxisEnum, string> {}

    public AxisDictionary axisDictionary = new AxisDictionary();

    [Serializable]
    public class AxisDictionaryKB : SerializableDictionaryBase<AxisEnum, string> { }

    public AxisDictionary axisDictionaryKB = new AxisDictionary();


    [SerializeField]
    private string player1InputPrefix;
    [SerializeField]
    private string player2InputPrefix;
    [SerializeField]
    private string player3InputPrefix;
    [SerializeField]
    private string player4InputPrefix;
    
    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private string GetPlayerPrefix(Player player)
    {
        switch(player)
        {
            case Player.One:
                return player1InputPrefix;
            case Player.Two:
                return player2InputPrefix;
            case Player.Three:
                return player3InputPrefix;
            case Player.Four:
                return player4InputPrefix;
            default:
                throw new ArgumentException("invalid player");
        }
    }

    public bool GetButtonDown(ButtonEnum input, Player player)
    {
        bool result = false;
        for(int i = 0; i < buttonDictionary[input].Length; i++)
        {
            result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        }
        return result;
    }

    public bool GetButtonDownKB(ButtonEnum input, Player player)
    {
        bool result = false;
        for (int i = 0; i < buttonDictionary[input].Length; i++)
        {
            result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        }
        return result;
    }

    public bool GetButton(ButtonEnum input, Player player)
    {
        bool result = false;
        for(int i = 0; i < buttonDictionary[input].Length; i++)
        {
            result = result || Input.GetButton(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        }
        return result;
    }

    
    public bool GetButtonUp(ButtonEnum input, Player player)
    {
        bool result = false;
        for(int i = 0; i < buttonDictionary[input].Length; i++)
        {
            result = result || Input.GetButtonUp(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        }
        return result;
    }

    public float GetAxis(AxisEnum input, Player player)
    {
        return Input.GetAxis(GetPlayerPrefix(player) + axisDictionary[input]);
    }

}
