using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockFromProgression : MonoBehaviour
{
    public static int lastSelected = -1;

    public bool allowUnlockFromProgression = false;
    public int unlocksWhatCharacter = -1;
    static string keyCharacterUnlock = "UFO_CharacterUnlockedProgression";
    //int unlockedState;
   // public int UnlockedState => unlockedState;

    // Start is called before the first frame update
    void Awake()
    {
        //unlockedState = PlayerPrefs.GetInt(keyLevelUnlock + unlocksWhatLevel, 0);
    }

    public void SetThisProgression()
    {
        if (allowUnlockFromProgression)
            lastSelected = unlocksWhatCharacter;
        else
            lastSelected = -1;
    }

    public static void UnlockCharacter()
    {
        //unlockedState = 1;
        if (lastSelected != -1)
        {
            if (IsUnlocked(lastSelected)) return;

            if (lastSelected >= 0 && lastSelected < GameManager.Instance.Characters.Length)
            {
                PlayerPrefs.SetInt(keyCharacterUnlock + lastSelected, 1);
                UnlockSystem.instance.SetUnlockCharacterFromProgression(GameManager.Instance.Characters[lastSelected].matchThreshold);
            }
            else
            {
                Debug.Log("CHARACTER UNLOCK PROGRESSION error: Tried to unlock a character with an index out of range of the Characters list on the Game Manager, index is " + lastSelected);
            }
        }
    }

    public static bool IsUnlocked(int characterIndex)
    {
        int r = PlayerPrefs.GetInt(keyCharacterUnlock + characterIndex, 0);
        return r == 1;
    }
}
