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

        protected override void CallOnAwake()
        {
            InitializeDictionary();
        }
        protected override void CallOnDestroy()
        {
        }
        public void InitializeDictionary()
        {
            _obstacleDic.Clear();
            List<ObstacleID> obstacleIDs = Enum.GetValues(typeof(ObstacleID)).Cast<ObstacleID>().ToList();

            foreach (ObstacleID id in obstacleIDs)
            {
                GameObject _tempfromobject = Resources.Load<GameObject>("Obstacle/" + id.ToString());
                if (_tempfromobject == null)
                {
                    Debug.LogError("Obstacle is not in resource" + id.ToString());
                }
                _obstacleDic.Add(id, _tempfromobject);
            }
        }

        public static void SetObstacle(SpawnData data, Vector2 pos,float damage)
        {
            GameObject _temp = Instantiate(_instance._obstacleDic[data._thisID], _instance.transform);
            _temp.transform.position = pos;
            Obstacle _tempObstacle = _temp.GetComponent<Obstacle>();
            _tempObstacle.SetSpawnData(data._lifeTime, data._activeTime, damage);
            _instance._activeObstacleList.Add(_tempObstacle);

        }

        public static void DeleteObstacleOnList(Obstacle obstacle)
        {
            if (_instance._activeObstacleList.Contains(obstacle))
            {
                _instance._activeObstacleList.Remove(obstacle);
                Destroy(obstacle.gameObject);
            }
        }
        public static void DeleteObstacleOnListAll()
        {
            foreach (Obstacle obs in _instance._activeObstacleList) 
            {
                obs.SetLifeTime(0);
            }

        }


        void Update()
        {

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
