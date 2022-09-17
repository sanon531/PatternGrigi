using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
using PG.Event;
namespace PG.Battle
{

    //��Ƽ��Ʈ�� ���Ǵ� ����Ʈ�� ���� ������ ���� ���� ������ ����
    //UI������ ĵ���� �ʿ��ٰ� �س��ұ� ����
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



        //�̰����� ���� �̹����� �ø���.
        public static void SetNewCaseOnList(ArtifactID id) 
        {
            //���ο� �����͸� ���°�.
            ArtifactShowCase _temptObj = Instantiate( _instance._prefabCase, _instance._placeTransform).GetComponent<ArtifactShowCase>();
            _instance._caseList.Add(_temptObj.gameObject);
            _instance._currentDic.Add(id, _temptObj);
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
        }

    }
}