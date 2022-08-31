using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.HealthSystemCM;
using PG.Data;
using PG.HealthSystemCM;

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

        //�� �ΰ��� ���� �Ⱦ���
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

                //���� ������ ���¸� �۵��� ���ߵ��� �����.
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
        private bool _inAction = false;     //�׼� �ϳ� �ϴ� ������
        private bool _inAttack = false;     //���� �ϳ� �ϴ� ������ 

        [SerializeField]
        private List<MobActionData> _mobActionDataList = new List<MobActionData>();

        void SetNextAction()
        {
            _currentActionData = _mobActionDataList[_currentActionOrder];
            _currentAction = _currentActionData._action;

            if (!_inAction)
            {
                //�׼� ù �����̸�
                StartCoroutine(ActionCoroutine(_currentActionData._actionTime));
            }

            switch (_currentAction)
            {
                case MobActionID.Move:

                    transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);

                    if (transform.position.y <= MobGenerator._instance._DamageLine.position.y)
                    {
                        MobGenerator._instance.DestroyMob(gameObject);

                        //������ �Դ� �κ�
                        Debug.Log("damage");
                    }
                    break;

                case MobActionID.Attack:
                   
                    //��ֹ� �ѹ� ��ȯ �� ���� actionTime ������ wait�� �Ȱ��� �۵�        
                    if (_inAttack) { break; }   //�̹� ��ֹ� ��ȯ ��������

                    for(int i = 0; i < _currentActionData._spawnDataList.Count; i++)
                    {
                        ObstacleManager.SetObstacle(_currentActionData._spawnDataList[i],
                                       _currentActionData._placeList[i], _currentActionData._spawnDataList[i]._damageMag);
                    }
                    _inAttack = true;
                    break;

                case MobActionID.Stunned:
                    _isStunned = true;
                    //������ ��� Ǫ�°��� ����� �����ؾ���
                    break;
            }
        }

        IEnumerator ActionCoroutine(float actionTime)
        {
            _inAction = true;
            yield return new WaitForSeconds(actionTime);
            //�׼� ��

            _currentActionOrder++;

            if(_currentActionOrder >= _mobActionDataList.Count)
            {
                //�׼� ������ �켱�� �ı��Ǵ°ɷ�
                MobGenerator._instance.DestroyMob(gameObject);
            }

            _inAction = false;
            _inAttack = false;
        }

    }

}
