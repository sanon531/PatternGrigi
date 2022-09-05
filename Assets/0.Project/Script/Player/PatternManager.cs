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
        public List<PatternNodeScript> _patternNodes = new List<PatternNodeScript>();

        [SerializeField]
        List<int> _defaultNode = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };


        //1-4�� ���� ���� ������ ������ �ɰ�.
        [SerializeField]
        int _lastNode = 1;
        [SerializeField]
        List<int> _inactivatedNode;
        [SerializeField]
        float _gainEXP = 10;

        [SerializeField]
        ParticleSystem _signParticle;

        // Start is called before the first frame update

        protected override void CallOnAwake()
        {
            _inactivatedNode = _defaultNode.ToList();
            StartChargeEvent();
            StartNodeEvent();
        }
        // Update is called once per frame

        protected override void CallOnDestroy()
        {
            // Update is called once per frame
            DeleteChargeEvent();
            DeleteNodeEvent();
        }
        private void Update()
        {
            CheckIsCharge();
        }


        //�������� ���� �Ǿ������� ����
        public static void DamageCall(int nodeID)
        {
            //���� �������� ä��� ���� �������� ��á����� �־��� ��尡 ����������. 
            //_instance.SetGaugeChange();
            //�ϴ� ������ ��� ���ְ�����. �װ� ���� �ǰ��� ���� 
            //���� ����Ѵ����� �ʱ�ȭ ������� �Ʒ� ���� ���� �ٲ��� ����
            float _length =
                _instance.GetNodePositionByID(_instance._lastNode, nodeID) *
                Global_CampaignData._lengthMagnData.FinalValue;
            _instance.CheckNodeOnDamage(nodeID);
            float _resultDamage = _length * Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue;
            Global_BattleEventSystem.CallOnCalcPlayerAttack(_resultDamage);


            //�̺κп��� ����ġ ���� �ڵ带 ���� �ؾ���.
            //
            Global_BattleEventSystem.CallOnGainEXP(_instance._gainEXP);

            LineTracer._instance.SetDrawLineEnd(_instance._patternNodes[nodeID].transform.position);
            VibrationManager.CallVibration();
        }

        #region//nodereach


        void StartNodeEvent()
        {
            Global_BattleEventSystem._onBattleBegin += StartTriggerNode;
            Global_BattleEventSystem._onNodeSetWeight += SetNodeWeightby;
        }
        void DeleteNodeEvent()
        {
            Global_BattleEventSystem._onNodeSetWeight -= SetNodeWeightby;
            Global_BattleEventSystem._onBattleBegin -= StartTriggerNode;
        }
        //������ ���۵� �� ����ϴ� �ڵ�
        void StartTriggerNode()
        {
            SetPresetPattern(DrawPatternPreset.Empty_Breath);
        }



        //������ ���� ��带 �����ϴ� ��Ȳ�ΰ� �ƴѰ�.
        //���� ������ ��ġ �Ǿ��°�. ó������ ������ �ִ�ä�� �����Ѵ�.
        bool _IsPatternSetted = false;
        //������ ���� ������
        bool _IsChargeReady = false;
        [SerializeField]
        List<int> _presetNodes = new List<int>();
        [SerializeField]
        int _currentPresetNodeNumber = 0;
        DrawPatternPreset _currentPattern;

        //�������� ���������� ���� ��带 �����ϴ� �޼ҵ�
        //ó������ ������ ������.
        float[] _weightRandom = new float[3] { 1.0f, 0f, 0f };
        NodePlaceType[] nodePlaceTypes = new NodePlaceType[3] { NodePlaceType.Random, NodePlaceType.Close, NodePlaceType.Far };
        void CheckNodeOnDamage(int nodeID)
        {
            _lastNode = nodeID;
            if (!_IsPatternSetted)
            {
                //���� ������ �����ϴ� �κ� 
                if (!_IsChargeReady)
                    SetRandomPattern(nodeID);
                else 
                {
                
                }

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
                    _IsPatternSetted = false;
                    SetRandomPattern(nodeID);
                    Global_BattleEventSystem.CallOnPatternSuccessed(_currentPattern);
                    //ShowDebugtextScript.SetDebug("Pattern Success!");
                    //�ϴ� ���� ���� ������ �ٷ� ���� ���� �ϵ��� ��
                }
                //ó���� ������ �����Ѵ�.
            }
            //�������� ������ ������ �������� �ٸ� ������ ���°� ���� �� �ߵ��ϵ��� ��
            //SetGaugeChange();
        }

        // ���� ������ �ϴ°� 
        void SetPresetPattern(DrawPatternPreset drawPattern)
        {
            _currentPresetNodeNumber = 0;
            _currentPattern = drawPattern;
            _presetNodes = GlobalDataStorage.PatternPresetDic[drawPattern];
            //Debug.Log(_presetNodes.Count);
            PresetPatternShower.SetPresetPatternList(_presetNodes);
            PresetPatternShower.ShowPresetPatternAll();
            //presetDataDic �� ���ο� ��ųʸ��� Ű������EPresetOfDrawPattern�� �޴´�.
            _IsPatternSetted = true;
            SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);
        }
        void SetRandomPattern(int nodeID)
        {
            int _temptid = nodeID;
            ResetAllNode();

            //������ ������ �׳� �������� ���� �κе��� �����.
            NodePlaceType currentPlace = nodePlaceTypes.PickRandomWeighted(_weightRandom);

            //Debug.Log(currentPlace +"sdfa");
            switch (currentPlace)
            {
                case NodePlaceType.Random:
                    _temptid = ReachTriggeredNode_Random(_temptid);
                    break;
                case NodePlaceType.Close:
                    _temptid = ReachTriggeredNode_Close(_temptid);
                    break;
                case NodePlaceType.Far:
                    _temptid = ReachTriggeredNode_Far(_temptid);
                    break;
                default:
                    Debug.LogError("CheckNodeOnDamage Error: no id");
                    break;
            }

            _IsPatternSetted = true;
            //Debug.Log(_presetNodes[_currentPresetNodeNumber]);
            //SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);

        }

        #region//����
        void SetNodeWeightby(float[] weight)
        {
            _weightRandom[0] += weight[0];
            _weightRandom[1] += weight[1];
            _weightRandom[2] += weight[2];

        }

        //��带 �������� ��ġ�ϴ� �޼ҵ� id�� ��ġ���ʵ��� �ϴ°� 
        public int ReachTriggeredNode_Random(int reachedNode)
        {
            Debug.Log("reached : " + reachedNode);
            //������ ������ ��ġ�� ���Ұ��� �������Ѵ�.
            _inactivatedNode.Remove(reachedNode);
            //���� �������� �������� �������Ҷ��� ���ؼ� �������� �Ѵ�.
            int i = _inactivatedNode.Count;
            int _deleteTarget = Random.Range(0, i);
            //Debug.Log(i + "set" + _deleteTarget);
            SetNodeToNextReach(_inactivatedNode[_deleteTarget]);
            return _deleteTarget;
        }
        public int ReachTriggeredNode_Close(int reachedNode)
        {
            _inactivatedNode.Remove(reachedNode);
            int _deleteTarget = _IDWithCloseDic[reachedNode].PickRandom();
            SetNodeToNextReach(_deleteTarget);
            Global_BattleEventSystem.CallOnNodeSetClose();
            return 0;
        }
        public int ReachTriggeredNode_Far(int reachedNode)
        {
            _inactivatedNode.Remove(reachedNode);
            int _deleteTarget = _IDWithFarDic[reachedNode].PickRandom();
            //Debug.Log(reachedNode+" -> :" + _deleteTarget);
            SetNodeToNextReach(_deleteTarget);
            Global_BattleEventSystem.CallOnNodeSetFar();
            return 0;
        }

        #endregion
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
            Global_BattleEventSystem._onPatternSuccessed += CallPatternEvent;

        }
        void DeleteChargeEvent()
        {
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;
            Global_BattleEventSystem._onPatternSuccessed -= CallPatternEvent;

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
                    _currentCharge -= Time.deltaTime * _chargeReduction;
                    ChargeGaugeUIScript.SetChargeGauge(_currentCharge / _maxCharge);
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
            ChargeGaugeUIScript.SetChargeGauge(_currentCharge / _maxCharge);
        }

        //���� ����, ������ ������ �����ǽý����̸� ���� ���� ���� ���� �ְ� ����.
        void StartChargeSequence()
        {
            Global_BattleEventSystem.CallOnChargeStart();
            ChargeGaugeUIScript.StartChargeSkill();
            CameraShaker.ShakeCamera(3f, 0.5f);
            //�÷��̾�� ������ �޾ƿ´�.
            SetPresetPattern(Player_Script.GetPlayerStatus()._currentChargePattern);
            _isChargeStart = true;
        }

        //������ ���� �Ǿ��� �� ����
        void EndChargeSequence()
        {
            ChargeGaugeUIScript.EndChargeSkill();
            _isChargeStart = false;
        }
        private void CallPatternEvent(DrawPatternPreset _patternPreset)
        {
            GlobalDataStorage.PatternWIthActionDic[_patternPreset].StartPatternAction();
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
        Dictionary<int, int[]> _IDWithCloseDic = new Dictionary<int, int[]>()
        {
            {0,new int[2]{1,3} }, {1,new int[3]{0,2,4} },{2,new int[2]{1,5} },
            {3,new int[3]{0,4,6} },{4,new int[4]{1,3,5,7} },{5,new int[3]{2,4,8} },
            {6,new int[2]{4,7} },{7,new int[3]{4,6,8} },{8,new int[2]{5,7} }
        };

        Dictionary<int, int[]> _IDWithFarDic = new Dictionary<int, int[]>()
        {
            {0,new int[3]{5,7,8} }, {1,new int[3]{6,7,8} },{2,new int[3]{3,6,7} },
            {3,new int[3]{2,5,8} },{4,new int[4]{0,2,6,7} },{5,new int[3]{0,3,6} },
            {6,new int[3]{1,2,5} },{7,new int[3]{0,1,2} },{8,new int[3]{0,1,3} }
        };
        //�Ÿ� ��� �κ�
        float GetNodePositionByID(int startID, int endID)
        {
            float _xval = Mathf.Pow(_IDDic[startID].x - _IDDic[endID].x, 2);
            float _yval = Mathf.Pow(_IDDic[startID].y - _IDDic[endID].y, 2);
            //Debug.Log(startID + " and " + endID);
            //Debug.Log(_IDDic[startID] + " + "+ _IDDic[endID] + ":"+ _xval +"+" + _yval);
            if (_xval == 0f && _yval == 0f)
                return 1;
            else
                return Mathf.Sqrt(_xval + _yval);
        }

        #endregion
    }
}