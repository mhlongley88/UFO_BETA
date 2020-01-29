using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UnlockNotification : MonoBehaviour
{
    public static UnlockNotification instance;
    public RectTransform canvasObject;
    public TextMeshProUGUI label;
    public float notificationDuration = 3.0f;

    float canvasInitialLocalY;

    bool currentlyShowing;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //canvasObject.localPosition += Vector3.down * canvasObject.sizeDelta.y;
        canvasInitialLocalY = canvasObject.localPosition.y;
    }

    private void Update()
    {
#if UNITY_EDITOR
        //test
        if (Input.GetKeyDown(KeyCode.J))
            Show("Hey! test!");
#endif
    }

    public void Show(string text)
    {
        if (currentlyShowing) return;

        currentlyShowing = true;
        label.text = text;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasObject.DOLocalMoveY(canvasObject.localPosition.y + canvasObject.sizeDelta.y, 0.5f));
        seq.AppendInterval(notificationDuration);
        seq.Append(canvasObject.DOLocalMoveY(canvasInitialLocalY, 0.5f));
        seq.AppendCallback(() => currentlyShowing = false);
    }
}
