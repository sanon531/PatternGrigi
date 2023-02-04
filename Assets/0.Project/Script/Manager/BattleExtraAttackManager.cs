using System;
using System.Collections;
using System.Collections.Generic;
using PG.Data;
using UnityEditor;
using UnityEngine;

namespace PG.Battle
{
    
    // 전투도중의 특수 효과들을 저장해두는
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
        
        private  void StartLaser()
        {
            for (int i = 0; i < 10; i++)
            {
                _thunderLines.Add(Instantiate(thunderPrefab,transform).GetComponent<LaserLine>());
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
                yield return new WaitForFixedUpdate();
                ThunderCalc();
            }

            yield return null;
        }

        void ThunderCalc()
        {
            if (_thunderCalled == false)
                return;
            var transformList = Global_CampaignData._activatedProjectileList;
            int currentListCount = 1;
            int listCount = Global_CampaignData._activatedProjectileList.Count;
            int connectionCount = Mathf.RoundToInt(Global_CampaignData._thunderCount.FinalValue);



            while (true)
            {
                //print("currentListCount>= listCount : " + (currentListCount >= listCount) +
                      //"\n connectionCount > 0:" + (connectionCount > 0));
                if(currentListCount>=listCount)
                    return;
                if(connectionCount <= 0)
                    return;

                //print("Start  " + transformList[currentListCount - 1].position
                                //+ "\n end " + transformList[currentListCount].position);
                CalcDamageByThunder(currentListCount-1,transformList[currentListCount-1].position, 
                    transformList[currentListCount].position,
                    Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue/2);
                currentListCount++;
                connectionCount--;
            }

        }
        void CalcDamageByThunder(int num,Vector2 lastPos, Vector2 currentPos,float damage)
        {
            Vector2 dir = currentPos - lastPos;
            float range = Vector2.Distance(currentPos,lastPos);
            dir = dir.normalized;
            RaycastHit2D[] hits=new RaycastHit2D[30];
            var count= Physics2D.RaycastNonAlloc(lastPos,dir,hits,range);

            var laser = _thunderLines[num];
            laser.SetActiveLaser(true);
            laser._StartPos = lastPos;
            laser._EndPos = currentPos;
            
            for (int i =0 ; i<count;i++)
            {
                if (hits[i].transform.CompareTag("Enemy"))
                {
                    hits[i].transform.GetComponent<MobScript>().Damage(damage);
                }
            }
        }

        
        
        

        #endregion
      
    }
}