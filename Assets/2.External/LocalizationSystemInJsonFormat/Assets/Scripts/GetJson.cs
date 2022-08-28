using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GetJson : MonoBehaviour
{
    [SerializeField] private string _nameFile;

    [HideInInspector] public List<JsonStructure> Items = new List<JsonStructure>();

    private string _lang;

    private void Awake()
    {
        LanguageDefinition();

        LoadItem(getPath(_nameFile, _lang));
    }

    //system language detection or stored in the registry
    private void LanguageDefinition() {
        Debug.Log("System Language - " + Application.systemLanguage);

        if (!PlayerPrefs.HasKey("lang"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
                PlayerPrefs.SetString("lang", "ru");
            else
                PlayerPrefs.SetString("lang", "en");
        }

        _lang = PlayerPrefs.GetString("lang");
    }

    //updating the values ​​of text fields when changing the language
    public void ReloadLang()
    {
        _lang = PlayerPrefs.GetString("lang");
        LoadItem(getPath(_nameFile, _lang));
        SetValueLang[] scr = FindObjectsOfType<SetValueLang>();

        foreach (SetValueLang item in scr)
            item.SetValueText();
    }

    //Loading values ​​from json file by keys
    public void LoadItem(string path)
    {
        Debug.Log(path);
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path, Encoding.UTF8);
            JsonStructure[] myItem = JsonHelper.FromJson<JsonStructure>(jsonString);
            Items.Clear();

            foreach (JsonStructure item in myItem)
                Items.Add(item);
        }
        else
        {
            Debug.LogError("Json file not found " + "\"" + _lang + "/" + _nameFile + ".json\"");
        }

    }

    //Path where json files are located
    private string getPath(string name, string lang)
    {
#if UNITY_EDITOR
        return Application.dataPath + "/2.External/StreamingAssets/Languages/" + lang + "/" + name + ".json";
#elif UNITY_ANDROID
		return Application.persistentDataPath+"/2.External/StreamingAssets/Languages/" + lang + "/" + name + ".json";
#elif UNITY_IPHONE
		return Application.persistentDataPath+"/2.External/StreamingAssets/Languages/" + lang + "/" + name + ".json";
#else
        return Application.dataPath + "/2.External/StreamingAssets/Languages/" + lang + "/" + name + ".json";
#endif
    }

}

[Serializable]
public class JsonStructure
{
    public string key;
    public string value;
}

