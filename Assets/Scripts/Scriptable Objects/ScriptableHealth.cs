using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthSettings", menuName = "UFO/HealthSettings", order = 0)]
public class ScriptableHealth : ScriptableObject
{
    // Start is called before the first frame update
    public float startingHealth = 3f;
    public float maxHealth = 5f;
    public float minHealth = 0f;


    /// <summary>
    /// Time it takes to refill from 0 to max health;
    /// </summary>
    public float refillDuration = 8f;

    /// <summary>
    /// Amount of time after taking damage before health starts to refill
    /// </summary>
    public float refillDelay = 3f;
}
