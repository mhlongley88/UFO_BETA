using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class LevelLoad : MonoBehaviour
{
    public string[] levelSceneNames;
    public static string lastLoadedLevel;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (!LobbyConnectionHandler.instance.IsMultiplayerMode)
        {
            lastLoadedLevel = levelSceneNames[ShowLevelTitle.levelStaticInt - 1];

            UnlockSystem.instance.recentlyUnlockedCharacters.Clear();
            UnlockSystem.instance.recentlyUnlockedLevels.Clear();

            yield return new WaitForSeconds(3.0f);

            //ShowLevelTitle.levelStaticInt = 1;

            if (!string.IsNullOrEmpty(LoadIntroSceneLevel.introSceneLevel))
            {
                SceneManager.LoadSceneAsync(LoadIntroSceneLevel.introSceneLevel);
            }
            else
            {
                int index = GameManager.Instance.selectedLevelIndex;
                if (GameManager.Instance.enterTutorial)
                {
                    index = levelSceneNames.Length-1 ;
                }
                SceneManager.LoadScene(levelSceneNames[index/*ShowLevelTitle.levelStaticInt - 1*/]);
                SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
            }
        }
        else
        {
            yield return new WaitForSeconds(3.0f);
            if (PhotonNetwork.IsMasterClient)
            {
                this.GetComponent<PhotonView>().RPC("LoadSceneOnlineMultiplayer", RpcTarget.AllViaServer, GameManager.Instance.selectedLevelIndex/*ShowLevelTitle.levelStaticInt - 1*/);
            }

        }
    }

    [PunRPC]
    public void LoadSceneOnlineMultiplayer(int sceneIndex)
    {
        lastLoadedLevel = levelSceneNames[sceneIndex];

        UnlockSystem.instance.recentlyUnlockedCharacters.Clear();
        UnlockSystem.instance.recentlyUnlockedLevels.Clear();

        //yield return new WaitForSeconds(3.0f);

        //ShowLevelTitle.levelStaticInt = 1;

        //if (!string.IsNullOrEmpty(LoadIntroSceneLevel.introSceneLevel))
        //{
        //    SceneManager.LoadSceneAsync(LoadIntroSceneLevel.introSceneLevel);
        //}
        //else
        {

            SceneManager.LoadScene(levelSceneNames[sceneIndex]);
            SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
        }
    }
}
