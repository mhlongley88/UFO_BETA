using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelSelectionHandler : MonoBehaviour
{
    public Sprite[] LevelSprites;
    public string[] levelNames;

    public LevelSelectionUI[] levelSelectionUIs;

    public TextMeshProUGUI LevelNameTxt;

    public ScrollRect levelsScrollRect;

    public Vector3 DefaultCenterScrollViewPos;
    public Vector2 DefaultWidthHeightFrame, DefaultWidthHeightImg;
    public Vector2 IncreasedWidthHeightFrame, IncreasedWidthHeightImg;

    private int totalLevels;
    // Start is called before the first frame update
    void Start()
    {
        totalLevels = levelSelectionUIs.Length;
        levelNames = new string[11] { "Los Angeles", "San Francisco", "Mexico City", "Easter Island", "New York", "Paris", "Rome", "Egypt", "Tokyo", "Honolulu", "Bermuda Triangle"};
        
        SelectLevel_OnClick(0);
    }

    public void MoveToCenter(LevelSelectionUI newCenterObj)
    {
        newCenterObj.transform.SetSiblingIndex(totalLevels / 2);
        newCenterObj.layoutElement.preferredHeight = IncreasedWidthHeightFrame.y;
        newCenterObj.layoutElement.preferredWidth = IncreasedWidthHeightFrame.x;

        newCenterObj.LevelImg.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(IncreasedWidthHeightImg.x, IncreasedWidthHeightImg.y);

        levelsScrollRect.content.transform.localPosition = DefaultCenterScrollViewPos;
    }

    public void SelectLevel_OnClick(int id)
    {
        
        GameManager.Instance.selectedLevelIndex = id;

        UnSelectAllLevels();
        levelSelectionUIs[id].SelectionOutline.enabled = true;
        MoveToCenter(levelSelectionUIs[id]);
        LevelNameTxt.text = levelNames[id];
    }

    void UnSelectAllLevels()
    {
        foreach(LevelSelectionUI ui in levelSelectionUIs)
        {
            ui.SelectionOutline.enabled = false;
            ui.LevelImg.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(DefaultWidthHeightImg.x, DefaultWidthHeightImg.y);
            ui.layoutElement.preferredHeight = DefaultWidthHeightFrame.y;
            ui.layoutElement.preferredWidth = DefaultWidthHeightFrame.x;
        }
    }
}
