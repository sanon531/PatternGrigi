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

        // 지금 해보고 싶은거 플레이어의 터치 한 부분을 인식한다
        //그리고 해당하는 포지션을 인식한뒤 해당 포지션 근처에 경험치 알갱이가 있으면 경험치 알갱이가 사라지고 경험치가 오른다
        // 경험치 알갱이는 한 종류이나 경험치 량에 따라서 색이 변동 되도록 할 것(자연스레.)
        // 현재는 그냥 경험치 알갱이 떨어뜨리고리스트 순회 하면서 근처의 리스트들의 데이터를 읽어들여 확인을 한다.


        //두트윈 적용시켜서 처리할것. 
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

        //일단 놓은 것을 보여주고 자동으로 회수 가 되도록 해보자.
        //지금은 그냥 
        void ReturnObj(int _amount) 
        {
            _inactiveTokenList.Add(_temptObj);
            _activeTokenList.Remove(_temptObj);
            _temptObj.GetComponent<ParticleSystem>().Stop();
             Global_BattleEventSystem.CallOnGainEXP(_amount);

        }




    }

}
