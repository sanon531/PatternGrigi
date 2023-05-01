using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;
namespace PG 
{
    public class MultiSceneUIScript : MonoSingleton<MultiSceneUIScript>
    {

        [SerializeField]
        Image _FadeInImage;

        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
            //GlobalUIEventSystem._onFadeOut += PublicFadeIn;
            //GlobalUIEventSystem._onFadeIn += PublicFadeOut;
            _FadeInImage.enabled = false;

        }

        public static void PublicFadeIn() 
        {
            _instance._FadeInImage.enabled = true;
            //_instance._FadeInImage.material.DOFloat(0, "_FadeAmount", 1f);
            _instance._FadeInImage.DOFade(1,1f);
        }
        public static void PublicFadeOut() 
        {
            //_instance._FadeInImage.material.DOFloat(1, "_FadeAmount", 1f).OnComplete(()=>_instance._FadeInImage.enabled = false);
            _instance._FadeInImage.DOFade(0,1f).OnComplete(()=>_instance._FadeInImage.enabled = false);
        }

        IEnumerable FadeOutProcess()
        {
            float timer = 1f;
            while (timer>0)
            {
                timer -= Time.deltaTime;
            }

            yield return null;
            
        }


    }
}
