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
            GlobalUIEventSystem._on���� += PublicFadeIn;
            GlobalUIEventSystem._off���� += PublicFadeOut;
            _FadeInImage.enabled = false;

        }

        void PublicFadeIn() 
        {
            _FadeInImage.enabled = true;
            _FadeInImage.DOFade(1f, 1f);
        }
        void PublicFadeOut() 
        {
            _FadeInImage.DOFade(0, 1f);
            StartCoroutine(LateTurnOff());
        }

        IEnumerator LateTurnOff() 
        {
            yield return new WaitForSecondsRealtime(1f);
            _FadeInImage.enabled = false;
        }

    }
}
