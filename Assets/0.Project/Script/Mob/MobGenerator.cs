using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Event;
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
        [SerializeField]
        private Transform _DamageLine;
        [SerializeField]
        private MobIDObjectDic _mobDic;
        [SerializeField]
        private List<MobScript> _mobList;


        void Start()
        {
            _waveTimeList = Global_CampaignData._waveTimeList;
            _waveClassList = Global_CampaignData._waveClassList;
        }

        void Update()
        {
            if (BattleSceneManager._instance.GetPlayTime() > _waveTimeList[_currentWaveOrder])
            {
                NextWave();
            }

            if (_spawnMobCount >= _currentMinMobNum)
            {
                FillMobs();
            }
        }


#region//Wave,Spawn

        private MobIDSpawnDataDic _currentMobSpawnDataDic;
        private int _currentWaveOrder = 0;
        private List<float> _waveTimeList;
        private List<WaveClass> _waveClassList;

        private void NextWave()
        {
            //이전 웨이브 코루틴들 정리 + 변수 초기화
            StopAllCoroutines(); 
            _spawnCrtnList = new List<IEnumerator>();
            _fillSpawnCrtnList = new List<IEnumerator>();
            _spawnMobCount = 0;
            _fillSpeed = 2f;
            _isFilling = false;

            //다음 웨이브 데이터
            _currentMobSpawnDataDic = _waveClassList[_currentWaveOrder].GetSpawnDataDic();
            _currentMinMobNum = _waveClassList[_currentWaveOrder].GetMinMobNum();

            //스폰시작
            foreach (CharacterID charID in _currentMobSpawnDataDic.Keys)
            {
                StartSpawnMob(charID, _currentMobSpawnDataDic[charID]);
            }
            _currentWaveOrder++;
        }

        //풀링으로 변경 예정
        private void SpawnMob(CharacterID mobID)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(_SpawnRange_Left.position.x, _SpawnRange_Right.position.x),
                _SpawnRange_Left.position.y, _SpawnRange_Left.position.z);

            MobScript temp = Instantiate(_mobDic[mobID], pos, _SpawnRange_Left.rotation).GetComponent<MobScript>();
            //temp.SetInitializeMob(mobSpawnData._actionDic);
            _mobList.Add(temp);
            _aliveMobCount = _mobList.Count;
            _spawnMobCount++;
        }

        private void StartSpawnMob(CharacterID mobID, MobSpawnData mobSpawnData)
        {
            StartCoroutine(WaitCoroutine(mobID, mobSpawnData));
        }

#endregion



#region//Filling

        //[SerializeField]
        private float _fillSpeed = 2f;
        //[SerializeField]
        private int _aliveMobCount = 0;
        //[SerializeField]
        private int _spawnMobCount = 0;

        private bool _isFilling = false;
        private int _currentMinMobNum;

        private List<IEnumerator> _spawnCrtnList;
        private List<IEnumerator> _fillSpawnCrtnList;

        private void FillMobs() //최소 마리수 채우는 함수
        {
            if (_aliveMobCount < _currentMinMobNum)
            {
                
                if (!_isFilling)
                {
                    _isFilling = true;

                    //일반스폰하던거 멈추고
                    foreach (IEnumerator crtn in _spawnCrtnList)
                    {
                        StopCoroutine(crtn);
                    }
                    //Fill스폰으로 전환
                    foreach (IEnumerator crtn in _fillSpawnCrtnList)
                    {
                        StartCoroutine(crtn);
                    }
                }
                else
                {
                    //여기로 들어오면 계속 채우고 있는데 속도가 못따라간다는 뜻
                    //-> 채우는 속도 점점 up시킴
                    _fillSpeed += 0.01f;
                }

            }
            else
            {
                if (_isFilling)
                {
                    _isFilling = false;
                    foreach (IEnumerator crtn in _fillSpawnCrtnList)
                    {
                        StopCoroutine(crtn);
                    }
                    foreach (IEnumerator crtn in _spawnCrtnList)
                    {
                        StartCoroutine(crtn);
                    }
                }
                else
                {
                    //여기로 들어오면 몹을 잘 못잡고 있다는 뜻
                    //-> 채우는 속도 down 시킴 (최소 2배, up보단 느리게)
                    if(_fillSpeed > 2)
                    {
                        _fillSpeed -= 0.003f;
                    }
                }
            }
        }

#endregion



#region//Coroutines

        IEnumerator WaitCoroutine(CharacterID mobID, MobSpawnData mobSpawnData)
        {
            yield return new WaitForSeconds(mobSpawnData._스폰대기시간);

            IEnumerator crtn = SpawnCoroutine(mobID, mobSpawnData._리스폰딜레이);
            _spawnCrtnList.Add(crtn);
            StartCoroutine(crtn);

            //최소 마리수 맞출 때 쓸 fill코루틴도 만들어두기 
            _fillSpawnCrtnList.Add(FillSpawnCoroutine(mobID, mobSpawnData._리스폰딜레이));
        }

        
        IEnumerator SpawnCoroutine(CharacterID mobID, float respawnDelay)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(respawnDelay);

            while (true)
            {
                SpawnMob(mobID);
                yield return waitForSeconds;
            }
        }

        IEnumerator FillSpawnCoroutine(CharacterID mobID, float respawnDelay)
        {
            while (true)
            {
                SpawnMob(mobID);
                //속도 계속 바꿔줄거라 new로
                yield return new WaitForSeconds(respawnDelay/_fillSpeed);
            }
        }

#endregion



#region//Others

        //삭제는 나중에 사용할 때에 맞게 수정해야 할 듯
        public static void DestroyMob(MobScript target)
        {
            _instance._mobList.Remove(target);
            _instance._aliveMobCount--;
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

#endregion
    }
}
