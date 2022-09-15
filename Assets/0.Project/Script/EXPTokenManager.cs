using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PG.Battle 
{
    struct EXPToken 
    {
    
    }

    public class EXPTokenManager : MonoSingleton<EXPTokenManager>
    {
        // Start is called before the first frame update



        protected override void CallOnAwake()
        {
            for(int i = 0; i < 30; i++)
                _inactiveTokenList.Add(Instantiate(_expTokenPrefab,transform));
        }

        // 지금 해보고 싶은거 플레이어의 터치 한 부분을 인식한다
        //그리고 해당하는 포지션을 인식한뒤 해당 포지션 근처에 경험치 알갱이가 있으면 경험치 알갱이가 사라지고 경험치가 오른다
        // 경험치 알갱이는 한 종류이나 경험치 량에 따라서 색이 변동 되도록 할 것(자연스레.)
        // 현재는 그냥 경험치 알갱이 떨어뜨리고리스트 순회 하면서 근처의 리스트들의 데이터를 읽어들여 확인을 한다.
        [SerializeField]
        GameObject _expTokenPrefab = new GameObject();
        List<GameObject> _activeTokenList = new List<GameObject>();
        List<GameObject> _inactiveTokenList = new List<GameObject>();


        //두트윈 적용시켜서 처리할것. 
        public static void PlaceEXPToken(Vector3 _position, int vector) 
        {
            _instance.GetTokenFromList();
            _instance._temptObj.transform.position = _position;
            //_instance._temptObj.
        }

        GameObject _temptObj = new GameObject();
        void GetTokenFromList() 
        {
            if (_inactiveTokenList.Count == 0)
            {
                _inactiveTokenList.Add(Instantiate(_expTokenPrefab, transform));
            }

            _temptObj = _inactiveTokenList[0];
            _inactiveTokenList.Remove(_temptObj);
            _activeTokenList.Add(_temptObj);
        }

        //일단 놓은 것을 보여주고 자동으로 회수 가 되도록 해보자.
        void Return() 
        {
        
        }




    }

}
