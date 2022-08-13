using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using PG.Event;
namespace PG.Battle
{
    public class ObstacleManager : MonoBehaviour, ISetNontotalPause
    {
        [SerializeField]
        ObstacleIDObjectDic _obstacleDic;
        private static ObstacleManager _instance;
        [SerializeField]
        List<Obstacle> _activeObstacleList = new List<Obstacle>();

        void Awake()
        {
            if (_instance != null)
                Debug.LogError("Double Obstacle Manager");
             _instance = this;
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;

        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
        }
        public void InitializeDictionary(List<ObstacleID> obstacleIDs)
        {
            foreach (ObstacleID id in obstacleIDs) 
            {
                GameObject _tempfromobject = Resources.Load<GameObject>("Obstacle/"+ id.ToString());
                if (_tempfromobject == null) 
                {
                    Debug.LogError("Obstacle is not in resource"+ id.ToString());
                }
                _obstacleDic.Add(id, _tempfromobject);
            }
        }

        public static void SetObstacle(SpawnData data,Vector2 pos)
        {
            GameObject _temp = Instantiate(_instance._obstacleDic[data._thisID], _instance.transform);
            _temp.transform.position = pos;
            Obstacle _tempObstacle = _temp.GetComponent<Obstacle>();
            _tempObstacle.SetSpawnData(data._lifeTime, data._activeTime);
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
            foreach (Obstacle obstacle in _instance._activeObstacleList) 
            {
                _instance._activeObstacleList.Remove(obstacle);
                Destroy(obstacle.gameObject);
            }

        }


        void Update()
        {

        }


        bool _isLevelupPaused = false;
        public void SetNonTotalPauseOn()
        {
            _isLevelupPaused = true;
        }

        public void SetNonTotalPauseOff()
        {
            _isLevelupPaused = false;
        }
    }

}
