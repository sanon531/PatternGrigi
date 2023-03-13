using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;
namespace PG 
{
    public class MultiSceneUIScript : MonoBehaviour
    {

        [SerializeField]
        Image _FadeInImage;

        // Start is called before the first frame update
        void Start()
        {
            GlobalUIEventSystem._onFadeOut += PublicFadeIn;
            GlobalUIEventSystem._onFadeIn += PublicFadeOut;
            _FadeInImage.enabled = false;

        }

        void PublicFadeIn() 
        {
            _FadeInImage.enabled = true;
            _FadeInImage.material.DOFloat(0, "_FadeAmount", 1f);
        }
        void PublicFadeOut() 
        {
            _FadeInImage.material.DOFloat(1, "_FadeAmount", 1f);
            StartCoroutine(LateTurnOff());
        }

        IEnumerator LateTurnOff() 
        {
            yield return new WaitForSecondsRealtime(1f);
            _FadeInImage.enabled = false;
        }

    }
}
