using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using PG.Data;
namespace PG
{
    public class GetLocalizedTextScript : MonoSingleton<GetLocalizedTextScript>
    {

        private string _lang;
        [SerializeField]
        bool _isdebug;
        [SerializeField]
        string _currentLanguage;


        protected override void CallOnAwake()
        {
            LanguageDefinition();
            SetAllTextByLanguage();
        }
        private void LanguageDefinition()
        {
            //Debug.Log("System Language - " + Application.systemLanguage + "  " + SystemLanguage.Korean.ToString());

            if (Application.systemLanguage == SystemLanguage.Korean)
                _lang = "kr";
            else
                _lang = "en";

            if (_isdebug)
                _lang = _currentLanguage;

            //Debug.Log(_lang);
        }
        //이곳에서 아티팩트, 기타 텍스트들을 전부  관리함. 다른 로털라이제션 할데이터 있다면 여기서
        private void SetAllTextByLanguage()
        {
            SetArtifactDataFromJson();
            SetUIDataFromJson();
        }


        public static void ReloadLang()
        {
            _instance.SetAllTextByLanguage();
        }
        private string getPath(string name, string lang)
        {
            return "Localization/Languages/" + lang + "/" + name ;
        }
        public List<JsonStructure> LoadItem(string path)
        {
            List<JsonStructure> _temptList = new List<JsonStructure>();
            TextAsset jsontext = Resources.Load<TextAsset>(path);
            JsonStructure[] myItem = JsonHelper.FromJson<JsonStructure>(jsontext.ToString());
            _temptList.Clear();

            foreach (JsonStructure item in myItem)
                _temptList.Add(item);
            return _temptList;
        }


        #region//artifact
        [SerializeField]
        ArtifactStringaDic _artifactNameDic = new ArtifactStringaDic() { };
        [SerializeField]
        ArtifactStringaDic _artifactArtifactEffectDic = new ArtifactStringaDic() { };
        [SerializeField]
        ArtifactStringaDic _artifactDevCommentDic = new ArtifactStringaDic() { };

        Dictionary<UITextID, string> _uiTextDic = new Dictionary<UITextID, string>();

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


        public void SetUIDataFromJson() 
        {
            List<JsonStructure> _temptList = new List<JsonStructure>();
            _uiTextDic.Clear();
            _temptList = LoadItem(getPath("UI", _lang));
            foreach (JsonStructure json in _temptList)
            {
                //Debug.Log(json.key + json.value);
                _uiTextDic.Add((UITextID)Enum.Parse(typeof(UITextID), json.key), json.value);
            }


        }


        public static string GetArtifactDataFromJson(ArtifactJsonData jsonData, ArtifactID targetid)
        {
            string _returnval = "Non Data";
            //Debug.Log(_instance._artifactNameDic[targetid]);
            try
            {
                switch (jsonData)
                {
                    case ArtifactJsonData.ArtifactName:
                        _returnval = _instance._artifactNameDic[targetid];
                        break;
                    case ArtifactJsonData.ArtifactEffect:
                        _returnval = _instance._artifactArtifactEffectDic[targetid];
                        break;
                    case ArtifactJsonData.DevComment:
                        _returnval = _instance._artifactDevCommentDic[targetid];
                        break;
                    default:
                        throw new ArgumentException ("GetArtifact data type Error");
                        break;
                }
            }
            catch (Exception e) 
            {
                throw new ArgumentException("Data NONO"+ jsonData.ToString()+" + " + targetid.ToString());
            }

            return _returnval;
        }


        public static string GetUIDataFromJson(UITextID uiTextID) 
        {
            string _returnval = "Non Data";
            //Debug.Log(_instance._artifactNameDic[targetid]);
            try
            {
                _returnval = _instance._uiTextDic[uiTextID];
            }
            catch (Exception e)
            {
                Debug.Log("Data NONO" + uiTextID.ToString());
            }

            return _returnval;

        }


        #endregion
    }

}
