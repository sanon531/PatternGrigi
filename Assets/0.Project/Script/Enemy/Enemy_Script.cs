using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using MoreMountains.NiceVibrations;

using PG.Data;
namespace PG.Battle
{
    public class Enemy_Script : MonoBehaviour, IGetHealthSystem
    {
        public static Enemy_Script Instance;

        [SerializeField]
        private float healthAmountMax, startingHealthAmount, currentHealth;
        bool _isEnemyAlive = true;
        private HealthSystem _healthSystem;
        [SerializeField]
        SpriteRenderer _sprite;
        [SerializeField]
        ParticleSystem _damageFX;
        public List<PresetDemoItem> _VibrationItems;

        private void Start()
        {
            if (Instance != null)
                Debug.LogError("nore than one enemy error");
            Instance = this;
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += HealthSystem_OnDead;
        }

        bool _isStatusChangable = true;
        float _actionTime, _maxActionTime;
        private void Update()
        {
            if (_isStatusChangable && _isEnemyAlive)
            {
                if (_actionTime > 0)
                {
                    _actionTime -= Time.deltaTime;
                }
                else
                {
                    SetNextAction();
                }
            }
        }


        [SerializeField]
        int _currentActionOrder = 0;
        [SerializeField]
        List<EnemyAction> _enemyActionList = new List<EnemyAction>() { EnemyAction.Wait};
        [SerializeField]
        ActionFloatDic _actionTimeDic;
        void SetNextAction()
        {

            EnemyAction _tempAction = _enemyActionList[_currentActionOrder];
            _maxActionTime = _actionTimeDic[_tempAction];
            _actionTime = _maxActionTime;

            //여기서 액션의 처리가 진행이 되고 액션은주어진 리스트에 따라 결정 된다고 하자.
            switch (_tempAction)
            {
                case EnemyAction.Wait:
                    //Debug.Log("Wait");
                    break;
                case EnemyAction.BasicAttack_1:
                    //Debug.Log("BasicAttack_1");
                    break;
                case EnemyAction.BasicAttack_2:
                    //Debug.Log("BasicAttack_2");
                    break;
                case EnemyAction.BasicAttack_3:
                    //Debug.Log("BasicAttack_3");
                    break;
                case EnemyAction.SpecialAttack:
                    //Debug.Log("SpecialAttack");
                    break;
            }
            _currentActionOrder++;
            _currentActionOrder =
                (_currentActionOrder < _enemyActionList.Count) ? _currentActionOrder : 0;
        }




        #region //Damage related
        public static void Damage(float _amount)
        {
            Instance._healthSystem.Damage(_amount);
            Instance._damageFX.Play();
            Debug.Log(_amount);

            DamageTextScript.Create(Instance._sprite.transform.position, 2f, 0.3f, (int)_amount, Color.red);

        }

        private void HealthSystem_OnDead(object sender, System.EventArgs e)
        {
            CameraShaker.ShakeCamera(10, 1);
            _sprite.DOFade(0, 2);
            _isEnemyAlive = false;
        }

        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }

        public virtual void PlayAHAP(int index)
        {
            MMVibrationManager.AdvancedHapticPattern(_VibrationItems[index].AHAPFile.text,
                                             _VibrationItems[index].WaveFormAsset.WaveForm.Pattern, _VibrationItems[index].WaveFormAsset.WaveForm.Amplitudes, -1,
                                             _VibrationItems[index].RumbleWaveFormAsset.WaveForm.Pattern, _VibrationItems[index].RumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes,
                                             _VibrationItems[index].RumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1,
                                             HapticTypes.LightImpact, this, -1, false);

        }

        #endregion


    }


}