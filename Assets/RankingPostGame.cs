using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPostGame : MonoBehaviour
{
    public static RankingPostGame instance;

    public Transform[] ranks;
    public Transform firstPlaceTransform;
    int place;

    private void Awake()
    {
        instance = this;
        place = ranks.Length - 1;
    }

    public void SubmitPlayer(int rankNumber, Transform t)
    {
        if (rankNumber >= 0 && rankNumber < ranks.Length)
        {
            Transform rank = ranks[rankNumber];
            Debug.Log(rankNumber);
            t.gameObject.isStatic = false;

            Transform rankParent = rank.parent;

        
            t.SetParent(rankParent);
            t.position = rank.position;

            t.rotation = rank.rotation;
            t.localScale = rank.localScale;

            if (rankNumber == 0) 
                t.SetParent(firstPlaceTransform);

            rank.gameObject.SetActive(false);
            t.gameObject.SetActive(true);
        }
    }
}
