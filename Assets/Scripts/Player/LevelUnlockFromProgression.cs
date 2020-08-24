using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlockFromProgression : MonoBehaviour
{
    public static int lastSelected = -1;

    public bool allowUnlockFromProgression = false;
    public int unlocksWhatLevel = -1;
    static string keyLevelUnlock = "UFOLevelUnlockedProgression";
    //int unlockedState;
   // public int UnlockedState => unlockedState;

    // Start is called before the first frame update
    void Awake()
    {
        //unlockedState = UserPrefs.instance.GetInt(keyLevelUnlock + unlocksWhatLevel, 0);
    }

    public void SetThisProgression()
    {
        if (allowUnlockFromProgression)
            lastSelected = unlocksWhatLevel;
        else
            lastSelected = -1;
    }

    public static void UnlockLevel()
    {
        //unlockedState = 1;
        if (lastSelected != -1)
        {
            if(IsUnlocked(lastSelected)) return;

            UserPrefs.instance.SetInt(keyLevelUnlock + lastSelected, 1);
            //UnlockNotification.instance?.SignalUnlockLevel();

            UnlockSystem.instance.SetUnlockLevelFromProgression();
        }
    }

    public static bool IsUnlocked(int levelNum)
    {
        int r = UserPrefs.instance.GetInt(keyLevelUnlock + levelNum, 0);
        return r == 1;
    }
}
