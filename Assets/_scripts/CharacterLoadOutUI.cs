using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterLoadOutUI : MonoBehaviour
{
    public GameObject selectedCharacter_LoadOut;
    public Image DamageFG, RateOfFireFG, AccuracyFG;
    public Transform characterModelContainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void RemoveAllChildren()
    {
        int count = characterModelContainer.childCount;
        for(int i = 0; i < count; i++)
        {
            Destroy(characterModelContainer.transform.GetChild(i).gameObject);
        }
    }

    public void DisplayCharacterInfo_LoadOutScreen()
    {
        RemoveAllChildren();
        int selectedCharacterIndex = GameManager.Instance.LocalPlayerId;
        Destroy(selectedCharacter_LoadOut);
        selectedCharacter_LoadOut = Instantiate(GameManager.Instance.Characters[selectedCharacterIndex].characterModel, Vector3.zero, Quaternion.identity, characterModelContainer);
        selectedCharacter_LoadOut.transform.localPosition = Vector3.zero;
        selectedCharacter_LoadOut.transform.localScale = Vector3.one;
        selectedCharacter_LoadOut.transform.localEulerAngles = Vector3.zero;

        

        var info = selectedCharacter_LoadOut.GetComponent<CharacterLevelSelectInfo>();
        info.UpdateValues(UserPrefs.instance.GetUFOProps().ufoData.Find(item => item.ufoIndex == selectedCharacterIndex));
        DamageFG.fillAmount = info.Damage;
        RateOfFireFG.fillAmount = info.RateOfFire;
        AccuracyFG.fillAmount = info.Accuracy;
        //MainMenuUIManager.Instance.HoldCharacterChoiceTemporarily(player, selectedCharacterIndex);
    }
}
