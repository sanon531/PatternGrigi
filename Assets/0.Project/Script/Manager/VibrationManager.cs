using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

namespace PG.Battle 
{
    public class VibrationManager : MonoSingleton<VibrationManager> 
    {


        [SerializeField]
        PresetDemoItem _enemyDeadVib;
        [SerializeField]
        PresetDemoItem _playerDeadVib;


        // Start is called before the first frame update


        public static void CallVibration() 
        {

            MMVibrationManager.StopContinuousHaptic(true);
            _instance._vibAlive = true;
            _instance._leftTime = _instance._limitTime;
            MMVibrationManager.ContinuousHaptic(_instance._currentIntensity, _instance._currentSharpness, 
                _instance._leftTime, HapticTypes.LightImpact, _instance, true, -1, true);
        }

        public static void CallVibration(float time,float intensity, float sharpness)
        {
            MMVibrationManager.StopContinuousHaptic(true);
            _instance._vibAlive = true;
            _instance._leftTime = time;
            _instance._currentIntensity = intensity;
            _instance._currentSharpness = sharpness;
            _instance._leftTime = _instance._limitTime;
            MMVibrationManager.ContinuousHaptic(intensity, sharpness,
                time, HapticTypes.LightImpact, _instance, true, -1, true);
        }


        public static void CallGameOverVib() 
        {
        
        
        }
        public static void CallEnemyDieVib() 
        {
            PresetDemoItem _vibrationItems = _instance._playerDeadVib;
            MMVibrationManager.AdvancedHapticPattern(_vibrationItems.AHAPFile.text,
                                     _vibrationItems.WaveFormAsset.WaveForm.Pattern, _vibrationItems.WaveFormAsset.WaveForm.Amplitudes, -1,
                                     _vibrationItems.RumbleWaveFormAsset.WaveForm.Pattern, _vibrationItems.RumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes,
                                     _vibrationItems.RumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1,
                                     HapticTypes.LightImpact, _instance, -1, false);

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
        float _currentIntensity, _currentSharpness = 0.1f;

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
