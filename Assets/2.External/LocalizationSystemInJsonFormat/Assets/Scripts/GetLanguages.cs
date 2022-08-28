using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GetLanguages : MonoBehaviour
{
    [SerializeField] private Transform parentSpawnLangButt;

    [SerializeField] private GameObject langButt;

    void Start()
    {
        var path = getPath();
        var files = Directory.GetDirectories(path);
        foreach (string fileName in files)
        {
            SpawnLangButtons(fileName + "/config.json");
        }
    }

    void SpawnLangButtons(string path)
    {
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path, Encoding.UTF8);
            JsonStructureConfig[] myItem = JsonHelper.FromJson<JsonStructureConfig>(jsonString);

            foreach (JsonStructureConfig item in myItem)
            {
                GameObject butt;
                if (item.key == "config")
                {
                    butt = Instantiate(langButt, parentSpawnLangButt.position, Quaternion.identity) as GameObject;
                    butt.transform.SetParent(parentSpawnLangButt);
                    butt.GetComponent<ConfigButtLang>().title = item.title;
                    butt.GetComponent<ConfigButtLang>().key_lang = item.key_lang;
                    butt.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
        }
        else
        {
            Debug.LogError("Json file config not found " + "\"" + path + "\"");
        }

    }

    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/2.External/StreamingAssets/Languages/";
#elif UNITY_ANDROID
		return Application.persistentDataPath+"/2.External/StreamingAssets/Languages/";
#elif UNITY_IPHONE
		return Application.persistentDataPath+"/2.External/StreamingAssets/Languages/";
#else
        return Application.dataPath + "/2.External/StreamingAssets/Languages/";
#endif
    }

}

[Serializable]
public class JsonStructureConfig
{
    public string key;
    public string title;
    public string key_lang;
}
