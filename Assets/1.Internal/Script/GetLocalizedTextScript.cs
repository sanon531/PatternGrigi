using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PG.Data;
namespace PG 
{
    public class GetLocalizedTextScript : MonoSingleton<GetLocalizedTextScript>
    {

        private string _lang;
        private string _nameFile;
        [SerializeField]
        bool _isdebug;
        [SerializeField]
        SystemLanguage _currentLanguage;
        protected override void CallOnAwake()
        {
            base.CallOnAwake();
            LanguageDefinition();
            SetAllTextByLanguage();
        }
        private void LanguageDefinition()
        {
            //Debug.Log("System Language - " + Application.systemLanguage + "  " + SystemLanguage.Korean.ToString());

            if (Application.systemLanguage== SystemLanguage.Korean)
                _lang = "kr";
            else
                _lang = "en";
            //Debug.Log(_lang);
        }
        //이곳에서 아티팩트, 기타 텍스트들을 전부  관리함. 다른 로털라이제션 할데이터 있다면 여기서
        private void SetAllTextByLanguage() 
        {
            SetArtifactDataFromJson();
        }


        public static void ReloadLang()
        {
            _instance._lang = PlayerPrefs.GetString("lang");
            _instance.SetAllTextByLanguage();
        }
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

        public List<JsonStructure> LoadItem(string path)
        {
            List<JsonStructure> _temptList = new List<JsonStructure>(); 
            Debug.Log(path);
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path, Encoding.UTF8);
                JsonStructure[] myItem = JsonHelper.FromJson<JsonStructure>(jsonString);
                _temptList.Clear();

                foreach (JsonStructure item in myItem)
                    _temptList.Add(item);
            }
            else
            {
                Debug.LogError("Json file not found " + "\"" + _lang + "/" + _nameFile + ".json\"");
            }
            return _temptList;
        }


        #region//artifact
        Dictionary<ArtifactID, string> _artifactNameDic = new Dictionary<ArtifactID, string>() { };
        Dictionary<ArtifactID, string> _artifactArtifactEffectDic = new Dictionary<ArtifactID, string>() { };
        Dictionary<ArtifactID, string> _artifactDevCommentDic = new Dictionary<ArtifactID, string>() { };

        public void SetArtifactDataFromJson() 
        {
            List<JsonStructure> _temptList = new List<JsonStructure>();
            _artifactNameDic.Clear();
            _temptList = LoadItem(getPath("ArtifactName", _lang));
            foreach (JsonStructure json in _temptList) 
            {
                //Debug.Log(json.key + json.value);
                _artifactNameDic.Add((ArtifactID)Enum.Parse(typeof(ArtifactID), json.key), json.value);

            }
            _temptList = LoadItem(getPath("ArtifactEffect", _lang));
            foreach (JsonStructure json in _temptList)
            {
                //Debug.Log(json.key + json.value);
                _artifactArtifactEffectDic.Add((ArtifactID)Enum.Parse(typeof(ArtifactID), json.key), json.value);

            }

        }

        public static string GetArtifactDataFromJson(ArtifactJsonData jsonData, ArtifactID targetid) 
        {
            string _returnval = "Error";
            switch (jsonData)
            {
                case ArtifactJsonData.ArtifactName:
                    _returnval= _instance._artifactNameDic[targetid];
                    break;
                case ArtifactJsonData.ArtifactEffect:
                    _returnval = _instance._artifactArtifactEffectDic[targetid];
                    break;
                case ArtifactJsonData.DevComment:
                    _returnval = _instance._artifactDevCommentDic[targetid];
                    break;
                default:
                    Debug.LogError("GetArtifact data type Error");
                    break;
            }
            if (_returnval == "Error")
                Debug.LogError("Get ArtifactData failed");


            return _returnval;
        }
        #endregion
    }

}
