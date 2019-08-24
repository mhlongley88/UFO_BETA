using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RESULTSCharacterAssignScript : MonoBehaviour
{
    /*  public Texture ch0_1_Tex;
      public Texture ch0_2_Tex;
      public Texture ch0_3_Tex;
      public Texture ch0_4_Tex;*/

    public Texture ch1_Tex;
    public Texture ch2_Tex;
    public Texture ch3_Tex;
    public Texture ch4_Tex;
    public Texture ch5_Tex;
    public Texture ch6_Tex;
    public Texture ch7_Tex;
    public Texture ch8_Tex;
    public Texture ch9_Tex;
    public Texture ch10_Tex;
    public Texture ch11_Tex;
    public Texture ch12_Tex;
    public Texture ch13_Tex;
    public Texture ch14_Tex;
    public Texture ch15_Tex;

    /* public GameObject character0_1;
     public GameObject character0_2;
     public GameObject character0_3;
     public GameObject character0_4;*/
    public GameObject character1;
    public GameObject character2;
    public GameObject character3;
    public GameObject character4;
    public GameObject character5;
    public GameObject character6;
    public GameObject character7;
    public GameObject character8;
    public GameObject character9;
    public GameObject character10;
    public GameObject character11;
    public GameObject character12;
    public GameObject character13;
    public GameObject character14;
    public GameObject character15;

    public Material[] myMaterials;
    public Material[] myOtherMaterials;

    public Material myMaterial;
    public Material myOtherMaterial;

    private Renderer currRenderer;

    public GameObject alienSkeleton;
    public GameObject shipHull;

    public int rankPlace = 0;

    public Player playerType;

    private int currChar;

    private void OnEnable()
    {
        if (PlayerManager.Instance.gameHasEnded)
        {
            Assign();
        }
    }

    // Start is called before the first frame update

    public void Assign()
    {

        //     switch (rankPlace)
        //     {
        //         case 1:

        //             PlayerType = GameManagerScript.winnerPlayer;

        //             break;

        //         case 2:

        //             if (GameManagerScript.Instance.deadPlayersIndex.Count > 0)
        //                 PlayerType = GameManagerScript.Instance.deadPlayersIndex[GameManagerScript.Instance.deadPlayersIndex.Count - 1];


        //             break;

        //         case 3:

        //             if (GameManagerScript.Instance.deadPlayersIndex.Count > 1)
        //                 PlayerType = GameManagerScript.Instance.deadPlayersIndex[GameManagerScript.Instance.deadPlayersIndex.Count - 2];

        //             break;

        //         case 4:
        //             if (GameManagerScript.Instance.deadPlayersIndex.Count > 2)
        //                 PlayerType = GameManagerScript.Instance.deadPlayersIndex[GameManagerScript.Instance.deadPlayersIndex.Count - 3];

        //             break;
        //     }
        // }
        Player rankedPlayer = Player.None;
        foreach (Player p in PlayerManager.Instance.players.Keys)
        {
            if (PlayerManager.Instance.players[p].rank == rankPlace)
            {
                rankedPlayer = p;
            }
        }
        if (playerType != Player.None)
        {
            myMaterial = myMaterials[(int)playerType];
            myOtherMaterial = myOtherMaterials[(int)playerType];

            Setup();
        }
    }

    private void EnableRenderers(GameObject character, Texture characterTexture)
    {
        character.SetActive(true);
        currRenderer = character.GetComponentInChildren<Renderer>();
        currRenderer.material.SetTexture("_MainTex", characterTexture);


        myMaterial.SetTexture("_MainTex", characterTexture);
        myOtherMaterial.SetTexture("_MainTex", characterTexture);
    }
    void Setup()
    {


        currChar = GameManager.Instance.GetPlayerCharacterChoice(playerType);

        character1.SetActive(false);

        character2.SetActive(false);
        character3.SetActive(false);
        character4.SetActive(false);
        character5.SetActive(false);
        character6.SetActive(false);
        character7.SetActive(false);
        character8.SetActive(false);
        character9.SetActive(false);
        character10.SetActive(false);
        character11.SetActive(false);
        character12.SetActive(false);
        character13.SetActive(false);
        character14.SetActive(false);
        character15.SetActive(false);

        switch (currChar)
        {
            case 1:
                EnableRenderers(character1, ch1_Tex);
                break;


            case 2:
                EnableRenderers(character2, ch2_Tex);
                break;

            case 3:
                EnableRenderers(character3, ch3_Tex);
                break;

            case 4:
                EnableRenderers(character4, ch4_Tex);
                break;

            case 5:
                EnableRenderers(character5, ch5_Tex);
                break;

            case 6:
                EnableRenderers(character6, ch6_Tex);
                break;

            case 7:
                EnableRenderers(character7, ch7_Tex);
                break;

            case 8:
                EnableRenderers(character8, ch8_Tex);
                break;

            case 9:
                EnableRenderers(character9, ch9_Tex);
                break;

            case 10:
                EnableRenderers(character10, ch10_Tex);
                break;

            case 11:
                EnableRenderers(character11, ch11_Tex);
                break;

            case 12:
                EnableRenderers(character12, ch12_Tex);
                break;

            case 13:
                EnableRenderers(character13, ch13_Tex);
                break;

            case 14:
                EnableRenderers(character14, ch14_Tex);
                break;

            case 15:
                EnableRenderers(character15, ch15_Tex);
                break;
        }
        alienSkeleton.GetComponent<SkinnedMeshRenderer>().material = myMaterial;
        shipHull.GetComponent<MeshRenderer>().material = myMaterial;
    }

    void CharacterAssign()
    {



    }
}
