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
        //몹을 생성, 저장, 삭제
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
            //웨이브 넘어갈 때
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
            yield return new WaitForSeconds(mobSpawnData._스폰대기시간);

            StartCoroutine(SpawnCoroutine(charID, mobSpawnData));
        }

        IEnumerator SpawnCoroutine(CharacterID charID, MobSpawnData mobSpawnData)
        {
            yield return new WaitForSeconds(mobSpawnData._리스폰딜레이);

            //스폰하는 부분

            Vector3 pos = new Vector3(UnityEngine.Random.Range(_SpawnRange_Left.position.x, _SpawnRange_Right.position.x),
                _SpawnRange_Left.position.y, _SpawnRange_Left.position.z);

            MobScript temp = Instantiate(_mobDic[charID], pos, _SpawnRange_Left.rotation).GetComponent<MobScript>();
            //temp.SetInitializeMob(mobSpawnData._actionDic);
            _mobList.Add(temp);
            _mobCount++;

            StartCoroutine(SpawnCoroutine(charID, mobSpawnData));
        }

        //삭제는 나중에 사용할 때에 맞게 수정해야 할 듯
        public static void DestroyMob(MobScript target)
        {
            _instance._mobList.Remove(target);
            _instance._mobCount--;
            Destroy(target.gameObject);
        }

        //현재 몹들의 위치 순으로 리스트 정렬하고 리턴
        public static List<MobScript> GetMobList()
        {
            _instance._mobList.Sort((mobA, mobB) => mobA.transform.position.y.CompareTo(mobB.transform.position.y));
            return _instance._mobList;
        }

        public static float GetDeadLine()
        {
            return _instance._DamageLine.position.y;
        }

        // 몹의 스폰은이렇게하지말자. 
        // 주어진 시트로 현재 최소한으로 소환일 될 적들의 수,
        // 적들의


    }

}
