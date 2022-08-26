using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
namespace PG.Battle 
{
    //���� �����ϰų� ������ ���۵ǰų� �ϴ� ��Ȳ���� �˸��� UI ���� ��.
    public class CurrentActionScript : MonoSingleton<CurrentActionScript>
    {
        [SerializeField]
        Image _image;
        [SerializeField]
        TextMeshProUGUI _text;
        float _leftTime = 0f;
        // Start is called before the first frame update
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {
        }

        // ���� Ʈ������ ���� �ִϸ��̼��� ���� ���� ����.
        public static void SetTextOnCurrentScript(string text , float time) 
        {
            _instance._text.text = text;
            _instance.transform.DOShakePosition(0.5f, 100f);
        }

    }

}
