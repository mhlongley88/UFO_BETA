using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMenuUI : MonoBehaviour
{
    public GameObject selectedCharacter;
    public Transform MainHubCharacterContainer; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnSelectedCharacter(GameObject characterPrefab)
    {
        Destroy(selectedCharacter);
        selectedCharacter = Instantiate(characterPrefab, MainHubCharacterContainer);
        selectedCharacter.transform.localPosition = Vector3.zero;
        selectedCharacter.transform.localScale = Vector3.one;
    }
}
