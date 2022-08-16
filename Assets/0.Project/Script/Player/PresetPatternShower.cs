using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class PresetPatternShower : MonoSingleton<PresetPatternShower>
    {
        //욕심이 좀 있어서 자동화 하여서 나중에 내부의 오브젝트에 
        // 일단 
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
