using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class PresetPatternShower : MonoBehaviour
    {
        private static PresetPatternShower s_instance;
        //����� �� �־ �ڵ�ȭ �Ͽ��� ���߿� ������ ������Ʈ�� 
        // �ϴ� 
        [SerializeField]
        List<GameObject> _listOfShow = new List<GameObject>(); 

        void Start()
        {
            s_instance = this;
            foreach (GameObject obj in _listOfShow) 
            {
                obj.SetActive(false);
            }
        }

        public static void ShowPresetPatternAll() 
        {
            foreach (GameObject obj in s_instance._listOfShow)
                obj.SetActive(true);

        }
        public static void HidePresetPatternByID(int id)
        {
            s_instance._listOfShow[id].SetActive(false);
        }
        public static void HidePresetPatternAll(int id)
        {
            foreach (GameObject obj in s_instance._listOfShow)
                obj.SetActive(false);
        }

    }


}
