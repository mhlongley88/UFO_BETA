using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class UserDataPair
{
    public string key;
    public string value;

    public UserDataPair() { }
    public UserDataPair(string _key, string _value) { key = _key; value = _value; }
}

[System.Serializable]
public class UserDataContent
{
    public List<UserDataPair> data = new List<UserDataPair>();
}

public class UserPrefs : MonoBehaviour
{
    public static UserPrefs instance;

    string path, pathUFOPrefs;
    public string filename = "UFO_SaveData";
    public string filenameUFOPrefs = "UFO_UpgradePrefs";
    UserDataContent content;
    UFOPrefs contentUFOPrefs;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }


    }

    private void Start()
    {
        Debug.Log(Application.dataPath);
#if UNITY_EDITOR
        path = Application.dataPath + "/" + filename + ".json";
        pathUFOPrefs = Application.dataPath + "/" + filenameUFOPrefs + ".json";
#else
        path = Application.persistentDataPath + "/" + filename + ".json";
        pathUFOPrefs = Application.persistentDataPath + "/" + filenameUFOPrefs + ".json";
#endif
        Load();
        LoadUfoPrefs();
    }

    void Load()
    {
        if (!File.Exists(path))
        {
            content = new UserDataContent();
            var json = JsonUtility.ToJson(content, true);
            File.WriteAllText(path, json);
        }
        else
        {
            content = JsonUtility.FromJson<UserDataContent>(File.ReadAllText(path));
        }
    }

    void LoadUfoPrefs()
    {
        if (!File.Exists(pathUFOPrefs))
        {
            contentUFOPrefs = new UFOPrefs();
            foreach(GameManager.CharacterAssets asset in GameManager.Instance.Characters)
            {
                UFOAttributes props = SetUFOProps(asset);
                contentUFOPrefs.ufoData.Add(props);
                
            }
            var json = JsonUtility.ToJson(contentUFOPrefs, true);
            File.WriteAllText(pathUFOPrefs, json);
        }
        else
        {
            contentUFOPrefs = JsonUtility.FromJson<UFOPrefs>(File.ReadAllText(pathUFOPrefs));
            //Debug.Log(contentUFOPrefs.ufoData.Count);
        }
    }

    UFOAttributes SetUFOProps(GameManager.CharacterAssets asset)
    {
        UFOAttributes props = new UFOAttributes();
        CharacterLevelSelectInfo character = asset.characterModel.GetComponent<CharacterLevelSelectInfo>();
        props.ufoIndex = character.ufoIndex;
        props.Damage = character.Damage;
        props.RateOfFire = character.RateOfFire;
        props.Accuracy = character.Accuracy;
        props.isUnlocked = character.isUnlocked;
        props.currSkinId = -1;
        props.priceGems = character.priceGems;
        for (int i=0; i < character.Skins.Length; i++)
        {
            SkinProps skinProps = new SkinProps();
            skinProps.isUnlocked = false;
            skinProps.id = i;
            skinProps.priceGems = 52;
            props.Skins.Add(skinProps);
        }
        return props;
    }

   public UFOPrefs GetUFOProps()
    {
        //Debug.Log(contentUFOPrefs.ufoData.Count);
        return contentUFOPrefs;
    }

    public bool HasKey(string key)
    {
        return GetPair(key) != null;
    }

    public void AddPair(string key, string value)
    {
        var pair = GetPair(key);

        if (pair == null)
            content.data.Add(new UserDataPair(key, value));
        else
            pair.value = value;

        Save();
    }

    public void SetString(string key, string value)
    {
        AddPair(key, value);
    }

    public void SetInt(string key, int value)
    {
        AddPair(key, value.ToString());
    }

    public void SetFloat(string key, float value)
    {
        AddPair(key, value.ToString());
    }

    public void SetBool(string key, bool value)
    {
        AddPair(key, value.ToString());
    }

    public UserDataPair GetPair(string key)
    {
        return content.data.FirstOrDefault(it => it.key == key);
    }

    public string GetString(string key, string defaultValue = "")
    {
        var pair = GetPair(key);
        return pair != null ? pair.value : defaultValue;
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        var pair = GetPair(key);

        return pair != null ? System.Convert.ToInt32(pair.value) : defaultValue;
    }

    public float GetFloat(string key, float defaultValue = 0.0f)
    {
        var pair = GetPair(key);
        return pair != null ? System.Convert.ToSingle(pair.value) : defaultValue;
    }

    public bool GetBool(string key, bool defaultValue = false)
    {
        var pair = GetPair(key);
        return pair != null ? System.Convert.ToBoolean(pair.value) : defaultValue;
    }

    //private void OnDisable()
    //{
    //    Save();
    //}

    public void Save()
    {
        var json = JsonUtility.ToJson(content, true);
        File.WriteAllText(path, json);

        var json2 = JsonUtility.ToJson(contentUFOPrefs, true);
        File.WriteAllText(pathUFOPrefs, json2);
    }
}
