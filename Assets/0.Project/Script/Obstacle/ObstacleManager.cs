using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using PG.Data;
using PG.Event;
namespace PG.Battle
{
    public class ObstacleManager : MonoSingleton<ObstacleManager>
    {
        [SerializeField]
        ObstacleIDObjectDic _obstacleDic;
        [SerializeField]
        List<Obstacle> _activeObstacleList = new List<Obstacle>();
        
        private static Dictionary<ObstacleID, IObjectPoolSW<Obstacle>> _totalObstacleDictionary 
            = new Dictionary<ObstacleID, IObjectPoolSW<Obstacle>>();
        
        private static readonly int _poolInitialSpawnNum = 10;
        
        protected override void CallOnAwake()
        {
            InitializeDictionary();
        }
        protected override void CallOnDestroy()
        {
        }
        public void InitializeDictionary()
        {
            foreach (ObstacleID id in Enum.GetValues(typeof(ObstacleID)))
            {
                //_obstacleDic.Add(id, Resources.Load<GameObject>("Obstacle/" + id));
                if (_obstacleDic[id] != null)
                {
                    _totalObstacleDictionary.Add(id,
                        new ProjectilePool<Obstacle>
                        (
                            CreateObstacle,
                            null,
                            OnRelease,
                            null,
                            true,
                            id :(int)id,
                            10000
                        )
                    );
                    for(int i = 0; i<_poolInitialSpawnNum ;i++)
                        _totalObstacleDictionary[id].FillStack();
                }
            }
        }
        
        public static void SetObstacle(SpawnData data, Vector2 pos,float damage)
        {
            Obstacle temp = _totalObstacleDictionary[data._thisID].PickUp();
            temp.transform.position = pos;
            temp.SetSpawnData(data._lifeTime, data._activeTime, damage, data._thisID);
            _instance._activeObstacleList.Add(temp);
            temp.gameObject.SetActive(true);
        }
        
        public static void DeleteObstacleOnList(Obstacle obstacle)
        {
            if (_instance._activeObstacleList.Contains(obstacle))
            {
                _instance._activeObstacleList.Remove(obstacle);
                //Destroy(obstacle.gameObject);
            }
            _totalObstacleDictionary[obstacle._id].SetBack(obstacle);
        }
        public static void DeleteObstacleOnListAll()
        {
            //pool clear가 아니고 모든 obstacle을 풀로 반납하는거
            foreach (Obstacle obs in _instance._activeObstacleList) 
            {
                obs.SetLifeTime(0);
                _totalObstacleDictionary[obs._id].SetBack(obs);
            }
        }

        private Obstacle CreateObstacle(int id)
        {
            ObstacleID tempID = (ObstacleID)id;
            Obstacle obstacle = Instantiate(_obstacleDic[tempID], transform).GetComponent<Obstacle>();
            obstacle.gameObject.SetActive(false);
            
            return obstacle;
        }

        private void OnRelease(Obstacle obstacle)
        {
            obstacle.gameObject.SetActive(false);
        }
        

        #region

        Dictionary<ObstacleID, List<GameObject>> _activateObstacleDictionary = new Dictionary<ObstacleID, List<GameObject>>()
        {
            {ObstacleID.Small_Fire ,new List<GameObject>(){ } },
            {ObstacleID.LongThinFire_Horizontal ,new List<GameObject>(){ } },
            {ObstacleID.LongThinFire_Vertical,new List<GameObject>(){ } },
            {ObstacleID.MovingLeafDownToUp,new List<GameObject>(){ } },
            {ObstacleID.MovingLeafLeftToRight,new List<GameObject>(){ } },
            {ObstacleID.MovingLeafRightToleft,new List<GameObject>(){ } },
            {ObstacleID.MovingLeafUpToDown,new List<GameObject>(){ } },
            {ObstacleID.Chase_Obstacle,new List<GameObject>(){ } },
            {ObstacleID.LookAt_Arrow,new List<GameObject>(){ } }

        };
        Dictionary<ObstacleID, List<GameObject>> _deactivateObstacleDictionary = new Dictionary<ObstacleID, List<GameObject>>()
        {
            {ObstacleID.Small_Fire ,new List<GameObject>(){ } },
            {ObstacleID.LongThinFire_Horizontal ,new List<GameObject>(){ } },
            {ObstacleID.LongThinFire_Vertical,new List<GameObject>(){ } },
            {ObstacleID.MovingLeafDownToUp,new List<GameObject>(){ } },
            {ObstacleID.MovingLeafLeftToRight,new List<GameObject>(){ } },
            {ObstacleID.MovingLeafRightToleft,new List<GameObject>(){ } },
            {ObstacleID.MovingLeafUpToDown,new List<GameObject>(){ } },
            {ObstacleID.Chase_Obstacle,new List<GameObject>(){ } },
            {ObstacleID.LookAt_Arrow,new List<GameObject>(){ } }        };


        GameObject PlaceObject(ObstacleID id)
        {
            if (_deactivateObstacleDictionary[id].Count == 0)
            {
                _deactivateObstacleDictionary[id].Add(Instantiate(_obstacleDic[id], transform));
            }
            GameObject _tempt = _deactivateObstacleDictionary[id][0];
            _deactivateObstacleDictionary[id].Remove(_tempt);
            _activateObstacleDictionary[id].Add(_tempt);
            return _tempt;
        }
        public static void SetBackObject(GameObject projectile, ObstacleID id)
        {
            if (_instance._activateObstacleDictionary[id].Contains(projectile))
            {
                _instance._activateObstacleDictionary[id].Remove(projectile);
                _instance._deactivateObstacleDictionary[id].Add(projectile);
            }
        }
        #endregion

    }

}
