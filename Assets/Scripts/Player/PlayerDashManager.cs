using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDashManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Slider dashMeter;

    void Start()
    {
        dashMeter.minValue = 0.0f;
        dashMeter.maxValue = 1.0f;
    }

    void Update()
    {
        float amnt = LevelUIManager.Instance.GetDashUIManager(playerController.player).fill.fillAmount;

        dashMeter.value = Mathf.Lerp(dashMeter.value, amnt, Time.deltaTime * 7.0f);
    }
}
