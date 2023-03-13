using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using System.IO;

namespace PG {
    public class SaveDataManager : MonoSingleton<SaveDataManager>
    {
        public SaveData saveData = new SaveData();

        string path;

        protected override void CallOnAwake()
        {
            base.CallOnAwake();
            path = Application.persistentDataPath + "/save.json";
            LoadData();
        }

        public void SaveData()
        {
            string data = JsonUtility.ToJson(saveData);
            File.WriteAllText(path, data);
        }

        public void LoadData()
        {
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                saveData = JsonUtility.FromJson<SaveData>(data);
            }
        }
    } 
}
