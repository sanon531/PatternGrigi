using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;

namespace PG.Battle
{
    public class ChargeGaugeUIScript : MonoBehaviour
    {
        static ChargeGaugeUIScript s_instance;

        [SerializeField]
        Image _chargeGaugeImage;

        [SerializeField]
        ParticleSystem _fulledParticle;
        [SerializeField]
        ParticleSystem _keepGoingParticle;

        // Start is called before the first frame update
        void Start()
        {
            s_instance = this;
            // ���⼭ �̹����� .SetLoops(-1, LoopType.Restart); �� ����� ���� ���ݽ��� �ִϸ��̼� ǥ�ø� �غ���.
        }
        private void OnDestroy()
        {
        }

        public static void SetChargeGauge(float inputCharge) 
        {
            s_instance.SetGaugeChange(inputCharge);
        }

        void SetGaugeChange(float inputCharge)
        {
            _chargeGaugeImage.fillAmount = (inputCharge);
        }

        public static void StartChargeSkill()
        {
            s_instance._fulledParticle.Play();
            s_instance._keepGoingParticle.Play();
        }
        public static void EndChargeSkill()
        {
            s_instance._fulledParticle.Stop();
            s_instance._keepGoingParticle.Stop();
        }


    }

}
