using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
namespace PG.Battle
{
    public class ObstacleManager : MonoBehaviour
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

        public static void DeleteObstacleOnList(Obstacle _obstacle) 
        {
            _instance._activeObstacleList.Remove(_obstacle);
        }

        void Update()
        {

        }
    }

}
