using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Data 
{
    //저장해야할 데이터 더 생기면 추가
    [Serializable]
    public class SaveData
    {
        [SerializeField]
        private bool showTutorial = false;

        public bool ShowTutorial
        {
            get { return showTutorial; }
            set
            {
                //값이 바뀌면
                if (showTutorial != value)
                {
                    //자동 저장
                    showTutorial = value;
                    SaveDataManager._instance.SaveData();
                }
            }
        }
    }
}
