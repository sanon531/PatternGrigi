using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
using PG.Data;

namespace PG.Battle
{
    public class MobGenerator2 : MonoSingleton<MobGenerator2>
    {
        //���� ����, ����, ����
        [SerializeField]
        private Transform _SpawnRange_Left;
        [SerializeField]
        private Transform _SpawnRange_Right;
        public Transform _DamageLine;

        [SerializeField]
        MobIDObjectDic _mobDic;

        [SerializeField]
        private List<MobScript> _mobList;
        private int _mobCount = 0;

        private MobIDSpawnDataDic _currentMobSpawnDataDic;
        private List<float> _waveTimeList;
        private List<WaveClass> _waveClassList;
        private int _currentWaveOrder = 0;

        void Start()
        {
            _waveTimeList = Global_CampaignData._waveTimeList;
            _waveClassList = Global_CampaignData._waveClassList;
        }

        void Update()
        {
            //���̺� �Ѿ ��
            if (BattleSceneManager._instance.GetTime_Minute() > _waveTimeList[_currentWaveOrder])
            {
                StopAllCoroutines();
                _currentMobSpawnDataDic = _waveClassList[_currentWaveOrder].GetSpawnDataDic();
                foreach (CharacterID charID in _currentMobSpawnDataDic.Keys)
                {
                    SpawnMob(charID, _currentMobSpawnDataDic[charID]);
                }
                _currentWaveOrder++;
            }
        }


        public void SpawnMob(CharacterID charID, MobSpawnData mobSpawnData)
        {
            StartCoroutine(SpawnStartCoroutine(charID, mobSpawnData));
        }

        IEnumerator SpawnStartCoroutine(CharacterID charID, MobSpawnData mobSpawnData)
        {
            yield return new WaitForSeconds(mobSpawnData._�������ð�);

            StartCoroutine(SpawnCoroutine(charID, mobSpawnData));
        }

        IEnumerator SpawnCoroutine(CharacterID charID, MobSpawnData mobSpawnData)
        {
            yield return new WaitForSeconds(mobSpawnData._������������);

            //�����ϴ� �κ�

            Vector3 pos = new Vector3(UnityEngine.Random.Range(_SpawnRange_Left.position.x, _SpawnRange_Right.position.x),
                _SpawnRange_Left.position.y, _SpawnRange_Left.position.z);

            MobScript temp = Instantiate(_mobDic[charID], pos, _SpawnRange_Left.rotation).GetComponent<MobScript>();
            //temp.SetInitializeMob(mobSpawnData._actionDic);
            _mobList.Add(temp);
            _mobCount++;

            StartCoroutine(SpawnCoroutine(charID, mobSpawnData));
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


    }

}
