using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lifeIndicators;



    public void ChangeLifeCount(int lifeCount)
    {

        for (int i = 0; i < lifeIndicators.Length; i++)
        {
            if (i < lifeCount)
            {
                lifeIndicators[i].SetActive(true);
            }
            else
            {
                lifeIndicators[i].SetActive(false);
            }
        }
    }
}
