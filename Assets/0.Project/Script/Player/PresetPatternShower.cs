using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
namespace PG.Battle
{
    public class PresetPatternShower : MonoSingleton<PresetPatternShower>
    {
        //욕심이 좀 있어서 자동화 하여서 나중에 내부의 오브젝트에 
        // 일단 d
        
        [SerializeField]
        GameObject _LazerPrefab;
        
        List<Transform> _nodes = new List<Transform>();
        List<GameObject> _listOfShow;

        [SerializeField]
        LaserIDObjectDic _laserIDDic = new LaserIDObjectDic();

        [SerializeField]
        LaserIDObjectListDic _objectPoolDic = new LaserIDObjectListDic();


        private void Start()
        {
            //노드 9개 위치를 리스트에 저장
            foreach (PatternNodeScript node in PatternManager._instance._patternNodes)
            {
                _nodes.Add(node.transform);
            }
        }

        public static void SetPresetPatternList(List<int> presetNodes,LaserKindID laserKindID)
        {
            _instance._listOfShow = new List<GameObject>();

            GameObject temp;

            //패턴의 노드번호를 받아서 해당하는 레이저 만들고 리스트에 추가
            for (int i=0; i < presetNodes.Count-1; i++)
            {
                temp = Instantiate(_instance._laserIDDic[laserKindID], _instance.transform) ;

                if(temp.GetComponent<LazerParticle>() != null) 
                {
                    temp.GetComponent<LazerParticle>()._StartPos = _instance._nodes[presetNodes[i]].position;
                    temp.GetComponent<LazerParticle>()._EndPos = _instance._nodes[presetNodes[i + 1]].position;
                }
                else
                {
                    temp.GetComponent<LazerLine>()._StartPos = _instance._nodes[presetNodes[i]].position;
                    temp.GetComponent<LazerLine>()._EndPos = _instance._nodes[presetNodes[i + 1]].position;
                }

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
            Destroy(_instance._listOfShow[id]);
        }

        public static void HidePresetPatternAll(int id)
        {
            foreach (GameObject obj in _instance._listOfShow)
                obj.SetActive(false);
        }

    }


}
