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

        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
        }
        protected override void CallOnDestroy()
        {
        }
        public static void ShowDamage(Vector3 startPos, float lifeTime, int text, Color color,Transform target,Transform middle) 
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


    }

}
