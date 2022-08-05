using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Event;
using DG.Tweening;

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
            // 여기서 이미지에 .SetLoops(-1, LoopType.Restart); 를 사용한 차지 공격시의 애니메이션 표시를 해보자.

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
                    EndChargePattern();
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
            Global_BattleEventSystem.CallOn차지시작();
            _fulledParticle.Play();
            _isChargeStart = true;
        }
        void EndChargePattern() 
        {
            _fulledParticle.Stop();
            Global_BattleEventSystem.CallOn차지종료();
            _isChargeStart = false;

        }
    }

}
