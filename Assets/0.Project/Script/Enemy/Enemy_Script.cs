using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using PG.Data;
using PG.Event;
using System;
namespace PG.Battle
{
    public class Enemy_Script : MonoBehaviour, IGetHealthSystem, ISetLevelupPause
    {
        private static Enemy_Script _instance;

        [SerializeField]
        private float healthAmountMax, startingHealthAmount, currentHealth;
        bool _isEnemyAlive = true;
        private HealthSystem _healthSystem;
        [SerializeField]
        SpriteRenderer _sprite;
        [SerializeField]
        ParticleSystem _damageFX;
        public PresetDemoItem _vibrationItems;
        Text _gametime, Enemytime;
        private void Awake()
        {
            if (_instance != null&& _instance!=this)
                Debug.LogError("nore than one enemy error");
            _instance = this;
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += HealthSystem_OnDead;

            Global_BattleEventSystem._on�������Ͻ����� += SetLevelUpPauseOn;
            Global_BattleEventSystem._on�������Ͻ��������� += SetLevelUpPauseOff;
        }

        bool _isStatusChangable = true;
        float _actionTime, _maxActionTime;
        //�ڷ�ƾ������ �ð����� �߻��ϴ� ������� ����.
        [SerializeField]
        float _enemyroutineTime;
        private void Update()
        {
            if (_isLevelUpPaused)
                return;

            if (_isStatusChangable && _isEnemyAlive)
            {
                _enemyroutineTime += Time.deltaTime;
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



        #region//Action related

        [SerializeField]
        int _currentActionOrder = 0;
        [SerializeField]
        List<EnemyAction> _enemyActionList = new List<EnemyAction>() { EnemyAction.Wait};
        List<IEnumerator> _routineList= new List<IEnumerator>();
        [SerializeField]
        ActionDataDic _actionDataDic;
        void SetNextAction()
        {
            EnemyAction _tempAction = _enemyActionList[_currentActionOrder];
            ActionData _temptdata = _actionDataDic[_tempAction];
            _maxActionTime = _temptdata._actionTime;
            _actionTime = _maxActionTime;
            CurrentStatusScript.SetTextOnCurrentScript(_tempAction.ToString(), 1f);
            //ShowDebugtextScript.SetDebug(_tempAction.ToString());
            //���⼭ �׼��� ó���� ������ �ǰ� �׼����־��� ����Ʈ�� ���� ���� �ȴٰ� ����.
            //���߿� �ڷ�ƾ���� ĵ���� �ǰ� �� �׷� ������ ������ ������ ������
            if(_tempAction!= EnemyAction.Wait) 
            {
                int i = 0;
                IEnumerator _tempIEnum;
                switch (_temptdata._spawnType)
                {
                    case SpawnType.SetAtOnce_WithSame:
                        foreach (Vector2 v in _temptdata._placeList) 
                        {
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[0], v, 0);
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);
                        }
                        StartCoroutine(RemoveAllRoutine(0));

                        break;
                    case SpawnType.SetGradually_WithSame:
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[0], v, i * _temptdata._placeTimeGradual);
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);
                            i++;
                        }
                        _tempIEnum = RemoveAllRoutine((i-1) * _temptdata._placeTimeGradual);
                        _routineList.Add(_tempIEnum);
                        StartCoroutine(_tempIEnum);

                        break;
                    case SpawnType.SetAtOnce_WithDifferent:
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[i], v, 0);
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);
                            i++;
                        }
                        StartCoroutine(RemoveAllRoutine(0));

                        break;
                    case SpawnType.SetGradually_WithDifferent:
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[i], v, i*_temptdata._placeTimeGradual);
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);

                            i++;
                        }
                        _tempIEnum = RemoveAllRoutine((i-1) * _temptdata._placeTimeGradual);
                        _routineList.Add(_tempIEnum);
                        StartCoroutine(_tempIEnum);

                        break;
                    case SpawnType.SetRandomly:
                        break;
                }

            }

            //Debug.Log("CoroutineList : " + _routineList.Count);

            _currentActionOrder++;
            _currentActionOrder =
                (_currentActionOrder < _enemyActionList.Count) ? _currentActionOrder : 0;
        }

        IEnumerator SetObstacleRoutine(SpawnData data,Vector2 pos,float waitTime) 
        {
            float _deadLine = _enemyroutineTime + waitTime;
            yield return new WaitWhile(() =>  _deadLine > _enemyroutineTime);
            yield return new WaitWhile(() => _deadLine > _enemyroutineTime);

            //Debug.Log("rip : " + (_deadLine > _enemyroutineTime));
            ObstacleManager.SetObstacle(data, pos);

        }
        IEnumerator RemoveAllRoutine(float waitTime)
        {
            float _deadLine = _enemyroutineTime + waitTime;
            //���� ���̰� �´°��� �𸣰���.
            yield return new WaitWhile(() => (_deadLine > _enemyroutineTime));
            while (_deadLine > _enemyroutineTime)
                yield return new WaitWhile(() => (_deadLine > _enemyroutineTime));
            //Debug.Log("Cleaning" + (_deadLine > _enemyroutineTime));
            _routineList.Clear();
        }

        bool _isLevelUpPaused = false;
        public void SetLevelUpPauseOn()
        {
            _isLevelUpPaused = true;
            for (int i = _routineList.Count -1; i>=0; i--) 
            {
                StopCoroutine(_routineList[i]);
                //�� ��ŸƮ �ϸ鼭 ��ü ���� ������ �Ǿ �߻��ϴ� �����ε�.
            }
        }

        public void SetLevelUpPauseOff()
        {
            _isLevelUpPaused = false;
            for (int i = _routineList.Count - 1; i >= 0; i--)
            {
                print("aa"+i +"+"+ _routineList.Count);
                StartCoroutine(_routineList[i]);
            }

        }
        //�׼��� �Ͻ������ؾ��ϴ� ��Ȳ���� Ȱ����.

        #endregion
        #region //Damage related
        public static bool Damage(float _amount)
        {
            _instance._healthSystem.Damage(_amount);
            _instance._damageFX.Play();
            //Debug.Log(_amount);

            DamageTextScript.Create(_instance._sprite.transform.position, 2f, 0.3f, Mathf.FloorToInt(_amount), Color.red);
            return _instance._isEnemyAlive;
        }

        private void HealthSystem_OnDead(object sender, System.EventArgs e)
        {
            CameraShaker.ShakeCamera(10, 1);
            _sprite.DOFade(0, 2);
            _isEnemyAlive = false;
            MMVibrationManager.AdvancedHapticPattern(_vibrationItems.AHAPFile.text,
                                             _vibrationItems.WaveFormAsset.WaveForm.Pattern, _vibrationItems.WaveFormAsset.WaveForm.Amplitudes, -1,
                                             _vibrationItems.RumbleWaveFormAsset.WaveForm.Pattern, _vibrationItems.RumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes,
                                             _vibrationItems.RumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1,
                                             HapticTypes.LightImpact, this, -1, false);

        }

        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }


        #endregion

        public static void SetIsStatusChangeable(bool val)
        {
            _instance._isStatusChangable = val;
        }

       
    }

}