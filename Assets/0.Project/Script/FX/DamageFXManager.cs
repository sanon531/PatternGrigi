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
            GlobalUIEventSystem._onDamageUI += GetDamage;
        }
        protected override void CallOnDestroy()
        {
            GlobalUIEventSystem._onDamageUI -= GetDamage;
        }
        public void GetDamage() 
        {
            //Instantiate(s_instance._showPrefab, s_instance.transform);
            _damageFadeImage.color = new Color(1,1,1,0.75f);
            _damageFadeImage.DOFade(0f,0.5f);
        }


    }

}
