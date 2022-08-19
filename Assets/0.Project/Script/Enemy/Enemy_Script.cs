using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using PG.Data;
using PG.Event;
using System.Linq;
using System;
using Spine.Unity;
namespace PG.Battle
{
    public class Enemy_Script : MonoSingleton<Enemy_Script>, IGetHealthSystem, ISetNontotalPause
    {

        [SerializeField]
        private float healthAmountMax, startingHealthAmount, currentHealth;
        bool _isEnemyAlive = true;
        private HealthSystem _healthSystem;
        [SerializeField]
        SkeletonAnimation _enemyskeleton;
        //SpriteRenderer _sprite;
        [SerializeField]
        List<ParticleSystem> _damageFXLists;

        public PresetDemoItem _vibrationItems;
        Text _gametime, Enemytime;
        private void Start()
        {
            if (_instance != null&& _instance!=this)
                Debug.LogError("nore than one enemy error");
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += HealthSystem_OnDead;

            GameObject.Find("EnemyHealthBarUI").GetComponent<HealthBarUI>().SetHealthSystem(_healthSystem);


            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;
            Global_BattleEventSystem._onChargeStart += SetOnActionStunned;
            Global_BattleEventSystem._onChargeEnd += SetOffActionStunned;

        }

        private void OnDestroy()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
            Global_BattleEventSystem._onChargeStart -= SetOnActionStunned;
            Global_BattleEventSystem._onChargeEnd -= SetOffActionStunned;

        }

        bool _isStatusChangable = true;
        bool _isStunned = false;

        float _actionTime, _maxActionTime;
        //�ڷ�ƾ������ �ð����� �߻��ϴ� ������� ����.
        [SerializeField]
        float _enemyroutineTime;
        private void Update()
        {
            if (_isNontotalPaused)
                return;
            else if (_isStatusChangable && _isEnemyAlive)
            {
                _enemyroutineTime += Time.deltaTime;

                //���� ������ ���¸� �۵��� ���ߵ��� �����.
                if (_isStunned)
                    return;

                if (_actionTime > 0 )
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
        EnemyAction _currentAction;
        [SerializeField]
        List<EnemyAction> _enemyActionList = new List<EnemyAction>() { EnemyAction.Wait};
        List<IEnumerator> _routineList= new List<IEnumerator>();
        [SerializeField]
        ActionDataDic _actionDataDic;
        void SetNextAction()
        {
            _currentAction = _enemyActionList[_currentActionOrder];
            EnemyActionData _temptdata = _actionDataDic[_currentAction];
            _maxActionTime = _temptdata._actionTime;
            _actionTime = _maxActionTime;
            CurrentActionScript.SetTextOnCurrentScript(_currentAction.ToString(), 1f);
            //ShowDebugtextScript.SetDebug(_tempAction.ToString());
            //���⼭ �׼��� ó���� ������ �ǰ� �׼����־��� ����Ʈ�� ���� ���� �ȴٰ� ����.
            //���߿� �ڷ�ƾ���� ĵ���� �ǰ� �� �׷� ������ ������ ������ ������
            if(_currentAction!= EnemyAction.Wait ) 
            {
                int i = 0;
                IEnumerator _tempIEnum;
                switch (_temptdata._spawnType)
                {
                    case SpawnType.SetAtOnce_WithSame:
                        //1�̻��� ���� ������ ���� �߸��Ǿ��ٰ� �˸�.
                        if (_temptdata._spawnDataList.Count > 1)
                            Debug.LogError("Error:ItPlaced more than one");
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[0], v, 0);
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);
                        }
                        StartCoroutine(RemoveAllRoutine(0));
                        break;
                    case SpawnType.SetGradually_WithSame:
                        
                        //1�̻��� ���� ������ ���� �߸��Ǿ��ٰ� �˸�.
                        if (_temptdata._spawnDataList.Count > 1)
                            Debug.LogError("Error:ItPlaced more than one");

                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[0], v, i * _temptdata._placeTimeGradual);
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);
                            i++;
                        }
                        _tempIEnum = RemoveAllRoutine((i - 1) * _temptdata._placeTimeGradual);
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
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[i], v, i * _temptdata._placeTimeGradual);
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);

                            i++;
                        }
                        _tempIEnum = RemoveAllRoutine((i - 1) * _temptdata._placeTimeGradual);
                        _routineList.Add(_tempIEnum);
                        StartCoroutine(_tempIEnum);

                        break;
                    case SpawnType.SetPresettime_WithSame:
                        if (_temptdata._spawnDataList.Count > 1)
                            Debug.LogError("Error:ItPlaced more than one");
                        //��ġ�� �׳� ���� �Է� �ϼ�.
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[0], v, _temptdata._placetimeList.GetRange(0,i).Sum());
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);
                            i++;
                        }
                        _tempIEnum = RemoveAllRoutine(_temptdata._placetimeList.Sum());
                        Debug.Log("Place total time"+_temptdata._placetimeList.Sum());
                        _routineList.Add(_tempIEnum);
                        StartCoroutine(_tempIEnum);
                        break;
                    case SpawnType.SetPresettime_WithDifference:
                        //��ġ�� �׳� ���� �Է� �ϼ�.
                        foreach (Vector2 v in _temptdata._placeList)
                        {
                            _tempIEnum = SetObstacleRoutine(_temptdata._spawnDataList[i], v, _temptdata._placetimeList.GetRange(0, i).Sum());
                            _routineList.Add(_tempIEnum);
                            StartCoroutine(_tempIEnum);
                            i++;
                        }
                        _tempIEnum = RemoveAllRoutine(_temptdata._placetimeList.Sum());
                        Debug.Log("Place total time" + _temptdata._placetimeList.Sum());
                        _routineList.Add(_tempIEnum);
                        StartCoroutine(_tempIEnum);
                        break;
                    case SpawnType.SetRandomly:
                        Debug.LogError("Error: setrandonly is underconstruction");
                        break;
                    default:
                        Debug.LogError("Error: Default is underconstruction");
                        break;
                }

            }

            //Debug.Log("CoroutineList : " + _routineList.Count);
            _currentActionOrder++;
            _currentActionOrder =
                (_currentActionOrder < _enemyActionList.Count) ? _currentActionOrder : 0;

            //�ִϸ��̼� Ʋ��.
            StartAnimationByCurrentAction();
        }

        //�и��� ���� ��������. �����µ��� 
        void SetOnActionStunned() 
        {
            _isStunned = true;
            ObstacleManager.DeleteObstacleOnListAll();
            foreach (IEnumerator routine in _routineList)
                StopCoroutine(routine);
            _routineList.Clear();

        }
        void SetOffActionStunned()
        {
            _isStunned = false;
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
            //Debug.Log("Cleaning" + _routineList.Count);
            _routineList.Clear();
        }

        bool _isNontotalPaused = false;
        public void SetNonTotalPauseOn()
        {
            Debug.Log("SetNonTotalPauseOn");
            _isNontotalPaused = true;
            for (int i = _routineList.Count -1; i>=0; i--) 
            {
                StopCoroutine(_routineList[i]);
                //�� ��ŸƮ �ϸ鼭 ��ü ���� ������ �Ǿ �߻��ϴ� �����ε�.
            }
        }

        public void SetNonTotalPauseOff()
        {
            Debug.Log("SetNonTotalPauseOf");

            _isNontotalPaused = false;
            for (int i = _routineList.Count - 1; i >= 0; i--)
            {
                //print("aa"+i +"+"+ _routineList.Count);
                StartCoroutine(_routineList[i]);
            }

        }
        //�׼��� �Ͻ������ؾ��ϴ� ��Ȳ���� Ȱ����.

        #endregion
        #region //Damage related
        public static bool Damage(float _amount)
        {
            _instance._healthSystem.Damage(_amount);
            _instance.RandomDamageFX();
            //Debug.Log(_amount);
            DamageTextScript.Create(_instance.transform.position, 2f, 0.3f, Mathf.FloorToInt(_amount), Color.red);
            Global_BattleEventSystem.CallOnCalcDamage(_amount);

            return _instance._isEnemyAlive;
        }


        void RandomDamageFX() 
        {
            int _randomIndex = UnityEngine.Random.Range(0, _damageFXLists.Count);
            _damageFXLists[_randomIndex].Play();

        }

        private void HealthSystem_OnDead(object sender, System.EventArgs e)
        {
            CameraShaker.ShakeCamera(10, 1);
            //_sprite.DOFade(0, 2);
            StartAnimation_Dead();
            _isEnemyAlive = false;
            VibrationManager.CallEnemyDieVib();
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


        #region//animation related

        Dictionary<EnemyAction, string> _actionStringDic = new Dictionary<EnemyAction, string>(){ 
            { EnemyAction.BasicAttack_1, "Pattern_1" },
            { EnemyAction.BasicAttack_2, "Pattern_2" }, 
            { EnemyAction.Wait, "Wait" }};

        void StartAnimationByCurrentAction() 
        {
            _enemyskeleton.state.SetAnimation(1, _actionStringDic[_currentAction], _currentAction == EnemyAction.Wait);
        }
        void StartAnimation_Dead()
        {
            _enemyskeleton.state.SetAnimation(1, "Dead", false);
        }


        #endregion

    }

}