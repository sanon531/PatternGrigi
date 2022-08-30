using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.HealthSystemCM;
using PG.Data;

namespace PG.Battle 
{
    public class MobScript : MonoBehaviour
    {

        [Header("Health")]
        [SerializeField]
        private float healthAmountMax;
        [SerializeField]
        private float startingHealthAmount;
        [SerializeField]
        private float currentHealth;
        private HealthSystem _healthSystem;

        [Header("Move Stat")]
        public float _moveSpeed;
        [SerializeField]
        private Vector3 _moveDirection;

        //이 두개는 아직 안쓰임
        bool _isEnemyAlive = true;
        bool _isStunned = false;

        void Start()
        {
            _healthSystem = new HealthSystem(healthAmountMax);
            _healthSystem.SetHealth(startingHealthAmount);

        }

        void Update()
        {
            /*if (_isNontotalPaused)
                return;
            else */if (_isEnemyAlive)
            {

                //만약 스턴인 상태면 작동을 멈추도록 만든다.
                if (_isStunned)
                    return;

                 SetNextAction();

            }
        }

        [Header("Action")]
        [SerializeField]
        private MobActionID _currentAction;
        private int _currentActionOrder = 0;
        private MobActionData _currentActionData;
        private bool _inAction = false;     //액션 하나 하는 중인지
        private bool _inAttack = false;     //공격 하나 하는 중인지 

        [SerializeField]
        private List<MobActionData> _mobActionDataList = new List<MobActionData>();

        void SetNextAction()
        {
            _currentActionData = _mobActionDataList[_currentActionOrder];
            _currentAction = _currentActionData._action;

            if (!_inAction)
            {
                //액션 첫 시작이면
                StartCoroutine(ActionCoroutine(_currentActionData._actionTime));
            }

            switch (_currentAction)
            {
                case MobActionID.Move:

                    transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);

                    if (transform.position.y <= MobGenerator._instance._DamageLine.position.y)
                    {
                        MobGenerator._instance.DestroyMob(gameObject);

                        //데미지 입는 부분
                        Debug.Log("damage");
                    }
                    break;

                case MobActionID.Attack:
                   
                    //장애물 한번 소환 후 남은 actionTime 동안은 wait과 똑같이 작동        
                    if (_inAttack) { break; }   //이미 장애물 소환 끝났으면

                    for(int i = 0; i < _currentActionData._spawnDataList.Count; i++)
                    {
                        ObstacleManager.SetObstacle(_currentActionData._spawnDataList[i],
                                       _currentActionData._placeList[i], _currentActionData._spawnDataList[i]._damageMag);
                    }
                    _inAttack = true;
                    break;

                case MobActionID.Stunned:
                    _isStunned = true;
                    //스턴을 어떻게 푸는건지 물어보고 수정해야함
                    break;
            }
        }

        IEnumerator ActionCoroutine(float actionTime)
        {
            _inAction = true;
            yield return new WaitForSeconds(actionTime);
            //액션 끝

            _currentActionOrder++;

            if(_currentActionOrder >= _mobActionDataList.Count)
            {
                //액션 끝난거 우선은 파괴되는걸로
                MobGenerator._instance.DestroyMob(gameObject);
            }

            _inAction = false;
            _inAttack = false;
        }

    }

}
