using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

namespace PG.Battle 
{
    public class VibrationManager : MonoBehaviour
    {

        static VibrationManager _instance;

        // Start is called before the first frame update
        void Awake()
        {
            _instance = this;
        }



        public static void CallVibration() 
        {

            MMVibrationManager.StopContinuousHaptic(true);
            _instance._vibAlive = true;
            _instance._leftTime = _instance._limitTime;
            MMVibrationManager.ContinuousHaptic(_instance._currentIntensity, _instance._currentIntensity, 
                _instance._leftTime, HapticTypes.LightImpact, _instance, true, -1, true);
        }

        public static void CallVibration(float time)
        {
            MMVibrationManager.StopContinuousHaptic(true);
            _instance._vibAlive = true;
            _instance._leftTime = time;
            _instance._leftTime = _instance._limitTime;
            MMVibrationManager.ContinuousHaptic(_instance._currentIntensity, _instance._currentIntensity,
                _instance._leftTime, HapticTypes.LightImpact, _instance, true, -1, true);
        }

        private void Update()
        {
            CheckVibration();
        }

        float _leftTime = 0f;
        [SerializeField]
        float _limitTime = 0.25f;
        bool _vibAlive = false;
        [SerializeField]
        float _currentIntensity, _currentSharpness = 1;

        void CheckVibration()
        {
            if (!_vibAlive)
                return;

            if (_leftTime > 0f)
            {
                _leftTime -= Time.deltaTime;
                //ShowDebugtextScript.SetDebug("time left" + _leftTime);
            }
            else
            {
                _vibAlive = false;
            }

        }

    }

}
