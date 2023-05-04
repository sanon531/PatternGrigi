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
        [SerializeField]
        private bool firstPlay = true;
        [SerializeField]
        private float bestTime = 0f;

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

        public bool FirstPlay
        {
            get { return firstPlay; }
            set
            {
                if (firstPlay != value)
                {
                    firstPlay = value;
                    SaveDataManager._instance.SaveData();
                }
            }
        }

        public float BestTime
        {
            get { return bestTime; }
            set
            {
                if (bestTime < value)
                {

                    bestTime = value;
                    SaveDataManager._instance.SaveData();
                }
            }
        }
    }
}
