using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SkinStats : MonoBehaviour
{
    //public SkinType type;
    public int skinId;
    //public Vector3 pos, rot;
    public bool hasAdditionalRequirements;
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;
    // Start is called before the first frame update
    void Start()
    {
        //OnActivate.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class CharacterSkinUI
{
    public int characterId, skinId;
}