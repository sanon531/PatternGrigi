using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.HealthSystemCM;
using PG.Data;
using PG.Event;

namespace PG.Battle
{
    public class MobScript : PoolableObject
    {
        #region//variables
        [Header("Health")]
        [SerializeField]
        private float healthAmountMax;
        [SerializeField]
        private float startingHealthAmount;
        [SerializeField]
        private float currentHealth;
        private HealthSystem _healthSystem;

        [Header("Move Stat")]
        public float _initialSpeed =1;
        public float _acceleration = 1;
        [SerializeField]
        private Vector3 _movement;
        [SerializeField]
        protected Collider2D _collider2D;
        [SerializeField]
        protected Rigidbody2D _rigidBody2D;

        [Header("Current Status")]
        CharacterID _currentCharactor = CharacterID.Slime;
        bool _isEnemyAlive = true;
        bool _isStunned = false;
        bool _isNontotalPaused = false;
        float _actionTime, _maxActionTime;
        float _reachedDamage = 20;
        [SerializeField]
        int _lootExp = 10;

        private float _extraDamage = 1;
        private float _loadedSpeed = 0;
        private Color _color;

        #endregion
        void Start()
        {
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += OnDead;
        }
        void OnDestroy()
        {
        }
        void Update()
        {
            if (_isNontotalPaused)
                return;
            else if (_isEnemyAlive)
            {
                if (_isStunned)
                    return;
                if (_actionTime > 0)
                {
                    _actionTime -= Time.deltaTime;
                    CalcMovement();
                }
                else
                {
                    SetNextAction();
                }

            }
        }

        [Header("Action")]
        [SerializeField]
        private MobActionID _currentAction;
        private int _currentActionOrder = 0;
        private MobActionData _currentActionData;

        [SerializeField]
        private List<MobActionID> _mobActionIDList = new List<MobActionID>();
        [SerializeField]
        MobActionDataDic _mobActionDic = new MobActionDataDic();

        //몹을 스폰 할때 초기 데이터들을 넣어주는 코드. 같은 몬스터라도 레벨에 따른 유동적인 강화, 이미지 변화를 위해 짜게됨
        public void SetInitializeMobSpawnData(MobSpawnData mobSpawnData)
        {
            healthAmountMax = mobSpawnData._체력;
            startingHealthAmount = mobSpawnData._체력;
            currentHealth = mobSpawnData._체력;

            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);

            _loadedSpeed = mobSpawnData._속도;
            _extraDamage = mobSpawnData._공격력;
            _color = mobSpawnData._색깔;

            _currentActionOrder = 0;
            _isPlaced = true;
        }

        //set은 단 한번 이뤄지며 동시에 계속되는 반복방식은 move 로 바꾼다.
        //이렇게 바꾼 이유는 애니메이션, 파티클 등의 효과는 한번만 선언하면 되고 그게더 성능에 좋기 때문
        void SetNextAction()
        {
            _currentAction = _mobActionIDList[_currentActionOrder];
            _currentActionData = _mobActionDic[_currentAction];
            _maxActionTime = _currentActionData._actionTime;
            _actionTime = _maxActionTime;

            switch (_currentAction)
            {
                case MobActionID.Wait:
                    break;
                case MobActionID.Move://1회만 호출 되는 곳으로 속력과 이동을 설정해줌. 
                    if (_loadedSpeed != 0) _initialSpeed = _loadedSpeed;
                    //_initialSpeed = _mobActionDic[MobActionID.Move]._speed;
                    break;
                case MobActionID.Attack:
                    //만약 위치가 설정이 안된
                    //경우 몹 위치에서 발사함.
                    if (_currentActionData._placeList.Count > 0)
                    {
                        for (int i = 0; i < _currentActionData._spawnDataList.Count; i++)
                        {
                            ObstacleManager.SetObstacle(_currentActionData._spawnDataList[i],_currentActionData._placeList[i],
                                            Global_CampaignData._charactorAttackDic[_currentCharactor].FinalValue * _extraDamage);
                        }
                    }
                    else 
                    {
                        for (int i = 0; i < _currentActionData._spawnDataList.Count; i++)
                        {
                            ObstacleManager.SetObstacle(_currentActionData._spawnDataList[i],gameObject.transform.position, 
                                           Global_CampaignData._charactorAttackDic[_currentCharactor].FinalValue * _extraDamage);
                        }
                    }

                    break;
                case MobActionID.Stunned:
                    break;
                default:
                    Debug.LogError("Mob action not included ");
                    break;
            }

            _currentActionOrder++;
            if (_currentActionOrder >= _mobActionIDList.Count)
            {
                _currentActionOrder = 0;
            }
        }

        void CalcMovement()
        {
            if (_currentAction != MobActionID.Move) 
            {
                _rigidBody2D.MovePosition(transform.position);
                return;
            }


            if (_rigidBody2D != null)
            {
                //무조건 아래로 내려가기 때문.
                _movement = (-1)*(_initialSpeed / 10) * Time.deltaTime * Vector3.up;
                _rigidBody2D.MovePosition(transform.position + _movement);
                //_initialSpeed += _acceleration * Time.deltaTime;
            }

            if (transform.position.y <= MobGenerator.GetDeadLine())
            {
                MobGenerator.DestroyMob(this);
                Player_Script.Damage(_reachedDamage);
            }


        }


        public void Damage(float val)
        {
            _healthSystem.Damage(val);
            DamageFXManager.ShowDamage(transform.position, Mathf.Round(val).ToString(),Color.white);
            StartCoroutine(Knockback(0.5f, Player_Script._instance._knockbackForce));
        }

        //Called on dead
        private void OnDead(object sender, System.EventArgs e)
        {
            _isEnemyAlive = false;
            MobGenerator.DestroyMob(this);
            EXPTokenManager.PlaceEXPToken(transform.position, _lootExp);
        }


        [SerializeField]
        SpriteRenderer _targetSprite;
        public void SetTargetted(bool val) 
        {
            _targetSprite.enabled = val;
        }

        public IEnumerator Knockback(float duration, float power)
        {
            float timer = 0f;
            int writeCall = 0;

            while (timer <= duration)
            {
                timer += Time.deltaTime;
                writeCall += 1;
                _rigidBody2D.AddRelativeForce(new Vector3(0f, power, 0f));
            }
            Debug.Log(writeCall);
            yield return 0;
        }

        #region//paused

        void SetOnNonTotalPaused()
        public Vector3 GetMobPosition() 
        {
            return transform.position;
        }

        #region//paused


        #endregion


    }

}
