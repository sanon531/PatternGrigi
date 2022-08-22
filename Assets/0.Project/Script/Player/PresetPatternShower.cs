using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

namespace PG.Battle
{
    public class PresetPatternShower : MonoSingleton<PresetPatternShower>
    {
        //욕심이 좀 있어서 자동화 하여서 나중에 내부의 오브젝트에 
        // 일단 
        
        [SerializeField]
        GameObject _lightningBoltPrefab;
        
        List<Transform> _nodes = new List<Transform>();
        List<GameObject> _listOfShow;


        protected override void CallOnAwake()
        {
        }
        private void Start()
        {
            //노드 9개 위치를 리스트에 저장
            foreach (PatternNodeScript node in PatternManager._instance._patternNodes)
            {
                _nodes.Add(node.transform);
            }
        }

        public static void SetPresetPatternList(List<int> presetNodes)
        {
            _instance._listOfShow = new List<GameObject>();

            GameObject temp, start, end;

            //패턴의 노드번호를 받아서 해당하는 레이저 만들고 리스트에 추가
            for (int i=0; i < presetNodes.Count-1; i++)
            {
                temp = Instantiate(_instance._lightningBoltPrefab, _instance.transform);
                start = temp.GetComponent<LightningBoltScript>().StartObject;
                end = temp.GetComponent<LightningBoltScript>().EndObject;
                start.transform.position = _instance._nodes[presetNodes[i]].position;
                end.transform.position = _instance._nodes[presetNodes[i+1]].position;
                _instance._listOfShow.Add(temp);
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
