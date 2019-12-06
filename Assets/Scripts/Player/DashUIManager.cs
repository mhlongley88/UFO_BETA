using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUIManager : MonoBehaviour
{
    public Image fill;

    private void OnEnable()
    {
        fill.fillAmount = 1.0f;
    }

    public void SetPercentage(float percentage)
    {
        float r = percentage / 100.0f;
        fill.fillAmount = r;
    }
}
