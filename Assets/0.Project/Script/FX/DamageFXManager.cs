using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PG.Event;
namespace PG.Battle
{
    public class DamageFXManager : MonoSingleton<DamageFXManager>
    {

        [SerializeField]
        GameObject _showPrefab;
        [SerializeField]
        Image _damageFadeImage;

        //[SerializeField] private DynamicTextData _defaultData;
        [SerializeField] private GameObject _FXPrefab;
        [SerializeField] private Transform _canvasTransform;

        [SerializeField]
        List<GameObject> _activatedFXList = new List<GameObject>();
        [SerializeField]
        List<GameObject> _inactivatedFXList = new List<GameObject>();

        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
            for (int i = 0; i < 30; i++)
                _inactivatedFXList.Add(Instantiate(_FXPrefab, transform));
        }
        protected override void CallOnDestroy()
        {
        }
        public static void ShowDamage(Vector3 startPos, float lifeTime, int text, Color color, Transform target, Transform middle)
        {
            //Instantiate(s_instance._showPrefab, s_instance.transform);
            //_damageFadeImage.color = new Color(1,1,1,0.75f);
            //_damageFadeImage.DOFade(0f,0.5f);
            GameObject ob = GameObject.Instantiate(_instance._showPrefab, startPos, Quaternion.identity);
            FloatingText floattext = ob.GetComponentInChildren<FloatingText>();
            floattext.SetText(text.ToString(),color);
        }

        //오브젝트 풀링 적용하기
        public static void ShowDamage(Vector2 position, string text, Color color)
        {
            GameObject _fxtext;
            if (_instance._inactivatedFXList.Count == 0)
            {
                _fxtext = Instantiate(_instance._FXPrefab, _instance.transform);
                _instance._activatedFXList.Add(_fxtext);
            }
            else 
            {
                _fxtext = _instance._inactivatedFXList[0];
                _instance._inactivatedFXList.Remove(_fxtext);
                _instance._activatedFXList.Add(_fxtext);
            }
            _fxtext.transform.position = position;
            _fxtext.GetComponent<FloatingText>().SetText(text, color);
        }


        public static void FinishFX(GameObject obj) 
        {
            if (_instance._activatedFXList.Contains(obj)) 
            {
                _instance._activatedFXList.Remove(obj);
                _instance._inactivatedFXList.Add(obj);
                obj.transform.position = _instance.transform.position;
            }

        }




    }

}
