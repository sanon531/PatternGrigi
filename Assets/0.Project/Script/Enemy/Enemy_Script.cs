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
            //?????? ?????? ?????? ?????? ???? ???????????? ???????? ???? ???? ?????? ????.
            //?????? ?????????? ?????? ???? ?? ???? ?????? ?????? ?????? ??????
            if(_tempAction!= EnemyAction.Wait) 
            {
                int i = 0;

                switch (_temptdata._spawnType)
                {
                    case SpawnType.SetAtOnce_WithSame:
                        foreach (Vector2 v in _temptdata._placeList)
                            StartCoroutine(SetObstacleRoutine(_temptdata._spawnDataList[0], v, 0));
                        break;
                    case SpawnType.SetGradually_WithSame:
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            StartCoroutine(SetObstacleRoutine(_temptdata._spawnDataList[0], v, i * _temptdata._placeTimeGradual));
                            i++;
                        }
                        break;
                    case SpawnType.SetAtOnce_WithDifferent:
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            StartCoroutine(SetObstacleRoutine(_temptdata._spawnDataList[i], v, 0));
                            i++;
                        }
                        break;
                    case SpawnType.SetGradually_WithDifferent:
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            StartCoroutine(SetObstacleRoutine(_temptdata._spawnDataList[i], v, i*_temptdata._placeTimeGradual));
                            i++;
                        }
                        break;
                    case SpawnType.SetRandomly:
                        break;
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