using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Data 
{
    //�����ؾ��� ������ �� ����� �߰�
    [Serializable]
    public class SaveData
    {
        [SerializeField]
        private bool showTutorial = false;
        [SerializeField]
        private bool firstPlay = true;

        public bool ShowTutorial
        {
            get { return showTutorial; }
            set
            {
                //���� �ٲ��
                if (showTutorial != value)
                {
                    //�ڵ� ����
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
                //���� �ٲ��
                if (firstPlay != value)
                {
                    //�ڵ� ����
                    firstPlay = value;
                    SaveDataManager._instance.SaveData();
                }
            }
        }
    }
}
