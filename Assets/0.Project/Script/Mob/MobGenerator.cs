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
        //���� ����, ����, ����
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

        private static Dictionary<CharacterID, ProjectilePool<MobScript>> _totalMobDictionary
            = new Dictionary<CharacterID, ProjectilePool<MobScript>>();
        
        private void NextWave()
        {
            //���� ���̺� �ڷ�ƾ�� ���� + ���� �ʱ�ȭ
            StopAllCoroutines(); 
            _spawnCrtnList = new List<IEnumerator>();
            _fillSpawnCrtnList = new List<IEnumerator>();
            _spawnMobCount = 0;
            _fillSpeed = 2f;
            _isFilling = false;

            //���� ���̺� ������
            _currentMobSpawnDataDic = _waveClassList[_currentWaveOrder].GetSpawnDataDic();
            _currentMinMobNum = _waveClassList[_currentWaveOrder].GetMinMobNum();
            
            //�� ������Ʈ Ǯ ����
            SettingNowPools();

            //��������
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
            Vector3 pos = new Vector3(UnityEngine.Random.Range(_SpawnRange_Left.position.x, _SpawnRange_Right.position.x),
                _SpawnRange_Left.position.y, _SpawnRange_Left.position.z);
            
            MobScript temp = _totalMobDictionary[mobID].PickUp();
            temp.transform.position = pos;
            
            temp.SetInitializeMobSpawnData(mobID, mobSpawnData);
            _mobList.Add(temp);
            _aliveMobCount = _mobList.Count;
            _spawnMobCount++;
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

        private void FillMobs() //�ּ� ������ ä��� �Լ�
        {
            if (_aliveMobCount < _currentMinMobNum)
            {
                
                if (!_isFilling)
                {
                    _isFilling = true;

                    //�Ϲݽ����ϴ��� ���߰�
                    foreach (IEnumerator crtn in _spawnCrtnList)
                    {
                        StopCoroutine(crtn);
                    }
                    //Fill�������� ��ȯ
                    foreach (IEnumerator crtn in _fillSpawnCrtnList)
                    {
                        StartCoroutine(crtn);
                    }
                }
                else
                {
                    //����� ������ ��� ä��� �ִµ� �ӵ��� �����󰣴ٴ� ��
                    //-> ä��� �ӵ� ���� up��Ŵ
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
                    //����� ������ ���� �� ����� �ִٴ� ��
                    //-> ä��� �ӵ� down ��Ŵ (�ּ� 2��, up���� ������)
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
            yield return new WaitForSeconds(mobSpawnData._�������ð�);

            IEnumerator crtn = SpawnCoroutine(mobID, mobSpawnData);
            _spawnCrtnList.Add(crtn);
            StartCoroutine(crtn);

            //�ּ� ������ ���� �� �� fill�ڷ�ƾ�� �����α� 
            _fillSpawnCrtnList.Add(FillSpawnCoroutine(mobID, mobSpawnData));
        }

        
        IEnumerator SpawnCoroutine(CharacterID mobID, MobSpawnData mobSpawnData)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(mobSpawnData._������������);

            while (true)
            {
                SpawnMob(mobID, mobSpawnData);
                yield return waitForSeconds;
            }
        }

        IEnumerator FillSpawnCoroutine(CharacterID mobID, MobSpawnData mobSpawnData)
        {
            float respawnDelay = mobSpawnData._������������;

            while (true)
            {
                SpawnMob(mobID, mobSpawnData);
                //�ӵ� ��� �ٲ��ٰŶ� new��
                yield return new WaitForSeconds(respawnDelay/_fillSpeed);
            }
        }

#endregion

#region//Polling

        private void SettingNowPools()
        {
            //������ ������ pool�� �����ִ� ���
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
                        OnDestroyMob,
                        true,
                        id :(int)id,
                        10000
                    )
                );
                        
                for (int i = 0; i<20 ;i++)
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
                //���̺갡 �Ѿ�� Ǯ�� ����� ���� �׳� �ı�
                Destroy(target.gameObject);
            }
            
            _instance._mobList.Remove(target);
            _instance._aliveMobCount--;
            
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
 
#endregion
    }
}
