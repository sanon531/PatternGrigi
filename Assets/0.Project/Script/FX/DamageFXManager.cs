using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace PG.Battle
{
    public class DamageFXManager : MonoBehaviour
    {
        static DamageFXManager s_instance;

        [SerializeField]
        GameObject _showPrefab;
        [SerializeField]
        Image _damageFadeImage;


        // Start is called before the first frame update
        void Start()
        {
            s_instance = this;
        }

        public static void Damage(float damageVal) 
        {
            //Instantiate(s_instance._showPrefab, s_instance.transform);
            s_instance._damageFadeImage.color = new Color(1,1,1,0.75f);
            s_instance._damageFadeImage.DOFade(0f,0.5f);
        }


    }

}
