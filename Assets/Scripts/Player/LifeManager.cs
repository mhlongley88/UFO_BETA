using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public Image[] lifeIndicators;

    public Image characterHead;

    public void ChangeLifeCount(int lifeCount)
    {

        for (int i = 0; i < lifeIndicators.Length; i++)
        {
            if (i < lifeCount)
            {
                lifeIndicators[i].gameObject.SetActive(true);
            }
            else
            {
                lifeIndicators[i].gameObject.SetActive(false);
            }
        }
    }

    public void DisableAllElements()
    {
        for (int i = 0; i < lifeIndicators.Length; i++)
        {
            lifeIndicators[i].gameObject.SetActive(false);
        }

        characterHead.gameObject.SetActive(false);
    }
}
