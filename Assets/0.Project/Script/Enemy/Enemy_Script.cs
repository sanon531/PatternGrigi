using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using PG.Data;
using System;
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
        ActionDataDic _actionDataDic;
        void SetNextAction()
        {

            EnemyAction _tempAction = _enemyActionList[_currentActionOrder];
            ActionData _temptdata = _actionDataDic[_tempAction];
            _maxActionTime = _temptdata._actionTime;
            _actionTime = _maxActionTime;
            ShowDebugtextScript.SetDebug(_tempAction.ToString());
            //여기서 액션의 처리가 진행이 되고 액션은주어진 리스트에 따라 결정 된다고 하자.
            //나중에 코루틴으로 캔슬도 되고 막 그럴 꺼지만 지금은 간단한 형성만
            if(_tempAction!= EnemyAction.Wait) 
            {
                int i = 0;

                if (_temptdata._spawnType == SpawnType.SetAtOnce)
                {
                    foreach (Vector2 v in _temptdata._placeList)
                        StartCoroutine(SetObstacleRoutine(_temptdata._spawnData, v, _temptdata._placeTimeGradual));
                }
                else 
                {
                    foreach (Vector2 v in _temptdata._placeList)
                    {
                        i++;
                        StartCoroutine(SetObstacleRoutine(_temptdata._spawnData, v, i * _temptdata._placeTimeGradual));
                    }
                }


            }


            _currentActionOrder++;
            _currentActionOrder =
                (_currentActionOrder < _enemyActionList.Count) ? _currentActionOrder : 0;
        }

        IEnumerator SetObstacleRoutine(SpawnData data,Vector2 pos,float waitTime) 
        {
            //Debug.Log(waitTime);
            yield return new WaitForSecondsRealtime(waitTime);
            ObstacleManager.SetObstacle(data, pos);

        }




        #region //Damage related
        public static void Damage(float _amount)
        {
            Instance._healthSystem.Damage(_amount);
            Instance._damageFX.Play();
            //Debug.Log(_amount);

            DamageTextScript.Create(Instance._sprite.transform.position, 2f, 0.3f, Mathf.FloorToInt(_amount), Color.red);

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