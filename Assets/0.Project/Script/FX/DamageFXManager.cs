using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PG.Event;
namespace PG.Battle
{
    public class DamageFXManager : MonoBehaviour 
    {

        [SerializeField]
        GameObject _showPrefab;
        [SerializeField]
        Image _damageFadeImage;

        // Start is called before the first frame update
        void Start()
        {
            GlobalUIEventSystem._onDamageUI += GetDamage;
        }
        private void OnDestroy()
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
