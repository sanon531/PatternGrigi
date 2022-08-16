using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;

namespace PG.Battle
{
    public class ChargeGaugeUIScript : MonoSingleton<ChargeGaugeUIScript>
    {

        [SerializeField]
        Image _chargeGaugeImage;

        [SerializeField]
        ParticleSystem _fulledParticle;
        [SerializeField]
        ParticleSystem _keepGoingParticle;

        // Start is called before the first frame update
        void Start()
        {
            // 여기서 이미지에 .SetLoops(-1, LoopType.Restart); 를 사용한 차지 공격시의 애니메이션 표시를 해보자.
        }
        private void OnDestroy()
        {
        }

        public static void SetChargeGauge(float inputCharge) 
        {
            _instance.SetGaugeChange(inputCharge);
        }

        void SetGaugeChange(float inputCharge)
        {
            _chargeGaugeImage.fillAmount = (inputCharge);
        }

        public static void StartChargeSkill()
        {
            _instance._fulledParticle.Play();
            _instance._keepGoingParticle.Play();
        }
        public static void EndChargeSkill()
        {
            _instance._fulledParticle.Stop();
            _instance._keepGoingParticle.Stop();
        }


    }

}
