using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PG.Event;

namespace PG.Battle 
{

    public class EXPTokenManager : MonoSingleton<EXPTokenManager>
    {
        [SerializeField]
        Transform _particleLocation;
        [SerializeField]
        Transform _targetTransform;
        Vector3 _targetPos;
        // Start is called before the first frame update
        [SerializeField]
        GameObject _expTokenPrefab ;
        [SerializeField]
        List<GameObject> _activeTokenList = new List<GameObject>();
        [SerializeField]
        List<GameObject> _inactiveTokenList = new List<GameObject>();



        protected override void CallOnAwake()
        {
            _targetPos = _targetTransform.position;
            for (int i = 0; i < 30; i++)
                _inactiveTokenList.Add(Instantiate(_expTokenPrefab, _particleLocation));
        }

        // ���� �غ��� ������ �÷��̾��� ��ġ �� �κ��� �ν��Ѵ�
        //�׸��� �ش��ϴ� �������� �ν��ѵ� �ش� ������ ��ó�� ����ġ �˰��̰� ������ ����ġ �˰��̰� ������� ����ġ�� ������
        // ����ġ �˰��̴� �� �����̳� ����ġ ���� ���� ���� ���� �ǵ��� �� ��(�ڿ�����.)
        // ����� �׳� ����ġ �˰��� ����߸�����Ʈ ��ȸ �ϸ鼭 ��ó�� ����Ʈ���� �����͸� �о�鿩 Ȯ���� �Ѵ�.


        //��Ʈ�� ������Ѽ� ó���Ұ�. 
        public static void PlaceEXPToken(Vector3 _position, int _amount) 
        {
            _instance.GetTokenFromList(_position,_amount);
            //_instance._temptObj.
        }

        GameObject _temptObj ;
        void GetTokenFromList(Vector3 _position, int _amount) 
        {
            if (_inactiveTokenList.Count == 0)
            {
                _inactiveTokenList.Add(Instantiate(_expTokenPrefab, _particleLocation));
            }
            _temptObj = _inactiveTokenList[0];
            _temptObj.transform.position = _position;
            _inactiveTokenList.Remove(_temptObj);
            _activeTokenList.Add(_temptObj);
            _temptObj.GetComponent<ParticleSystem>().Play();
            _temptObj.transform.DOMove(_targetPos,2).SetDelay(0.5f).OnComplete(()=> ReturnObj(_amount));
        }

        //�ϴ� ���� ���� �����ְ� �ڵ����� ȸ�� �� �ǵ��� �غ���.
        //������ �׳� 
        void ReturnObj(int _amount) 
        {
            _inactiveTokenList.Add(_temptObj);
            _activeTokenList.Remove(_temptObj);
            _temptObj.GetComponent<ParticleSystem>().Stop();
             Global_BattleEventSystem.CallOnGainEXP(_amount);

        }




    }

}
