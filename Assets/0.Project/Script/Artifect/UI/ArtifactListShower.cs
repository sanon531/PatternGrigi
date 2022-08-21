using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
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


        [SerializeField]
        Transform _placeTransform;

    


        //�̰����� ���� �̹����� �ø���.
        public static void SetNewCaseOnList(ArtifactID id) 
        {
            //���ο� �����͸� ���°�.
            ArtifactShowCase _temptObj = Instantiate( _instance._prefabCase, _instance._placeTransform).GetComponent<ArtifactShowCase>();
            _instance._currentDic.Add(id, _temptObj);
            _temptObj.SetDataOnCase(id);
        }

        public static void SetNumberOnCase(ArtifactID id,int val) 
        {
            _instance._currentDic[id].SetDataOnCase(val);
        }

    }
}