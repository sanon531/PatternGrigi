using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
        [SerializeField]
        private List<Transform> spawnPosList;
        
        private readonly List<Vector3> _spawnPos = new List<Vector3>();


        protected override void CallOnAwake()
        {
            base.CallOnAwake();
            foreach (var posOrigin in spawnPosList)
            {
                _spawnPos.Add(posOrigin.position);
            }
        }

        protected override void CallOnDestroy()
        {
            base.CallOnDestroy();
        }


        void Start()
        {
            _waveTimeList = Global_CampaignData._waveTimeList;
            _waveClassList = Global_CampaignData._waveClassList;
            
            Global_BattleEventSystem._onGameOver +=DelayedDelete;
            Global_BattleEventSystem._onGameClear += DelayedDelete;
        }

        private void OnDisable()
        {
            Global_BattleEventSystem._onGameOver -=DelayedDelete;
            Global_BattleEventSystem._onGameClear -= DelayedDelete;
        }

        void Update()
        {
            if (Global_CampaignData._gameCleared || Global_CampaignData._gameOver)
                return;
            
            if (BattleSceneManager._instance.GetPlayTime() > _waveTimeList[_currentWaveOrder]) 
                NextWave();

            if (_spawnMobCount >= _currentMinMobNum)
            {
                FillMobs();
            }
        }


        #region Wave,Spawn

        private MobIDSpawnDataDic _currentMobSpawnDataDic;
        private int _currentWaveOrder = 0;
        private List<float> _waveTimeList;
        private List<WaveClass> _waveClassList;

        private static Dictionary<CharacterID, ProjectilePool<MobScript>> _totalMobDictionary
            = new Dictionary<CharacterID, ProjectilePool<MobScript>>();
        
        private static readonly int _poolInitialSpawnNum = 20;
        private int _sortingOrder = 0;
        private void NextWave()
        {
            Global_BattleEventSystem.CallOnWaveChange(_currentWaveOrder);
            
            
            //이전 웨이브 코루틴들 정리 + 변수 초기화
            StopAllCoroutines();
            _sortingOrder = 0;
            _spawnCrtnList = new List<IEnumerator>();
            _fillSpawnCrtnList = new List<IEnumerator>();
            _spawnMobCount = 0;
            _fillSpeed = 2f;
            _isFilling = false;

            //다음 웨이브 데이터
            _currentMobSpawnDataDic = _waveClassList[_currentWaveOrder].GetSpawnDataDic();
            _currentMinMobNum = _waveClassList[_currentWaveOrder].GetMinMobNum();
            //몹 오브젝트 풀 세팅
            SettingNowPools();

            //스폰시작
            foreach (CharacterID charID in _currentMobSpawnDataDic.Keys)
            {
                StartSpawnMob(charID, _currentMobSpawnDataDic[charID]);
            }
            _currentWaveOrder++;
        }

        private void StartSpawnMob(CharacterID mobID, MobSpawnData mobSpawnData)
        {
            StartCoroutine(WaitCoroutine(mobID, mobSpawnData));
        }
        
        private void SpawnMob(CharacterID mobID, MobSpawnData mobSpawnData)
        {
            _sortingOrder++;
            
            //random
            
            
            //Vector3 pos = new Vector3(UnityEngine.Random.Range(_SpawnRange_Left.position.x, _SpawnRange_Right.position.x), _SpawnRange_Left.position.y, _SpawnRange_Left.position.z);

            Vector3 pos = _spawnPos.PickRandom();
            
            MobScript temp = _totalMobDictionary[mobID].PickUp();
            temp.transform.position = pos;
            
            temp.SetInitializeMobSpawnData(mobID, mobSpawnData,_sortingOrder);
            _mobList.Add(temp);
            _aliveMobCount = _mobList.Count;
            _spawnMobCount++;
        }
        #endregion

        void DelayedDelete()
        {
            Debug.Log("deleted");
            StopAllCoroutines();
            DeleteAllEnemy();
        }

        IEnumerator DeleteAllEnemy()
        {
             yield return new WaitForSeconds(2f);
             for (int i = 0 ; i < _mobList.Count;i++) {
                 Destroy(_mobList[i]);
             }
        }


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

            IEnumerator crtn = SpawnCoroutine(mobID, mobSpawnData);
            _spawnCrtnList.Add(crtn);
            StartCoroutine(crtn);

            //최소 마리수 맞출 때 쓸 fill코루틴도 만들어두기 
            _fillSpawnCrtnList.Add(FillSpawnCoroutine(mobID, mobSpawnData));
        }

        
        IEnumerator SpawnCoroutine(CharacterID mobID, MobSpawnData mobSpawnData)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(mobSpawnData._리스폰딜레이);

            while (true)
            {
                for(int i = 0; i < mobSpawnData._스폰수; i++)
                {
                    SpawnMob(mobID, mobSpawnData);
                }
                
                yield return waitForSeconds;
            }
        }

        IEnumerator FillSpawnCoroutine(CharacterID mobID, MobSpawnData mobSpawnData)
        {
            float respawnDelay = mobSpawnData._리스폰딜레이;

            while (true)
            {
                SpawnMob(mobID, mobSpawnData);
                //속도 계속 바꿔줄거라 new로
                yield return new WaitForSeconds(respawnDelay/_fillSpeed);
            }
        }

#endregion

#region//Polling

        private void SettingNowPools()
        {
            //이전에 만들어둔 pool이 남아있는 경우
            foreach (ProjectilePool<MobScript> pool in _totalMobDictionary.Values)
            {
                pool.Clear();
            }
            
            _totalMobDictionary.Clear();
                    
            foreach (CharacterID id in _currentMobSpawnDataDic.Keys)
            {
                _totalMobDictionary.Add(id,
                    new ProjectilePool<MobScript>
                    (
                        CreateMob,
                        OnGet,
                        OnRelease,
                        null,
                        true,
                        id :(int)id,
                        10000
                    )
                );
                        
                for (int i = 0; i<_poolInitialSpawnNum ;i++)
                    _totalMobDictionary[id].FillStack();
            }
        }

        private MobScript CreateMob(int id)
        {
            CharacterID mobID = (CharacterID)id;
            MobScript mob = Instantiate(_mobDic[mobID], transform).GetComponent<MobScript>();
            mob.gameObject.SetActive(false);
                    
            return mob;
        }
                
        private void OnGet(MobScript mob)
        {
            mob.gameObject.SetActive(true);
        }
        
        private void OnRelease(MobScript mob)
        {
            mob.gameObject.SetActive(false);
        }

        private void OnDestroyMob(MobScript mob)
        {
            Destroy(mob.gameObject);
        }
        
#endregion

#region//Others

        public static void RemoveMob(CharacterID mobID, MobScript target)
        {
            if (_totalMobDictionary.ContainsKey(mobID))
            {
                _totalMobDictionary[mobID].SetBack(target);
            }
            else
            {
                //웨이브가 넘어가서 풀이 사라진 경우는 그냥 파괴
                Destroy(target.gameObject);
            }
            
            _instance._mobList.Remove(target);
            _instance._aliveMobCount--;
            
        }

        //현재 몹들의 위치 순으로 리스트 정렬하고 리턴
        public static List<MobScript> GetMobList()
        {
            _instance._mobList.Sort((mobA, mobB) => mobA.transform.position.y.CompareTo(mobB.transform.position.y));
            return _instance._mobList;
        }
        public static MobScript GetClosestEnemy()
        {
            return _instance.CalcClosestEnemy();
        }

        private MobScript CalcClosestEnemy()
        {
            var playerTranform = Player_Script.GetPlayerPosition();
            MobScript closestEnemyTransform = null;
            float closestDistanceSqr = Mathf.Infinity;
            foreach (var potentialEnemyTransform in _mobList)
            {
                Vector3 directionToEnemy = potentialEnemyTransform.GetMobPosition() - playerTranform;
                float dSqrToEnemy = directionToEnemy.sqrMagnitude;
                if (dSqrToEnemy < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToEnemy;
                    closestEnemyTransform = potentialEnemyTransform;
                }
            }
            return closestEnemyTransform;
        }

        public static float GetDeadLine()
        {
            return _instance._DamageLine.position.y;
        }
 
#endregion
    }
}
