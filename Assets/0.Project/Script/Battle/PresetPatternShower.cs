using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class PresetPatternShower : MonoBehaviour
    {
        private static PresetPatternShower s_instance;
        //욕심이 좀 있어서 자동화 하여서 나중에 내부의 오브젝트에 
        // 일단 
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

        public static void ShowPresetPattern() 
        {
            foreach (GameObject obj in s_instance._listOfShow)
                obj.SetActive(true);

        }
        public static void ClosePresetPatternByID(int id)
        {
            s_instance._listOfShow[id].SetActive(true);
        }

    }


}
