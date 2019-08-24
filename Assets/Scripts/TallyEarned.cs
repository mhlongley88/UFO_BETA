using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyEarned : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Add()
    {
        if (GameManager.Instance.gameOver == true)
        {
            GamesCompletedTally.gamesCompleted += 1;
            Debug.Log("Games Completed:" + GamesCompletedTally.gamesCompleted);
        }
    }
}
