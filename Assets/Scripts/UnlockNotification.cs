using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Rewired;

public class UnlockNotification : MonoBehaviour
{
    public static UnlockNotification instance;
    //public RectTransform canvasObject;
    //public TextMeshProUGUI label;
    //public float notificationDuration = 3.0f;
    //float canvasInitialLocalY;
    //bool currentlyShowing;

    public GameObject characterUnlockedVfx;
    public GameObject levelUnlockedVfx;

    public bool keepOnScreenUntilProgress = false;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //if (instance != null && instance != this)
        //{
        //    //Destroy(gameObject);
        //}
        //else
        //{
        //    instance = this;
        //    // DontDestroyOnLoad(gameObject);
        //}
    }

    //private void Start()
    //{
    //    //canvasObject.localPosition += Vector3.down * canvasObject.sizeDelta.y;
    //    canvasInitialLocalY = canvasObject.localPosition.y;
    //}

    private void Update()
    {
        if (keepOnScreenUntilProgress)
        {
            if (characterUnlockedVfx.activeInHierarchy || levelUnlockedVfx.activeInHierarchy)
            {
                for (int i = 0; i < 4; i++)
                {
                    var rewirePlayer = ReInput.players.GetPlayer(i);

                    if (rewirePlayer.GetButtonDown("Submit"))
                    {
                        if (characterUnlockedVfx.activeInHierarchy)
                            characterUnlockedVfx.SetActive(false);

                        if (levelUnlockedVfx.activeInHierarchy)
                            levelUnlockedVfx.SetActive(false);
                    }
                }
            }
        }

#if UNITY_EDITOR
        //test
        if (Input.GetKeyDown(KeyCode.J))
            SignalUnlockCharacter();

        if (Input.GetKeyDown(KeyCode.K))
            SignalUnlockLevel();
#endif
    }

    public void SignalUnlockCharacter()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => characterUnlockedVfx.SetActive(true));

        if (!keepOnScreenUntilProgress)
        {
            seq.AppendInterval(3.0f);
            seq.AppendCallback(() => characterUnlockedVfx.SetActive(false));
        }
    }

    public void SignalUnlockLevel()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => levelUnlockedVfx.SetActive(true));

        if (!keepOnScreenUntilProgress)
        {
            seq.AppendInterval(3.0f);
            seq.AppendCallback(() => levelUnlockedVfx.SetActive(false));
        }
    }

    //public void Show(string text)
    //{
    //    if (currentlyShowing) return;

    //    currentlyShowing = true;
    //    label.text = text;

    //    Sequence seq = DOTween.Sequence();
    //    seq.Append(canvasObject.DOLocalMoveY(canvasObject.localPosition.y + canvasObject.sizeDelta.y, 0.5f));
    //    seq.AppendInterval(notificationDuration);
    //    seq.Append(canvasObject.DOLocalMoveY(canvasInitialLocalY, 0.5f));
    //    seq.AppendCallback(() => currentlyShowing = false);
    //}
}
