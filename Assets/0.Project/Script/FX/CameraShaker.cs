using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace PG 
{
    public class CameraShaker : MonoSingleton<CameraShaker>
    {
        [SerializeField]
        CinemachineVirtualCamera _cinemachine;
        float _startIntensity = 0;
        float _startTimerTotal = 0;
        float _shakerTimer = 0;
        CinemachineBasicMultiChannelPerlin _cinemachinePerlin;
        // Start is called before the first frame update
        protected override void CallOnAwake()
        {
            _cinemachinePerlin = _cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        }

        // Update is called once per frame
        void Update()
        {
            if (_shakerTimer > 0)
            {
                _shakerTimer -= Time.deltaTime;
                if (_shakerTimer <= 0f)
                {
                    _cinemachinePerlin.m_AmplitudeGain = 0f;
                    Mathf.Lerp(_startIntensity, 0f, 1 - (_shakerTimer / _startTimerTotal));
                }
            }

        }
        public static void ShakeCamera(float _intensity, float _time)
        {
            _instance._cinemachinePerlin.m_AmplitudeGain = _intensity;
            _instance._startIntensity = _intensity;
            _instance._shakerTimer = _time;
            _instance._startTimerTotal = _time;


        }
    }

}
