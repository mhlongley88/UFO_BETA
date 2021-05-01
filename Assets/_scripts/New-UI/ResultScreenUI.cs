using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultScreenUI : MonoBehaviour
{
    public PlayerData[] playerData;
    public PlayerDataUI[] WinDataUI, LoseDataUI;
    public PlayerDataUI localWinUI, localLoseUI;
    public Transform WinUfoContainer, LoseUfoContainer, UFOTemplate;

    public GameObject winScreen, loseScreen;

    
    private bool isLocalWinner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EnableResultScreen(bool localWinner)
    {
        Camera[] cams = FindObjectsOfType<Camera>();
        foreach(Camera c in cams)
        {
            c.gameObject.SetActive(false);
        }
        if (localWinner)
        {
            winScreen.SetActive(true);
        }
        else
        {
            loseScreen.SetActive(true);
        }
        //Debug.Log(localWinner);
        UpdateLocalPlayerUI(localWinner);
    }

    public void UpdateLocalPlayerUI(bool localWinner)
    {
        UFOAttributes attr = GameManager.Instance.GetSelectedUfoAttribute();
        float prog = attr.ufoXP;
        int level = attr.ufoLevel;

        Transform container;
        if (localWinner)
        {
            
            container = WinUfoContainer;

            localWinUI.currLevelProgress.fillAmount = prog;
            localWinUI.plevel.text = (level + 1).ToString();
        }
        else
        {
            container = LoseUfoContainer;

            localLoseUI.currLevelProgress.fillAmount = prog;
            localLoseUI.plevel.text = (level + 1).ToString();
        }

        GameObject pModel = GameManager.Instance.GetLocalPlayerModel();
        GameObject localPlayer = Instantiate(pModel, container);
        localPlayer.transform.localPosition = UFOTemplate.localPosition;
        localPlayer.transform.localScale = UFOTemplate.localScale;
    }

    public void EnableResultContainer(int rank, PlayerData pd)
    {
        //Debug.Log(pd.plevel);
        WinDataUI[rank].container.SetActive(true);
        //WinDataUI[rank].plevel.text = (int.Parse(pd.plevel) + 1).ToString();
        WinDataUI[rank].pname.text = pd.pname;
        WinDataUI[rank].currLevelProgress.fillAmount = pd.currLevelProgress;

        LoseDataUI[rank].container.SetActive(true);
        //LoseDataUI[rank].plevel.text = (int.Parse(pd.plevel) + 1).ToString();
        LoseDataUI[rank].pname.text = pd.pname;
        LoseDataUI[rank].currLevelProgress.fillAmount = pd.currLevelProgress;

        //currDataUI[rank].charImg = 
    }
}

[System.Serializable]
public class PlayerData {

    public string pname;
    public string plevel;
    public float currLevelProgress;
    public int charId;
}

[System.Serializable]
public class PlayerDataUI
{
    public TextMeshProUGUI pname;
    public TextMeshProUGUI plevel;
    public Image currLevelProgress, charImg;
    public GameObject container;
}