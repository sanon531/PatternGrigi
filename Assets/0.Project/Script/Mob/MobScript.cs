using System;
using PG.Data;
using PG.Event;
using PG.HealthSystemCM;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace PG.Battle
{
    public class MobScript : MonoBehaviour
    {
        #region //variables

        [Header("Health")] [SerializeField] private float healthAmountMax;
        [SerializeField] private float startingHealthAmount;
        [SerializeField] private float currentHealth;
        private HealthSystem _healthSystem;

        [Header("Move Stat")] public float _initialSpeed = 1;
        public float _acceleration = 1;
        [SerializeField] private Vector3 _movement;
        [SerializeField] protected Collider2D _collider2D;
        [SerializeField] protected Rigidbody2D _rigidBody2D;
        [SerializeField] protected AudioSource thisAudioSource;
        [SerializeField] protected Animator _animator;


        [Header("Current Status")] CharacterID _charactorID;
        bool _isEnemyAlive = false;
        bool _isStunned = false;
        bool _isNontotalPaused = false;
        float _actionTime, _maxActionTime;
        float _reachedDamage = 20;
        float _lootExp = 1;

        private float _CharDamage = 1;
        private float _loadedSpeed = 0;
        private Color _color;
        private eSubColor _subColor;

        #endregion

        void Start()
        {
            _isRigidBody2DNotNull = _rigidBody2D != null;
        }

        void OnEnable()
        {
        }

        void FixedUpdate()
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

                if (_slowTimer > 0)
                    TickSlow();
            }
        }

        [Header("Action")] [SerializeField] private MobActionID _currentAction;
        private int _currentActionOrder = 0;
        private MobActionData _currentActionData;

        [SerializeField] private List<MobActionID> _mobActionIDList = new List<MobActionID>();
        [SerializeField] MobActionDataDic _mobActionDic = new MobActionDataDic();

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private SpriteRenderer _bodySpriteRenderer;

        private Color _originalColor;
        private bool _isKnockBack = false;

        //몹을 스폰 할때 초기 데이터들을 넣어주는 코드
        public void SetInitializeMobSpawnData(CharacterID mobID, MobSpawnData mobSpawnData, int sortingOrder)
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
            _CharDamage = mobSpawnData._공격력;
            //_color = Color.black;
            _subColor = mobSpawnData._색깔;
            if (!_animator.SafeIsUnityNull())
            {
                _animator.SetTrigger(_subColor.ToString());
            }
            _spriteRenderer.color = _color;
            _originalColor = _color;
            _isEnemyAlive = true;
            _isStunned = false;
            _actionTime = 0;
            _currentActionOrder = 0;
            _lootExp = Global_CampaignData._killGetEXP;

            _isKnockBack = Global_CampaignData._isKnockBack;
            _bodySpriteRenderer = GetComponent<SpriteRenderer>();
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
                case MobActionID.Move: //1회만 호출 되는 곳으로 속력과 이동을 설정해줌. 
                    if (_loadedSpeed != 0) _initialSpeed = _loadedSpeed;
                    //_initialSpeed = _mobActionDic[MobActionID.Move]._speed;
                    break;
                case MobActionID.Attack:
                    foreach (MobAttackData data in _currentActionData._mobAttackDataList)
                    {
                        if (data._mobPosSpawn)
                        {
                            ObstacleManager.SetObstacle(data._spawnData, gameObject.transform.position,
                                Global_CampaignData._charactorAttackDic[_charactorID].FinalValue * _CharDamage);
                        }
                        else
                        {
                            foreach (Vector2 pos in data._spawnPosList)
                            {
                                ObstacleManager.SetObstacle(data._spawnData, pos,
                                    Global_CampaignData._charactorAttackDic[_charactorID].FinalValue * _CharDamage);
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
        #region Movement&Damage
        private Vector3 _towardDirrection = Vector3.up;
        void CalcMovement()
        {
            if (_currentAction != MobActionID.Move)
            {
                _rigidBody2D.MovePosition(transform.position);
                return;
            }
            Vector3 direction = Player_Script.GetPlayerPosition() - transform.position;
            direction.Normalize();
            //_towardDirrection = plau
            if (_isRigidBody2DNotNull)
            {
                //막히지 않을 경우 아래 막힐경우 양옆으로 이동한다.
                _movement = (_initialSpeed / 50) * Time.deltaTime * direction;
                _rigidBody2D.MovePosition((transform.position + _movement*_slowRatio  ));
                //_initialSpeed += _acceleration * Time.deltaTime;
            }

            /*
            if (transform.position.y <= MobGenerator.GetDeadLine())
            {
                StopAllCoroutines();
                MobGenerator.RemoveMob(_charactorID, this);
                Player_Script.Damage(_reachedDamage);
            }
            */

        }
        private Coroutine knockback;
        public void Damage(Vector3 colliderPos,float val)
        {
            if (_isEnemyAlive)
            {
                _healthSystem.Damage(val);
                currentHealth = _healthSystem.GetHealth();
                DamageFXManager.ShowDamage(transform.position, Mathf.Round(val).ToString(), Color.white);
                ShowDebugtextScript.ShowCurrentAccumulateDamage(Mathf.RoundToInt(val));
                //print(gameObject.name + " : " + _healthSystem.GetHealth());
                currentHealth = _healthSystem.GetHealth();
                Global_BattleEventSystem.CallOnMobDamaged(val);
                if (!gameObject.activeSelf)
                    return;

                if(_isKnockBack)
                    StartCoroutine(Knockback(colliderPos,0.5f, Player_Script._instance._knockbackForce));
                thisAudioSource.Play();
                StartCoroutine(HitEffect());
            }
        }
        
        
        public void Damage(Vector3 colliderPos, float val,bool isKnockBack)
        {
            if (_isEnemyAlive)
            {
                _healthSystem.Damage(val);
                currentHealth = _healthSystem.GetHealth();
                DamageFXManager.ShowDamage(transform.position, Mathf.Round(val).ToString(), Color.white);
                ShowDebugtextScript.ShowCurrentAccumulateDamage(Mathf.RoundToInt(val));
                //print(gameObject.name + " : " + _healthSystem.GetHealth());
                currentHealth = _healthSystem.GetHealth();
                Global_BattleEventSystem.CallOnMobDamaged(val);
                if (!gameObject.activeSelf)
                    return;

                if(isKnockBack)
                    StartCoroutine(Knockback(colliderPos,0.5f, Player_Script._instance._knockbackForce));
                thisAudioSource.Play();
                StartCoroutine(HitEffect());
            }
        }

        public void DamageWithNoSound(Vector3 colliderPos,float val)
        {
            if (_isEnemyAlive)
            {
                _healthSystem.Damage(val);
                currentHealth = _healthSystem.GetHealth();
                DamageFXManager.ShowDamage(transform.position, Mathf.Round(val).ToString(), Color.white);
                ShowDebugtextScript.ShowCurrentAccumulateDamage(Mathf.RoundToInt(val));
                //print(gameObject.name + " : " + _healthSystem.GetHealth());
                currentHealth = _healthSystem.GetHealth();
                Global_BattleEventSystem.CallOnMobDamaged(val);
                if (!gameObject.activeSelf)
                    return;

                StartCoroutine(Knockback(colliderPos,0.5f, Player_Script._instance._knockbackForce));
                //thisAudioSource.Play();
                StartCoroutine(HitEffect());
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
            //print(_lootExp);
            Global_BattleEventSystem.CallOnGainEXP(_lootExp);
            FXCallManager.PlayDeadFX(transform.position);
        }

        private float _slowTimer = 0f;
        private float _slowRatio = 1f;
        private bool _isRigidBody2DNotNull;

        public void SetDebuff(EMobDebuff debuff)
        {
            switch (debuff)
            {
                case EMobDebuff.Slow:
                    _slowTimer = Global_CampaignData._slowTime;
                    _slowRatio = Global_CampaignData._slowAmount;
                    Debug.Log(_slowRatio);
                    _spriteRenderer.color = Color.green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(debuff), debuff, null);
            }
        }


        private void TickSlow()
        {
            _slowTimer -= Time.deltaTime;
            if (_slowTimer <= 0)
            {
                _slowRatio = 1;
                _slowTimer = 0;
                _spriteRenderer.color = _originalColor;
            }
        }

        private IEnumerator Knockback(Vector3 colliderPos,float duration, float power)
        {
            _isStunned = true;
            float timer = 0f;

            var knockbackdir = (transform.position - colliderPos).normalized;

            while (timer <= duration)
            {
                timer += Time.deltaTime;

                _rigidBody2D.AddForce(knockbackdir *power);
            }

            _isStunned = false;
            _rigidBody2D.velocity = Vector2.zero;
            yield return null;
        }

        private List<Vector3> _towardsList = new List<Vector3>()
        {
            Vector3.left,Vector3.right
        };
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy_Barricade"))
            {
                _towardDirrection = _towardsList.PickRandom() ;
            }else if (col.CompareTag("Boundary_Side"))
            {
                _towardDirrection.x = -_towardDirrection.x ;
            }
            
            if (col.transform.CompareTag("Player"))
            {
                Player_Script.Damage(_CharDamage);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy_Barricade"))
            {
                _towardDirrection = Vector3.up;
            }
            
        }


        public IEnumerator HitEffect()
        {
            _bodySpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            _bodySpriteRenderer.color = Color.white;
        }

        public Vector3 GetMobPosition()
        {
            return transform.position;
        }


        #endregion
    }
}