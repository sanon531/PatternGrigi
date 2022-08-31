using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;

namespace PG.Battle 
{
    public class MobGenerator : MonoSingleton<MobGenerator>
    {
        //몹을 생성, 저장, 삭제
        [SerializeField]
        private Transform _SpawnRange_Left;
        [SerializeField]
        private Transform _SpawnRange_Right;
        public Transform _DamageLine;
        

        [SerializeField]
        private bool _spawnStart;

        [SerializeField]
        MobIDObjectDic _mobDic;

        [SerializeField]
        private List<MobSpawnData> _mobSpawnDataList = new List<MobSpawnData>();
        [SerializeField]
        private List<GameObject> _mobList;

        private int _mobCount = 0;


        void Start()
        {

        }

        void Update()
        {
            if (_spawnStart)
            {
                SpawnMob();
                _spawnStart = false;
                
            }
        }


        public void SpawnMob()
        {
            foreach (MobSpawnData mobSpawnData in _mobSpawnDataList)
            {
                StartCoroutine(SpawnStartCoroutine(mobSpawnData));
            }
        }

        IEnumerator SpawnStartCoroutine(MobSpawnData mobSpawnData)
        {
            yield return new WaitForSeconds(mobSpawnData.wait_time);

            StartCoroutine(SpawnCoroutine(mobSpawnData));
        }

        IEnumerator SpawnCoroutine(MobSpawnData mobSpawnData)
        {
            yield return new WaitForSeconds(mobSpawnData.respawn_delay);

            //스폰하는 부분

            Vector3 pos = new Vector3(UnityEngine.Random.Range(_SpawnRange_Left.position.x, _SpawnRange_Right.position.x), 
                _SpawnRange_Left.position.y, _SpawnRange_Left.position.z);

            GameObject temp = Instantiate(_mobDic[mobSpawnData.mobID], pos, _SpawnRange_Left.rotation);
            temp.GetComponent<MobScript>()._moveSpeed = mobSpawnData.move_speed;
            _mobList.Add(temp);
            _mobCount++;

            StartCoroutine(SpawnCoroutine(mobSpawnData));
        }

        //삭제는 나중에 사용할 때에 맞게 수정해야 할 듯
        public void DestroyMob(GameObject target)
        {
            _mobList.Remove(target);
            _mobCount--;
            Destroy(target);
        }

        //현재 몹들의 위치 순으로 리스트 정렬하고 리턴
        public List<GameObject> GetMobList()
        {
            _mobList.Sort((mobA, mobB) => mobA.transform.position.y.CompareTo(mobB.transform.position.y));
            return _mobList;
        }

        [Serializable]
        public class MobSpawnData
        {
            public MobID mobID;
            public float wait_time;
            public float respawn_delay;

            public float move_speed;
            
        }
    }

}
