using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> enemies = new List<Enemy>();
    private void Awake()
    {
        if (!enemies.Contains(this))
            enemies.Add(this);
    }
    private void OnEnable()
    {
        if (!enemies.Contains(this))
            enemies.Add(this);
    }

    private void OnDestroy()
    {
        if (enemies.Contains(this))
            enemies.Remove(this);
    }

    private void OnDisable()
    {
        if (enemies.Contains(this))
            enemies.Remove(this);
    }
}
