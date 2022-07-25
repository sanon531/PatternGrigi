using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
namespace PG.Battle 
{
    public class CurrentStatusScript : MonoBehaviour
    {
        [SerializeField]
        Image _image;
        [SerializeField]
        TextMeshProUGUI _text;
        private static CurrentStatusScript _instance;
        float _leftTime = 0f;
        // Start is called before the first frame update
        void Start()
        {
            if (_instance != null)
                Debug.LogError("one more current Status");
            _instance = this;
        }
        // Update is called once per frame
        void Update()
        {
        }

        // 대충 트위닝을 통한 애니메이션을 넣을 꺼란 예정.
        public static void SetTextOnCurrentScript(string text , float time) 
        {
            _instance._text.text = text;
            _instance.transform.DOShakePosition(0.5f, 100f);
        }

    }

}
