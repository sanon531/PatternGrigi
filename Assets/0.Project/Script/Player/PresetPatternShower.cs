using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class PresetPatternShower : MonoSingleton<PresetPatternShower>
    {
        //����� �� �־ �ڵ�ȭ �Ͽ��� ���߿� ������ ������Ʈ�� 
        // �ϴ� 
        [SerializeField]
        List<GameObject> _listOfShow = new List<GameObject>(); 

        protected override void OnAwake()
        {
            foreach (GameObject obj in _listOfShow) 
            {
                obj.SetActive(false);
            }
        }

        public static void ShowPresetPatternAll() 
        {
            foreach (GameObject obj in _instance._listOfShow)
                obj.SetActive(true);

        }
        public static void HidePresetPatternByID(int id)
        {
            _instance._listOfShow[id].SetActive(false);
        }

        public static void HidePresetPatternAll(int id)
        {
            foreach (GameObject obj in _instance._listOfShow)
                obj.SetActive(false);
        }

    }


}
