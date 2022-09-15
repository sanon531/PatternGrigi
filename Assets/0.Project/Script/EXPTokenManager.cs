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

        // ���� �غ��� ������ �÷��̾��� ��ġ �� �κ��� �ν��Ѵ�
        //�׸��� �ش��ϴ� �������� �ν��ѵ� �ش� ������ ��ó�� ����ġ �˰��̰� ������ ����ġ �˰��̰� ������� ����ġ�� ������
        // ����ġ �˰��̴� �� �����̳� ����ġ ���� ���� ���� ���� �ǵ��� �� ��(�ڿ�����.)
        // ����� �׳� ����ġ �˰��� ����߸�����Ʈ ��ȸ �ϸ鼭 ��ó�� ����Ʈ���� �����͸� �о�鿩 Ȯ���� �Ѵ�.
        [SerializeField]
        GameObject _expTokenPrefab = new GameObject();
        List<GameObject> _activeTokenList = new List<GameObject>();
        List<GameObject> _inactiveTokenList = new List<GameObject>();


        //��Ʈ�� ������Ѽ� ó���Ұ�. 
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

        //�ϴ� ���� ���� �����ְ� �ڵ����� ȸ�� �� �ǵ��� �غ���.
        void Return() 
        {
        
        }




    }

}
