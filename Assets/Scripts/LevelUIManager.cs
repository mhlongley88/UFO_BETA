using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelUIManager : MonoBehaviour
{
    private static LevelUIManager instance;

    public LifeManager p1LifeManager;
    public LifeManager p2LifeManager;
    public LifeManager p3LifeManager;
    public LifeManager p4LifeManager;

    public static LevelUIManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private LifeManager GetLifeManager(Player p)
    {
        switch(p)
        {
            case Player.One:
                return p1LifeManager;
            case Player.Two:
                return p2LifeManager;
            case Player.Three:
                return p3LifeManager;
            case Player.Four:
                return p4LifeManager;
            default:
                throw new ArgumentException("Invalid Player: " + Enum.GetName(typeof(Player), p));            
        }
    }

    public void ChangeLifeCount(Player p, int livesLeft)
    {
        GetLifeManager(p).ChangeLifeCount(livesLeft);
    }

    public void EnableUI(Player p)
    {
        GetLifeManager(p).gameObject.SetActive(true);
    }

}
