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

        [SerializeField] private DynamicTextData _defaultData;
        [SerializeField] private GameObject _canvasPrefab;
        [SerializeField] private Transform _canvasTransform;

        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
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
            floattext.SetText(text.ToString());
            floattext.SetColor(color);

            DamageTextTweener tweener = ob.GetComponentInChildren<DamageTextTweener>();
            tweener.BeginTextTweeener(target, middle, lifeTime);
        }


        public static void ShowDamage(Vector2 position, string text)
        {
            GameObject newText = Instantiate(_instance._canvasPrefab, position, Quaternion.identity, _instance._canvasTransform);
            newText.GetComponent<DynamicTextForOneCanvas>().Initialise(text, _instance._defaultData);
        }




    }

}
