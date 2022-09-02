using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;

namespace PG.Battle 
{
    public class MobGenerator : MonoSingleton<MobGenerator>
    {
        //���� ����, ����, ����
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
        private List<MobScript> _mobList;


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

            //�����ϴ� �κ�

            Vector3 pos = new Vector3(UnityEngine.Random.Range(_SpawnRange_Left.position.x, _SpawnRange_Right.position.x), 
                _SpawnRange_Left.position.y, _SpawnRange_Left.position.z);

            MobScript temp = Instantiate(_mobDic[mobSpawnData.mobID], pos, _SpawnRange_Left.rotation).GetComponent<MobScript>();
            temp.SetInitializeMob(mobSpawnData._actionDic);
            _mobList.Add(temp);
            _mobCount++;

            StartCoroutine(SpawnCoroutine(mobSpawnData));
        }

        //������ ���߿� ����� ���� �°� �����ؾ� �� ��
        public static void DestroyMob(MobScript target)
        {
            _instance._mobList.Remove(target);
            _instance._mobCount--;
            Destroy(target.gameObject);
        }

        //���� ������ ��ġ ������ ����Ʈ �����ϰ� ����
        public static List<MobScript> GetMobList()
        {
            _instance._mobList.Sort((mobA, mobB) => mobA.transform.position.y.CompareTo(mobB.transform.position.y));
            return _instance._mobList;
        }

        public static float GetDeadLine() 
        {
            return _instance._DamageLine.position.y;
        }

        // ���� �������̷�����������. 
        // �־��� ��Ʈ�� ���� �ּ������� ��ȯ�� �� ������ ��,
        // ������


        [Serializable]
        public class MobSpawnData
        {
            public CharacterID mobID;
            public float wait_time;
            public float respawn_delay;
            public MobActionDataDic _actionDic;

        }
    }

}
