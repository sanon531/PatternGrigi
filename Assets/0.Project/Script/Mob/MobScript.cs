using PG.Data;
using PG.Event;
using PG.HealthSystemCM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Battle
{
    public class MobScript : MonoBehaviour
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
        CharacterID _charactorID;
        bool _isEnemyAlive = false;
        bool _isStunned = false;
        bool _isNontotalPaused = false;
        float _actionTime, _maxActionTime;
        float _reachedDamage = 20;
        float _lootExp = 1;

        private float _extraDamage = 1;
        private float _loadedSpeed = 0;
        private Color _color;
        
        #endregion
        void Start()
        {
        }
        void OnEnable()
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
            else
            {
                
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

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        //몹을 스폰 할때 초기 데이터들을 넣어주는 코드
        public void SetInitializeMobSpawnData(CharacterID mobID, MobSpawnData mobSpawnData,int sortingOrder)
        {
            healthAmountMax = mobSpawnData._체력;
            startingHealthAmount = mobSpawnData._체력;
            currentHealth = mobSpawnData._체력;
            _spriteRenderer.sortingOrder = sortingOrder;
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);
            _healthSystem.OnDead += OnDead;

            _charactorID = mobID;
            
            _loadedSpeed = mobSpawnData._속도;
            _extraDamage = mobSpawnData._공격력;
            _color = mobSpawnData._색깔;

            gameObject.GetComponent<SpriteRenderer>().color = _color;

            _isEnemyAlive = true;
            _isStunned = false;
            _actionTime = 0;
            _currentActionOrder = 0;
            _lootExp = Global_CampaignData._killGetEXP;

            //SetTargetted(false);
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
                    foreach (MobAttackData data in _currentActionData._mobAttackDataList)
                    {
                        if (data._mobPosSpawn)
                        {
                            ObstacleManager.SetObstacle(data._spawnData,gameObject.transform.position, 
                                Global_CampaignData._charactorAttackDic[_charactorID].FinalValue * _extraDamage);
                        }
                        else
                        {
                            foreach (Vector2 pos in data._spawnPosList)
                            {
                                ObstacleManager.SetObstacle(data._spawnData,pos,
                                    Global_CampaignData._charactorAttackDic[_charactorID].FinalValue * _extraDamage);
                            }
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
                StopAllCoroutines();
                MobGenerator.RemoveMob(_charactorID, this);
                Player_Script.Damage(_reachedDamage);
            }


        }


        private Coroutine knockback;
        public void Damage(float val)
        {
            if (_isEnemyAlive)
            {
                _healthSystem.Damage(val);
                currentHealth = _healthSystem.GetHealth();
                DamageFXManager.ShowDamage(transform.position, Mathf.Round(val).ToString(),Color.white);
                ShowDebugtextScript.ShowCurrentAccumulateDamage(Mathf.RoundToInt(val));
                //print(gameObject.name + " : " + _healthSystem.GetHealth());
                currentHealth = _healthSystem.GetHealth();
                //if(knockback is null )
                //knockback = StartCoroutine(Knockback(0.5f, Player_Script._instance._knockbackForce));
            }
        }

        //Called on dead
        private void OnDead(object sender, System.EventArgs e)
        {
            StopAllCoroutines();
            _isEnemyAlive = false;
            MobGenerator.RemoveMob(_charactorID, this);
            
            //트위닝 이슈로 삭제함. 즉발 식으로 대체함. 추후 다른 식으로 구현 할것
            //EXPTokenManager.PlaceEXPToken(transform.position, _lootExp);
            Global_BattleEventSystem.CallOnGainEXP(_lootExp);
            FXCallManager.PlayDeadFX(transform.position);
        }


        private IEnumerator Knockback(float duration, float power)
        {
            _isStunned = true;
            float timer = 0f;

            while (timer <= duration)
            {
                timer += Time.deltaTime;
                
                _rigidBody2D.AddForce(new Vector3(0f, power, 0f));
            }
            _isStunned = false;
            yield return null;
        }


        public Vector3 GetMobPosition() 
        {
            return transform.position;
        }



    }

}
