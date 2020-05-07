using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMatch : MonoBehaviour
{
    public static DoubleMatch lastSelected;
    public static bool useDoubleMatch;

    public bool useIt = false;

    [Header("Character Index Overrides")]
    public int bot1CharacterIndex = -1;
    public int bot2CharacterIndex = -1;
    public int bot3CharacterIndex = -1;

    public void SetDoubleMatch()
    {
        useDoubleMatch = useIt;
        lastSelected = this;
    }
}
