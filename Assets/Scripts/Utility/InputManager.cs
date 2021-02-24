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
    bool isConsole = false, isPC = false;
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
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            //Debug.Log("PC");
            isConsole = false;
            isPC = true;
        }
        else if (Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.XboxOne)
        {
            Debug.Log("Console");
            isPC = false;
            isConsole = true;
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

    public bool GetButtonDownCharacterSelection(ButtonEnum input, Player player)
    {
        bool result = false;


        if (!LobbyConnectionHandler.instance.IsMultiplayerMode && player == Player.Four)
        {
            if (player == Player.Four)
            {
                for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
                {
                    result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
                }
            }

        }
        else if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            // Debug.Log("wrong");
            for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
            {
                result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
            }
        }

        for (int i = 0; i < buttonDictionary[input].Length; i++)
            {
                result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
            }

        if (result)
        {
            Debug.Log(input.ToString() + "----" + player.ToString() + "===" + result.ToString());
        }

            

        //if (result)
        //{
        //    switch (player)
        //    {
        //        case Player.One:
        //            MainMenuUIManager.Instance.debugText.text = input.ToString() + "----" + player.ToString() + "===" + result.ToString();
        //            break;
        //        case Player.Two:
        //            MainMenuUIManager.Instance.debugText2.text = input.ToString() + "----" + player.ToString() + "===" + result.ToString();
        //            break;
        //        case Player.Three:
        //            MainMenuUIManager.Instance.debugText3.text = input.ToString() + "----" + player.ToString() + "===" + result.ToString();
        //            break;
        //        case Player.Four:
        //            MainMenuUIManager.Instance.debugText4.text = input.ToString() + "----" + player.ToString() + "===" + result.ToString();
        //            break;
        //        case Player.None:
        //            break;
        //    }
        //}
        //else
        //{
        //    MainMenuUIManager.Instance.debugText.text = "false";
        //    MainMenuUIManager.Instance.debugText2.text = "false";
        //    MainMenuUIManager.Instance.debugText3.text = "false";
        //    MainMenuUIManager.Instance.debugText4.text = "false";
        //}

        
       // Debug.Log(input.ToString() + "----" + player.ToString() + "===" + result.ToString());
       // MainMenuUIManager.Instance.debugText2.text = input.ToString() + "----" + player.ToString() + "===" + result.ToString();
        return result;
    }

    public bool GetButtonDown(ButtonEnum input, Player player)
    {
        //bool result = false;

        //{
        //    for (int i = 0; i < buttonDictionary[input].Length; i++)
        //    {
        //        result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        //    }

        //    {
        //        for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
        //        {
        //            result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
        //        }
        //    }
        //}

        bool result = false;


        if (!LobbyConnectionHandler.instance.IsMultiplayerMode && player == Player.Four)
        {
            if (player == Player.Four)
            {
                for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
                {
                    result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
                }
            }

        }
        else if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            // Debug.Log("wrong");
            for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
            {
                result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
            }
        }

        for (int i = 0; i < buttonDictionary[input].Length; i++)
        {
            result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        }

        if (result)
        {
            Debug.Log(input.ToString() + "----" + player.ToString() + "===" + result.ToString());
        }


        return result;
    }

    public bool GetButtonDownKB(ButtonEnum input, Player player)
    {
        bool result = false;
        for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
        {
            result = result || Input.GetButtonDown(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
        }
        return result;
    }

    public bool GetButton(ButtonEnum input, Player player)
    {
        //bool result = false;

        //{
        //    for (int i = 0; i < buttonDictionary[input].Length; i++)
        //    {
        //        result = result || Input.GetButton(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        //    }

        //    {
        //        for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
        //        {
        //            result = result || Input.GetButton(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
        //        }
        //    }

        //}

        bool result = false;


        if (!LobbyConnectionHandler.instance.IsMultiplayerMode && player == Player.Four)
        {
            if (player == Player.Four)
            {
                for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
                {
                    result = result || Input.GetButton(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
                }
            }

        }
        else if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            // Debug.Log("wrong");
            for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
            {
                result = result || Input.GetButton(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
            }
        }

        for (int i = 0; i < buttonDictionary[input].Length; i++)
        {
            result = result || Input.GetButton(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        }

        

        return result;
    }

    
    public bool GetButtonUp(ButtonEnum input, Player player)
    {
       // bool result = false;


        // for (int i = 0; i < buttonDictionary[input].Length; i++)
        // {
        //     result = result || Input.GetButtonUp(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        // }
        //// if (player == Player.Four)
        // {
        //     for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
        //     {
        //         result = result || Input.GetButtonUp(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
        //     }
        // }

        bool result = false;


        if (!LobbyConnectionHandler.instance.IsMultiplayerMode && player == Player.Four)
        {
            if (player == Player.Four)
            {
                for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
                {
                    result = result || Input.GetButtonUp(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
                }
            }

        }
        else if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            // Debug.Log("wrong");
            for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
            {
                result = result || Input.GetButtonUp(GetPlayerPrefix(player) + buttonDictionaryKB[input].buttonNames[i]);
            }
        }

        for (int i = 0; i < buttonDictionary[input].Length; i++)
        {
            result = result || Input.GetButtonUp(GetPlayerPrefix(player) + buttonDictionary[input].buttonNames[i]);
        }



        return result;
    }

    public float GetAxis(AxisEnum input, Player player)
    {
        // if (isConsole)
        //{
        //    return Input.GetAxis(GetPlayerPrefix(player) + axisDictionary[input]);
        //}
        //else
        //{
        //    return Input.GetAxis(/*GetPlayerPrefix(player) + */axisDictionaryKB[input]);
        //}
        //return Input.GetAxis(GetPlayerPrefix(player) + axisDictionary[input]);
        //float value = 0;
        //if(Input.GetAxis(GetPlayerPrefix(player) + axisDictionary[input]) != 0)
        //{
        //    value = Input.GetAxis(GetPlayerPrefix(player) + axisDictionary[input]);
        //   // Debug.Log(value + "1");
        //}
        //else if(Input.GetAxis(GetPlayerPrefix(player) + axisDictionaryKB[input]) != 0)
        //{
        //    value = Input.GetAxis(GetPlayerPrefix(player) + axisDictionaryKB[input]);
        //  //  Debug.Log(value + "2");
        //}
        return Input.GetAxis(GetPlayerPrefix(player) + axisDictionary[input]);
    }

    public float GetAxisKB(AxisEnum input, Player player)
    {
        float value = 0;
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode && player == Player.Four)
        {
            if (player == Player.Four)
            {
                value = Input.GetAxisRaw(GetPlayerPrefix(player) + axisDictionaryKB[input]);
            }

        }
        else if (LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            // Debug.Log("wrong");
            //for (int i = 0; i < buttonDictionaryKB[input].Length; i++)
            //{

            //}
            value = Input.GetAxisRaw(GetPlayerPrefix(player) + axisDictionaryKB[input]);
        }
        return value;//Input.GetAxis(GetPlayerPrefix(player) + axisDictionaryKB[input]);
    }
}
