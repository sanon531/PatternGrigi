using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using UnityEngine.Serialization;

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
        
        private Dictionary<LaserKindID, ProjectilePool<GameObject>> _totalLaserDictionary
            = new Dictionary<LaserKindID, ProjectilePool<GameObject>>();
        
        private readonly int _poolInitialSpawnNum = 10;
        
        LaserKindID _nowPresetLaserKindID;
        
        private void Start()
        {
            //노드 9개 위치를 리스트에 저장
            foreach (PatternNodeScript node in PatternManager._instance._patternNodes)
            {
                _nodes.Add(node.transform);
            }
            InitializeDictionary();
        }
        
        private void InitializeDictionary()
        {
            foreach (LaserKindID id in Enum.GetValues(typeof(LaserKindID)))
            {
                if (_laserIDDic[id] != null)
                {
                    _totalLaserDictionary.Add(id,
                        new ProjectilePool<GameObject>
                        (
                            CreateLaser,
                            null,
                            null,
                            null,
                            true,
                            id :(int)id,
                            10000
                        )
                    );
                    for(int i = 0; i<_poolInitialSpawnNum ;i++)
                        _totalLaserDictionary[id].FillStack();
                }
            }
        }
        
        private GameObject CreateLaser(int id)
        {
            LaserKindID laserKindID = (LaserKindID)id;
            GameObject laser = Instantiate(_instance._laserIDDic[laserKindID], _instance.transform) ;
            
            laser.gameObject.SetActive(false);
            
            return laser;
        }

        public static void SetPresetPatternList(List<int> presetNodes,LaserKindID laserKindID)
        {
            _instance._nowPresetLaserKindID = laserKindID;
            _instance._listOfShow = new List<GameObject>();
            
            GameObject temp;

            //패턴의 노드번호를 받아서 해당하는 레이저 만들고 리스트에 추가
            for (int i=0; i < presetNodes.Count-1; i++)
            {
                //temp = Instantiate(_instance._laserIDDic[laserKindID], _instance.transform) ;

                temp = _instance._totalLaserDictionary[laserKindID].PickUp();
                
                if(temp.GetComponent<LaserParticle>() != null) 
                {
                    temp.GetComponent<LaserParticle>()._StartPos = _instance._nodes[presetNodes[i]].position;
                    temp.GetComponent<LaserParticle>()._EndPos = _instance._nodes[presetNodes[i + 1]].position;
                }
                else
                {
                    temp.GetComponent<LaserLine>()._StartPos = _instance._nodes[presetNodes[i]].position;
                    temp.GetComponent<LaserLine>()._EndPos = _instance._nodes[presetNodes[i + 1]].position;
                }

                _instance._listOfShow.Add(temp);
            }

            ShowPresetPatternAll();
        }
        
        public static void ShowPresetPatternAll() 
        {
            foreach (GameObject obj in _instance._listOfShow)
                obj.gameObject.SetActive(true);
        }
        public static void HidePresetPatternByID(int id)
        {
            GameObject target = _instance._listOfShow[id];
            
            target.SetActive(false);
            
            _instance._totalLaserDictionary[_instance._nowPresetLaserKindID].SetBack(target);
        }
        
        public static void HidePresetPatternAll()
        {
            foreach (GameObject obj in _instance._listOfShow)
            {
                obj.SetActive(false);
                _instance._totalLaserDictionary[_instance._nowPresetLaserKindID].SetBack(obj);   
            }
        }
    }
}
