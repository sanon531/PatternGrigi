using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using PG.Event;
namespace PG.Battle
{
    public class ObstacleManager : MonoBehaviour, ISetLevelupPause
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
            Global_BattleEventSystem._on레벨업일시정지 += SetLevelUpPauseOn;
            Global_BattleEventSystem._on레벨업일시정지해제 += SetLevelUpPauseOff;

        }
        private void OnDestroy()
        {
            Global_BattleEventSystem._on레벨업일시정지 -= SetLevelUpPauseOn;
            Global_BattleEventSystem._on레벨업일시정지해제 -= SetLevelUpPauseOff;
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


        bool _isLevelupPaused = false;
        public void SetLevelUpPauseOn()
        {
            _isLevelupPaused = true;
        }

        public void SetLevelUpPauseOff()
        {
            _isLevelupPaused = false;
        }
    }

}
