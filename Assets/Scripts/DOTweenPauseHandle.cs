using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenPauseHandle : MonoBehaviour
{
    DOTweenAnimation[] doTweenPauseHandles;
    DOTweenPath[] paths;

    bool valuePaused;

    // Start is called before the first frame update
    void Start()
    {
        doTweenPauseHandles = GetComponentsInChildren<DOTweenAnimation>(true);
        paths = GetComponentsInChildren<DOTweenPath>(true);

        valuePaused = GameManager.Instance.paused;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.paused != valuePaused)
        {
            valuePaused = GameManager.Instance.paused;

            for (int i = 0; i < doTweenPauseHandles.Length; i++)
            {
                var anim = doTweenPauseHandles[i];

                if (valuePaused)
                    anim.DOPause();
                else 
                    anim.DOPlay();
            }

            for (int i = 0; i < paths.Length; i++)
            {
                var anim = paths[i];

                if (valuePaused)
                    anim.DOPause();
                else
                    anim.DOPlay();
            }
        }
    }
}
