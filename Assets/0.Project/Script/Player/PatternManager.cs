using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PG.Event;
using PG.Data;

namespace PG.Battle
{
    public class PatternManager : MonoSingleton<PatternManager>, ISetNontotalPause
    {
        [SerializeField]
        List<PatternNodeScript> _patternNodes = new List<PatternNodeScript>();

        [SerializeField]
        List<int> _defaultNode = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        List<int> _nodeHistory = new List<int>();

        //1-4�� ���� ���� ������ ������ �ɰ�.
        [SerializeField]
        int _lastNode = 1;
        [SerializeField]
        List<int> _inactivatedNode;
        [SerializeField]
        float _damage = 10;
        [SerializeField]
        float _gainEXP= 10;

        [SerializeField]
        ParticleSystem _signParticle;

        // Start is called before the first frame update
  
        private void Start()
        {
            _inactivatedNode = _defaultNode.ToList();
            Global_BattleEventSystem._onBattleBegin += StartTriggerNode;
            StartChargeEvent();


        }
        // Update is called once per frame

        private void OnDestroy()
        {
            // Update is called once per frame
            Global_BattleEventSystem._onBattleBegin -= StartTriggerNode;
            DeleteChargeEvent();
        }
        private void Update()
        {
            CheckIsCharge();
        }


        //�������� ���� �Ǿ������� ����.(�̺�Ʈ�� �ٲܰ�.)
        public static void DamageCall(int nodeID)
        {
            float _resultDamage = _instance.GetNodePositionByID(_instance._lastNode, nodeID) * _instance._damage;

            //���� �������� ä��� ���� �������� ��á����� �־��� ��尡 ����������. 
            _instance.SetGaugeChange();
            _instance.CheckNodeOnDamage(nodeID);
            //���� ȣ��
            VibrationManager.CallVibration();

            //�׾����� ��� ��� ���� �ʱ�ȭ �Ѵ�.
            if (!Enemy_Script.Damage(_resultDamage)) 
            {
                _instance.ResetAllNode();
                _instance._lastNode = -1;
            }
            LineTracer._instance.SetDrawLineEnd(_instance._patternNodes[nodeID].transform.position);
            Global_BattleEventSystem.CallOnGainEXP(_instance._gainEXP);
        }

        #region//nodereach
        //������ ���۵� �� ����ϴ� �ڵ�
        void StartTriggerNode()
        {
            if (_lastNode != 4)
            {
                _nodeHistory = new List<int>(1) { 4 };
                SetTriggerNodeByID(4);
            }

        }

        //Ȯ������ ���� ��带 �������ִ� �ڵ�
        public void SetTriggerNodeByID(int id)
        {
            SetNodeToNextReach(id);
        }


        //�������� ���������� ���� ��带 �����ϴ� �޼ҵ�
        void CheckNodeOnDamage(int nodeID)
        {
            if (_isRandomNodeSetMode)
            {
                _lastNode = nodeID;
                ReachTriggeredNode_Random(nodeID);
                //������ ������ �׳� �������� ���� �κе��� �����.
            }
            else
            {
                //���� ���� ���� �ȳ����̸�
                if (_currentPresetNodeNumber < _presetNodes.Count) 
                {
                    ResetAllNode();
                    SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);
                    if (_currentPresetNodeNumber != 0)
                        PresetPatternShower.HidePresetPatternByID(_currentPresetNodeNumber - 1);
                    _currentPresetNodeNumber++;
                }
                else
                {
                    //��ų������ �����ϴ� ������ ����.
                    _lastNode = nodeID;
                    _isRandomNodeSetMode = true;
                    ReachTriggeredNode_Random(nodeID);
                    Global_BattleEventSystem.CallOnPatternSuccessed(_currentPattern);
                    ShowDebugtextScript.SetDebug("Pattern Success!");
                    //�ϴ� ���� ���� ������ �ٷ� ���� ���� �ϵ��� ��
                }
                //ó���� ������ �����Ѵ�.
            }
        }


        //������ ���� ��带 �����ϴ� ��Ȳ�ΰ� �ƴѰ�.
        bool _isRandomNodeSetMode = true;
        //���� ������ ��ġ �Ǿ��°�.
        bool _IsCurrentNodeSetted = false;

        [SerializeField]
        List<int> _presetNodes = new List<int>();
        [SerializeField]
        int _currentPresetNodeNumber = 0;
        EDrawPatternPreset _currentPattern;
        // ���� ������ �ϴ°� 
        void SetSkillToPresetNodeFollow(EDrawPatternPreset drawPattern)
        {
            if (!_isRandomNodeSetMode && !_IsCurrentNodeSetted)
            {
                _currentPresetNodeNumber = 0;
                _currentPattern = drawPattern;
                _presetNodes = S_PatternStorage.S_PatternPresetDic[drawPattern];
                PresetPatternShower.ShowPresetPatternAll();
                //presetDataDic �� ���ο� ��ųʸ��� Ű������EPresetOfDrawPattern�� �޴´�.
                _IsCurrentNodeSetted = true;

            }
            else
            {
                Debug.Log("AlreadyAnother" + _isRandomNodeSetMode + _IsCurrentNodeSetted);
            }
        }


        //��带 �������� ��ġ�ϴ� �޼ҵ� id�� ��ġ���ʵ��� �ϴ°� 
        public void ReachTriggeredNode_Random(int reachedNode)
        {
            //Debug.Log("reached : " + _reachedNode);
            ResetAllNode();
            //������ ������ ��ġ�� ���Ұ��� �������Ѵ�.
            _inactivatedNode.Remove(reachedNode);
            //���� �������� �������� �������Ҷ��� ���ؼ� �������� �Ѵ�.
            int i = _inactivatedNode.Count;
            int _deleteTarget = Random.Range(0, i);
            //Debug.Log(i + "set" + _deleteTarget);
            SetNodeToNextReach(_inactivatedNode[_deleteTarget]);

        }

        //���� ��尡 ������ �ϴ� ���ְ� ���°�. 
        void ResetAllNode()
        {
            _inactivatedNode = _defaultNode.ToList();
            foreach (PatternNodeScript _nodes in _patternNodes)
                _nodes.SetIsReachable(false);

        }

        void SetNodeToNextReach(int i)
        {
            //Debug.Log("input " + i );
            if (_inactivatedNode.Contains(i))
            {
                _signParticle.Play();
                Vector3 _targetpos = new Vector3((_IDDic[i].x - 1) * 1.75f, (-_IDDic[i].y) * 1.75f, 0);
                _signParticle.gameObject.transform.position = _targetpos;
                _patternNodes[i].SetIsReachable(true);
                _inactivatedNode.Remove(i);
            }
            else
                Debug.LogError("Wrong node Error: Already Exist");
        }
        #endregion



        #region//charge

        //�Ͻ������� �ڵ��� ������
        bool _isPaused = false;
        [SerializeField]
        float _maxCharge = 100;
        [SerializeField]
        float _currentCharge = 0;
        [SerializeField]
        float _chargeAmount = 25f;
        [SerializeField]
        float _chargeReduction = 10f;

        [SerializeField]
        bool _isChargeStart = false;

        //���۽� , ������ �̺�Ʈ Ż����
        void StartChargeEvent() 
        {
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;
        }
        void DeleteChargeEvent()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;

        }

        void CheckIsCharge() 
        {
            // ���� �Ͻ����� ���¸� �׳� �ѱ�
            if (_isPaused)
                return;

            if (_isChargeStart)
            {
                //ShowDebugtextScript.SetDebug(_currentCharge.ToString());
                //���� �ð��� ����
                if (_currentCharge > 0)
                {
                    _currentCharge -= Time.deltaTime* _chargeReduction;
                    ChargeGaugeUIScript.SetChargeGauge(_currentCharge/ _maxCharge);
                }
                //�ð� �ʰ� �ý��� ó��. ������ 
                else 
                {
                    //�ϴ��� ���� �Ǿ��� ������ ���̳��� ������ �������� ������,
                    //���� �̾�鼭 �� �ϴ°Ŵ� ���߿� �����غ����� ��.
                    Global_BattleEventSystem.CallOnChargeEnd();
                    _currentCharge = 0;
                    EndChargeSequence();
                }
            }

        }
        void SetGaugeChange()
        {
            if (_isChargeStart) return;
            _currentCharge += _chargeAmount;
            if (_maxCharge <= _currentCharge)
            {
                StartChargeSequence();
            }
            ChargeGaugeUIScript.SetChargeGauge(_currentCharge/ _maxCharge);
        }

        //���� �����Ҷ�
        void StartChargeSequence()
        {
            Global_BattleEventSystem.CallOnChargeStart();
            ChargeGaugeUIScript.StartChargeSkill();
            CameraShaker.ShakeCamera(3f, 0.5f);

            _isRandomNodeSetMode = false;
            //�÷��̾�� ������ �޾ƿ´�.
            SetSkillToPresetNodeFollow(Player_Script.GetPlayerStatus()._currentChargePattern);
            _isChargeStart = true;
        }

        //������ ���� �Ǿ��� �� ����
        void EndChargeSequence()
        {
            ChargeGaugeUIScript.EndChargeSkill();
            _isChargeStart = false;
        }


        public void SetNonTotalPauseOn()
        {
            _isPaused = true;
        }

        public void SetNonTotalPauseOff()
        {
            _isPaused = false;
        }


        #endregion


        #region

        Dictionary<int, Vector2Int> _IDDic = new Dictionary<int, Vector2Int>()
        {
            {0,new Vector2Int(0,0) },
            {1,new Vector2Int(1,0) },
            {2,new Vector2Int(2,0) },
            {3,new Vector2Int(0,1) },
            {4,new Vector2Int(1,1) },
            {5,new Vector2Int(2,1) },
            {6,new Vector2Int(0,2) },
            {7,new Vector2Int(1,2) },
            {8,new Vector2Int(2,2) }
        };
        float GetNodePositionByID(int startID, int endID)
        {
            float _xval = Mathf.Pow(_IDDic[startID].x - _IDDic[endID].x, 2);
            float _yval = Mathf.Pow(_IDDic[startID].y - _IDDic[endID].y, 2);
            //Debug.Log(startID + " and " + endID);
            //Debug.Log(_IDDic[startID] + " + "+ _IDDic[endID] + ":"+ _xval +"+" + _yval);
            return Mathf.Sqrt(_xval + _yval);
        }

        #endregion
    }
}