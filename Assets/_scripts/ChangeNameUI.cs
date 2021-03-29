using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeNameUI : MonoBehaviour
{
    public GameObject NameChangePanel;
    public TMP_InputField changeNameInputField;
    public TextMeshProUGUI lobbyNameText;
    public string[] RandomNames;
    string playerNameKey;
    // Start is called before the first frame update
    void Start()
    {
        playerNameKey = GameManager.Instance.playerNameKey;
        //Debug.Log(GameManager.Instance.GetDisplayName());
        if (GameManager.Instance.GetDisplayName() == "")
        {
            int rand = Random.Range(0, RandomNames.Length);
            GameManager.Instance.SetDisplayName(RandomNames[rand]);

            // Default Open Levels
            UserPrefs.instance.SetBool("levelUnlocked0", true);
            UserPrefs.instance.SetBool("levelUnlocked1", true);
            UserPrefs.instance.SetBool("levelUnlocked2", true);
        }
        UpdateDisplayNameUI(GameManager.Instance.GetDisplayName());
    }

    public void UpdateDisplayNameUI(string s)
    {
        LobbyConnectionHandler.instance.myDisplayName = lobbyNameText.text = s;
    }

    public void SubmitChangedName()
    {

        if (changeNameInputField.textComponent.text == "") return;
        
        GameManager.Instance.SetDisplayName(changeNameInputField.text);
        UpdateDisplayNameUI(changeNameInputField.text);
        //UserPrefs.instance.SetString(playerNameKey, changeNameInputField.text);
        changeNameInputField.text = "";
        ActivateNameChangePanel(false);
        MainMenuUIManager.Instance.touchMenuUI.MainHubCharacterContainer.gameObject.SetActive(true);
    }

    public void ActivateNameChangePanel(bool en)
    {
        NameChangePanel.SetActive(en);
    }
}
