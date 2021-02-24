using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterSelectHandler : MonoBehaviour
{
    public Transform[] UnlockedCharacterContainers;
    public Transform[] LockedCharacterContainers;
    public Vector3[] CharactersLocalPositions;
    public Transform CharactersContainer;
    public SelectCharacterUI[] AvailableCharacters;
    public TextMeshProUGUI[] UnlockedUFOText;
    // Start is called before the first frame update
    void OnEnable()
    {
        DisplayUnlockedCharacters();
    }


    void Refresh()
    {
        foreach (SelectCharacterUI character in AvailableCharacters)
        {
            character.gameObject.transform.SetParent(CharactersContainer);
            character.gameObject.GetComponent<Button>().interactable = false;
            character.gameObject.SetActive(false);
        }
    }

    void DisplayUnlockedCharacters()
    {
        Refresh();

        int unlockedCount = 0, currentChildCount = 0;
        int unlockedContainerId = 0, lockedContainerId = 0;

        UnlockedCharacterContainers[unlockedContainerId].parent.gameObject.SetActive(true);
        LockedCharacterContainers[lockedContainerId].parent.gameObject.SetActive(true);

        for (int i=0;i<AvailableCharacters.Length; i++)
        {
            UFOAttributes attr = GameManager.Instance.GetUfoAttribute(i);
            if (attr != null && attr.isUnlocked)
            {
                if(UnlockedCharacterContainers[unlockedContainerId].childCount >= 3)
                {
                    unlockedContainerId++;
                    UnlockedCharacterContainers[unlockedContainerId].parent.gameObject.SetActive(true);
                }
                unlockedCount++;
                
                AvailableCharacters[i].transform.SetParent(UnlockedCharacterContainers[unlockedContainerId]);
                AvailableCharacters[i].SetSprite(false);
                AvailableCharacters[i].gameObject.GetComponent<Button>().interactable = true;
                currentChildCount = UnlockedCharacterContainers[unlockedContainerId].childCount;
            }
            else if(attr != null)
            {
                if (LockedCharacterContainers[lockedContainerId].childCount >= 3)
                {
                    lockedContainerId++;
                    LockedCharacterContainers[lockedContainerId].parent.gameObject.SetActive(true);
                }

                AvailableCharacters[i].transform.SetParent(LockedCharacterContainers[lockedContainerId]);
                AvailableCharacters[i].SetSprite(true);
                currentChildCount = LockedCharacterContainers[lockedContainerId].childCount;
            }
            else
            {
                AvailableCharacters[i].gameObject.SetActive(false);
                continue;
            }
            AvailableCharacters[i].gameObject.SetActive(true);
            AvailableCharacters[i].GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);
            AvailableCharacters[i].GetComponent<RectTransform>().localPosition = CharactersLocalPositions[currentChildCount - 1];
            

        }
        foreach(TextMeshProUGUI txt in UnlockedUFOText)
        {
            txt.text = unlockedCount.ToString() + "/" + AvailableCharacters.Length.ToString();
        }
    }

    public void SelectCharacter_OnClick(int index)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
