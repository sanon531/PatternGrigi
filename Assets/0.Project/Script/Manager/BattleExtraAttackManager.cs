using System;
using System.Collections;
using System.Collections.Generic;
using PG.Data;
using PG.Event;
using UnityEditor;
using UnityEngine;

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
            for (int i = 0; i < 40; i++)
            {
                _thunderLines.Add(Instantiate(thunderPrefab, transform).GetComponent<LaserLine>());
                _thunderLines[i]._StartPos = transform.position;
                _thunderLines[i]._EndPos = transform.position;
            }
        }

        public static void StartThunderCall()
        {
            _instance._thunderCalled = true;
            _instance.StartCoroutine(_instance.ThunderCallCoroutine());
        }

        public static void StopThunderCall()
        {
            _instance._thunderCalled = false;
        }


        private IEnumerator ThunderCallCoroutine()
        {
            while (_thunderCalled)
            {
                yield return new WaitForEndOfFrame();
                ThunderCalc();
            }

            yield return null;
        }

        private float damageInterval;

        void ThunderCalc()
        {
            if (_thunderCalled == false)
                return;
            var transformList = Global_CampaignData._activatedProjectileList;
            int transformListCount = Global_CampaignData._activatedProjectileList.Count;
            int currentTransformListCount = 1;
            int connectionCountMax = Mathf.RoundToInt(Global_CampaignData._thunderCount.FinalValue);
            int currentConnectionCount = 0;

            while (true)
            {
                if(transformListCount <= currentTransformListCount)
                    break;

                while (true)
                {
                    if(transformListCount <= currentTransformListCount)
                        break;
                    if(connectionCountMax<=currentConnectionCount)
                        break;
                    CalcDamageByThunder(currentTransformListCount - 1, transformList[currentTransformListCount - 1].position,
                        transformList[currentTransformListCount].position,
                        Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue / 2);
                    currentConnectionCount++;
                    currentTransformListCount++;
                }
                currentTransformListCount++;
                currentConnectionCount = 0 ;
            }
            for (int i = currentTransformListCount; i < _thunderLines.Count; i++)
            {
                var laser = _thunderLines[i];
                if(laser.GetActiveLaser())
                    laser.SetActiveLaser(false);
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

        void CalcDamageByThunder(int num, Vector2 lastPos, Vector2 currentPos, float damage)
        {
            Vector2 dir = currentPos - lastPos;
            float range = Vector2.Distance(currentPos, lastPos);
            dir = dir.normalized;
            RaycastHit2D[] hits = new RaycastHit2D[30];
            var count = Physics2D.RaycastNonAlloc(lastPos, dir, hits, range);

            var laser = _thunderLines[num];
            laser.SetActiveLaser(true);
            laser._StartPos = lastPos;
            laser._EndPos = currentPos;

            for (int i = 0; i < count; i++)
            {
                if (hits[i].transform.CompareTag("Enemy"))
                {
                    hits[i].transform.GetComponent<MobScript>().Damage(damage);
                }
            }
        }

        #endregion
      
        
        
        #region inspiration

        [SerializeField] private SpriteRenderer GaugeSpr;
        private float nowNodeCnt = 0;

        public static void EnableInspiration()
        {
            _instance.GaugeSpr.color = new Color(1f, 1f, 1f, _instance.nowNodeCnt / Global_CampaignData._inspirationNodeCount.FinalValue);
            
            Global_BattleEventSystem._onCalcPlayerAttack += _instance.OnPassingNode;
        }

        public static void DisableInspiration()
        {
            _instance.GaugeSpr.color = new Color(1f,1f,1f,0f);
            
            Global_BattleEventSystem._onCalcPlayerAttack -= _instance.OnPassingNode;
        }
        
        private void OnPassingNode(float damage)
        {
            nowNodeCnt++;
            GaugeSpr.color = new Color(1f,1f,1f,nowNodeCnt / Global_CampaignData._inspirationNodeCount.FinalValue);

            //full charging
            if (nowNodeCnt >= Global_CampaignData._inspirationNodeCount.FinalValue)
            {
                InspirationAttack();
                
                //magic circle animation
                GaugeSpr.GetComponent<Animator>().enabled = true;
            }
        }

        public static void OnCircleAnimationEnd()
        {
            _instance.nowNodeCnt = 0;
            _instance.GaugeSpr.color = new Color(1f,1f,1f,0f);
            _instance.GaugeSpr.GetComponent<Animator>().enabled = false;
        }
        
        private void InspirationAttack()
        {
            float basicDamage = Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue
                                * 0.5f * Global_CampaignData._inspirationNodeCount.FinalValue;
            //Damage Upgrade
            float finalDamage = basicDamage * (1.0f + 0.2f *
                (Global_CampaignData._currentArtifactDictionary[ArtifactID.Spread_Inspiration].UpgradeCount - 1));
            
            
        }


        #endregion
    }
}