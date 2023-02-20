using System;
using System.Collections;
using System.Collections.Generic;
using PG.Data;
using PG.Event;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace PG.Battle
{
    // Storing Special Battle Attack on Game. 
    public class BattleExtraAttackManager : MonoSingleton<BattleExtraAttackManager>
    {
        private void Update()
        {
        }

        protected override void CallOnAwake()
        {
            base.CallOnAwake();
            StartLaser();
        }


        #region thunder

        private bool _thunderCalled = true;
        [SerializeField] private GameObject thunderPrefab;
        private List<LaserLine> _thunderLines = new List<LaserLine>();
        private void StartLaser()
        {
        }

        public static void StartThunderCall()
        {
            _instance.StartThunder_Private();
        }

        void StartThunder_Private()
        {
            _thunderCalled = true;
            StartCoroutine(_instance.ThunderDamageCoroutine());
            StartCoroutine(_instance.ThunderLaserCoroutine());
            _damageLastTime = Time.time;
            _thunderLines.Add(Instantiate(thunderPrefab,transform).GetComponent<LaserLine>());
        }

        
        public static void StopThunderCall()
        {
            _instance._thunderCalled = false;
        }

        
        private IEnumerator ThunderDamageCoroutine()
        {
            while (_thunderCalled)
            {
                yield return new WaitForSeconds(0.1f);
                ThunderDamageCalc();
            }

            yield return null;
        }

        //레이져에서 받은 데이터를 기반으로
        private IEnumerator ThunderLaserCoroutine()
        {
            while (_thunderCalled)
            {
                yield return new WaitForFixedUpdate();
                ThunderLaserCalc();
            }

            yield return null;
        }
        
        private float _damageLastTime = 0;
        public int a = 0;
        private List<List<Vector3>> _currentLaserPositionList = new List<List<Vector3>>();
        //레이져의  
        void ThunderDamageCalc()
        {
            if (_thunderCalled == false)
                return;
            a = _currentLaserPositionList.Count;
            foreach (var posPair in _currentLaserPositionList)
            {
                CalcDamageByThunder(posPair[0],
                    posPair[1],
                    Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue / 2);
            }
            

            /*
            while (true)
            {
                //print("currentListCount>= transformListCount : " + (currentListCount >= transformListCount) +"\n connectionCount > 0:" + (connectionCount > 0));
                if (currentListCount >= transformListCount || connectionCount <= 0)
                {
                    break;
                }

                //print("Start  " + transformList[currentListCount - 1].position
                //+ "\n end " + transformList[currentListCount].position);
                CalcDamageByThunder(currentListCount - 1, transformList[currentListCount - 1].position,
                    transformList[currentListCount].position,
                    Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue / 2);
                currentListCount++;
                connectionCount--;
            }

            for (int i = currentListCount; i < _thunderLines.Count; i++)
            {
                var laser = _thunderLines[i];
                if(laser.GetActiveLaser())
                    laser.SetActiveLaser(false);
            }
            */
        }

        void CalcDamageByThunder(Vector2 lastPos, Vector2 currentPos, float damage)
        {
            Vector2 dir = currentPos - lastPos;
            float range = Vector2.Distance(currentPos, lastPos);
            dir = dir.normalized;
            RaycastHit2D[] hits = new RaycastHit2D[30];
            var count = Physics2D.RaycastNonAlloc(lastPos, dir, hits, range);

            for (int i = 0; i < count; i++)
            {
                if (hits[i].transform.CompareTag("Enemy"))
                {
                    hits[i].transform.GetComponent<MobScript>().Damage(damage);
                }
            }
        }


        private int _lastMaxLaserCount = 0;
        void ThunderLaserCalc()
        {
            //current
            var transformList = Global_CampaignData._activatedProjectileList;
            int transformListCount = Global_CampaignData._activatedProjectileList.Count;
            int currentTransformListCount = 1;
            int connectionCountMax = Mathf.RoundToInt(Global_CampaignData._thunderCount.FinalValue);
            int currentConnectionCount = 1;
            _currentLaserPositionList.Clear();
            while (true)
            {
                if(transformListCount <= currentTransformListCount)
                    break;
                while (true)
                {
                    if(transformListCount <= currentTransformListCount)
                        break;
                    if(connectionCountMax <= currentConnectionCount)
                        break;

                    if (_thunderLines.Count <= currentTransformListCount + 1)
                    {
                        _thunderLines.Add(Instantiate(thunderPrefab,transform).GetComponent<LaserLine>());
                        break;
                    }
                    
                    var laser = _thunderLines[currentTransformListCount];
                    laser.SetActiveLaser(true);
                    laser._StartPos = transformList[currentTransformListCount-1].position;
                    laser._EndPos = transformList[currentTransformListCount].position;

                    _currentLaserPositionList.Add(new List<Vector3>(){
                        transformList[currentTransformListCount-1].position
                        ,transformList[currentTransformListCount].position});
                    
                    
                    currentConnectionCount++;
                    currentTransformListCount++;
                }
                currentTransformListCount++;
                currentConnectionCount = 0 ;
            }

            if (_lastMaxLaserCount > currentTransformListCount)
            {
                print(_lastMaxLaserCount + "+" + currentTransformListCount);
                for (int i = currentTransformListCount; i < _lastMaxLaserCount; i++)
                    _thunderLines[i].SetActiveLaser(false);
            }
            _lastMaxLaserCount = currentTransformListCount;
        }

        #endregion
        
        
        #region inspiration

        [SerializeField] private ParticleSystem GaugeParticle;
        [SerializeField] private ParticleSystem EmitParticle;
        
        public static void EnableInspiration()
        {
            _instance.GaugeParticle.startColor= new Color(1f,1f,1f,0f);
            _instance.GaugeParticle.Play();

            Global_BattleEventSystem._onPatternFilled += _instance.OnPassingNode;
            Global_BattleEventSystem._onPatternSuccessed += _instance.OnPatternSuccess;
        }

        public static void DisableInspiration()
        {
            _instance.GaugeParticle.Stop();
            _instance.EmitParticle.Stop();
            
            Global_BattleEventSystem._onPatternFilled -= _instance.OnPassingNode;
        }
        
        private void OnPassingNode(float fillRate)
        {
            GaugeParticle.startColor= new Color(1f,1f,1f,fillRate * 0.7f);
            float s = 4.0f * fillRate + 1.0f;
            GaugeParticle.transform.localScale = new Vector3(s,s,s);
        }

        private void OnPatternSuccess(DrawPatternPresetID patternPreset)
        {
            GaugeParticle.startColor = new Color(1f,1f,1f,0f);
            
            if (patternPreset == DrawPatternPresetID.Empty_Breath)
            {
                InspirationAttack();
                _instance.EmitParticle.Play();
            }
        }
        
        private void InspirationAttack()
        {
            float basicDamage = Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue
                                * 0.5f * Global_CampaignData._randomPatternNodeCount.FinalValue;
            //Damage Upgrade
            float finalDamage = basicDamage * (1.0f + 0.2f *
                Global_CampaignData._currentArtifactDictionary[ArtifactID.Spread_Inspiration].UpgradeCount);

            List<MobScript> mobList = EmitParticle.GetComponentInChildren<InspirationCircleTrigger>().InRangeMobList;

            //중간에 삭제 가능성 있기 때문에 foreach는 불가능
            for (int i = 0; i < mobList.Count; i++)
            {
                mobList[i].Damage(finalDamage);
            }
        }


        #endregion
    }
}