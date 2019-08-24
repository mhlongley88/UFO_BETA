using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{


    public string[] levelSceneNames;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(levelSceneNames[ShowLevelTitle.levelStaticInt - 1]);
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);

    }


}
