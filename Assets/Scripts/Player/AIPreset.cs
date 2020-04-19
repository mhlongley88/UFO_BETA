using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIPreset", menuName = "UFO Bot/New AI Preset")]
public class AIPreset : ScriptableObject
{
    public bool abduct;
    public bool useDash;
    public bool useSpecials;
    public Vector2 shootRateMinMax = new Vector2(0.25f, 1.4f);
    public bool increaseHealth;
    public float increaseHealthRate = 0.6f;
    public float amountHealthToIncrease = 0.6f;

    public bool increaseDamage = false;
    public float increaseDamageRate = 1.2f;
    public float amountDamageToIncrease = 1;
}
