using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;

namespace PG.Battle
{
    public class ChargeSkillManager : MonoBehaviour
    {
        static ChargeSkillManager s_instance;

        [SerializeField]
        Image _chargeGaugeImage;
        float _maxCharge = 100;
        float _currentCharge = 100;
        bool _isChargeStart = false;
        [SerializeField]
        ParticleSystem _fulledParticle;

        // Start is called before the first frame update
        void Start()
        {
            s_instance = this;


        }

        // Update is called once per frame
        void Update()
        {
            if (_isChargeStart)
            {
                if (_currentCharge > 0)
                {
                    _currentCharge -= Time.deltaTime;
                    _chargeGaugeImage.fillAmount = (float)(_currentCharge / _maxCharge);
                }
                else
                {
                    _isChargeStart = false;
                }
            }



        }
        public static void AddSkillCharge(float inputCharge) 
        {
            s_instance.SetGaugeChange(inputCharge);
        }

        void SetGaugeChange(float inputCharge)
        {
            _currentCharge += inputCharge;
            if (_maxCharge <= _currentCharge)
            {
                StartPatternSkill();
            }
            _chargeGaugeImage.fillAmount = (float)(_currentCharge / _maxCharge);
        }

        void StartPatternSkill()
        {
            //patternmanager.call특수패턴부르기();
            //글로벌 배틀 이벤트.call차지게이지다참();
            _fulledParticle.Play();
            _isChargeStart = true;
        }
    }

}
