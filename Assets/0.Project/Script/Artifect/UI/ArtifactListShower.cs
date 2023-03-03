using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using PG.Event;
using UnityEngine.UI;

namespace PG.Battle
{

    //아티팩트에 사용되는 리스트에 관한 데이터 굳이 나눈 이유는 이제
    //UI배정을 캔버스 쪽에다가 해놓았기 때문
    public class ArtifactListShower : MonoSingleton<ArtifactListShower>
    {
        [SerializeField]
        ArtifactIDShowCaseDic _currentDic;
        [SerializeField]
        GameObject _prefabCase;
        List<GameObject> _caseList = new List<GameObject>();

        [SerializeField]
        Transform _placeTransform;
        

        protected override void CallOnAwake()
        {
            RefreshCase();
            Global_BattleEventSystem._onBattleBegin += RefreshCase;
        }
        protected override void CallOnDestroy()
        {
            Global_BattleEventSystem._onBattleBegin -= RefreshCase;
        }



        //이곳에서 새로 이미지를 올린다.
        public static void SetNewCaseOnList(ArtifactID id) 
        {
            //새로운 데이터를 놓는것.
            ArtifactShowCase _temptObj = Instantiate( _instance._prefabCase, _instance._placeTransform).GetComponent<ArtifactShowCase>();
            _instance._caseList.Add(_temptObj.gameObject);
            _instance._currentDic.Add(id, _temptObj);
            //Debug.Log(id);
            _temptObj.SetDataOnCase(id);
        }

        public static void SetNumberOnCase(ArtifactID id,int val) 
        {
            _instance._currentDic[id].SetDataOnCase(val);
        }

        void RefreshCase() 
        {
            _currentDic.Clear();
            for (int i = _caseList.Count -1; i>=0; i--) 
            {
                Destroy(_caseList[i]);
            }

            var width = _placeTransform.GetComponent<RectTransform>().rect.width;
            _placeTransform.GetComponent<GridLayoutGroup>().cellSize = new Vector3(width,width);
        }

    }
}