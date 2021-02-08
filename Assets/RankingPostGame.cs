using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPostGame : MonoBehaviour
{
    public static RankingPostGame instance;

    public Transform[] ranks;
    public Transform firstPlaceTransform;
    public GameObject[] ranksContainers;
    int place;

    List<GameObject> instantiatedObjects = new List<GameObject>();

    private void Awake()
    {
        instance = this;
        place = ranks.Length - 1;

        for (int i = 0; i < ranksContainers.Length; i++)
        {
            ranksContainers[i].SetActive(false);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < instantiatedObjects.Count; i++)
            Destroy(instantiatedObjects[i]);

        instantiatedObjects.Clear();
    }

    private void OnDisable()
    {
        for (int i = 0; i < instantiatedObjects.Count; i++)
            Destroy(instantiatedObjects[i]);

        instantiatedObjects.Clear();
    }

    public void SubmitPlayer(int rankNumber, GameObject playerModel)
    {
        if (rankNumber >= 0 && rankNumber < ranks.Length)
        {
            GameObject obj = Instantiate(playerModel);
            instantiatedObjects.Add(obj);

            Transform t = obj.transform;

            Transform rank = ranks[rankNumber];
            ranksContainers[rankNumber].SetActive(true);

            //Debug.Log(rankNumber);

            t.gameObject.isStatic = false;

            Transform rankParent = rank.parent;

        
            t.SetParent(rankParent);
            t.position = rank.position;

            t.rotation = rank.rotation;
            t.localScale = rank.localScale;

            if (rankNumber == 0)
            {
                t.SetParent(firstPlaceTransform);
                TouchGameUI.instance.LevelScreenControls.SetActive(false);
                TouchGameUI.instance.ResultScreenControls.SetActive(true);
            }
                

            rank.gameObject.SetActive(false);
            t.gameObject.SetActive(true);
        }
    }
}
