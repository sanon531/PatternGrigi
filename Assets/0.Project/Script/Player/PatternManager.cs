using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using PG.Battle.FX;
using PG.Event;
using PG.Data;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace PG.Battle
{
    public class PatternManager : MonoSingleton<PatternManager>
    {
        [SerializeField]
        bool _isDebug = true;

        
        [SerializeField]
        public List<PatternNodeScript> _patternNodes = new List<PatternNodeScript>();
        
        [SerializeField]
        List<int> _defaultNode = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        [SerializeField]
        int _lastNode = 1;
        [SerializeField]
        List<int> _inactivatedNode;
        [SerializeField]
        float _gainEXP_byDebug = 10;
        
        [SerializeField]
        ParticleSystem _signParticle;
        // Start is called before the first frame update
        
        protected override void CallOnAwake()
        {
            _inactivatedNode = _defaultNode.ToList();
            StartChargeEvent();
            StartNodeEvent();
            StartDelayData();
        
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
            CheckDelayData();
        }


        public static void DamageCallWhenNodeReach(int nodeID)
        {
            float length =
                _instance.GetNodePositionByID(_instance._lastNode, nodeID) *
                Global_CampaignData._lengthMagnData.FinalValue;
            float resultDamage = length * Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue;
            _instance.CalcDamageOnSlash(_instance._lastNode,nodeID,resultDamage);
            Global_BattleEventSystem.CallOnCalcPlayerAttack(resultDamage);

            _instance.CheckNodeOnDamage(nodeID);


            LineTracer._instance.SetDrawLineEnd(_instance._patternNodes[nodeID].transform.position);
            VibrationManager.CallVibration();
        }

        void CalcDamageOnSlash(int lastNode, int currentNode,float damage)
        {
            Vector2 lastPos = _patternNodes[lastNode].transform.position;
            Vector2 currentPos = _patternNodes[currentNode].transform.position;
            Vector2 dir = currentPos - lastPos;
            float range = Vector2.Distance(currentPos,lastPos);
            dir = dir.normalized;
            FXCallManager.PlaySlashFX(lastPos,currentPos);
            RaycastHit2D[] hits=new RaycastHit2D[30];
            var count= Physics2D.RaycastNonAlloc(lastPos,dir,hits,range);

            for (int i =0 ; i<count;i++)
            {
                if (hits[i].transform.CompareTag("Enemy"))
                {
                    //��ġ�� ���� ��Ű�� 
                    hits[i].transform.GetComponent<MobScript>().Damage(damage);
                }
            }
        }




        #region nodereach


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
            SetPresetPattern(DrawPatternPresetID.Empty_Breath);
        }



        //������ ���� ��带 �����ϴ� ��Ȳ�ΰ� �ƴѰ��� ���ϴ� �κ�.
        //������ ���� ������ ���� �������� �Ǿ��°��� ���̴� �κ�.
        bool _IsChargeReady = false;
        [SerializeField]
        List<int> _presetNodes = new List<int>();
        [SerializeField]
        int _currentPresetNodeNumber = 0;
        DrawPatternPresetID _currentPattern;


        float[] _weightRandom = new float[3] { 1.0f, 0f, 0f };
        NodePlaceType[] nodePlaceTypes = new NodePlaceType[3] { NodePlaceType.Random, NodePlaceType.Close, NodePlaceType.Far };
        void CheckNodeOnDamage(int nodeID)
        {
            SetGaugeChange();
            _lastNode = nodeID;
            if (_isDebug)
                Global_BattleEventSystem.CallOnGainEXP(_gainEXP_byDebug);

            //���� ���� ���� �ȳ����̸�
            //Debug.Log(_currentPresetNodeNumber + ":" + _presetNodes.Count);
            if (_currentPresetNodeNumber != 0)
                PresetPatternShower.HidePresetPatternByID(_currentPresetNodeNumber - 1);
            _currentPresetNodeNumber++;
            if (_currentPresetNodeNumber < _presetNodes.Count)
            {
                ResetAllNode();
                SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);
                Global_BattleEventSystem.CallOnPatternFilled((float)_currentPresetNodeNumber/_presetNodes.Count);
            }
            else
            {
                //��ų������ �����ϴ� ������ ����.
                //Debug.Log("call by end Pattern"+ _currentPattern);

                if (_IsChargeReady)
                {
                    StartChargeSequence();
                }
                else 
                {
                    Global_BattleEventSystem.CallOnPatternSuccessed(_currentPattern);

                    if (_coolTimeToken > 0) 
                    {
                        SetRandomPattern(_lastNode);
                        _coolTimeToken--;
                    }
                    else
                    {
                        _isPatternDelayed = true;
                    }


                }

                //ShowDebugtextScript.SetDebug("Pattern Success!");
                //�ϴ� ���� ���� ������ �ٷ� ���� ���� �ϵ��� ��
            }
            //ó���� ������ �����Ѵ�.
            //�������� ������ ������ �������� �ٸ� ������ ���°� ���� �� �ߵ��ϵ��� ��
        }

        // ���� ������ �ϴ°� 
        void SetPresetPattern(DrawPatternPresetID drawPattern)
        {
            _currentPresetNodeNumber = 0;
            _currentPattern = drawPattern;
            _presetNodes = GlobalDataStorage.PatternPresetDic[drawPattern].ToList();
            //Debug.Log(_presetNodes.Count);
            PresetPatternShower.SetPresetPatternList(_presetNodes, GlobalDataStorage.PatternWIthLaserDic[drawPattern]);
            //presetDataDic �� ���ο� ��ųʸ��� Ű������EPresetOfDrawPattern�� �޴´�.
            SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);
        }
        void SetRandomPattern(int nodeID)
        {
            int _temptid = nodeID;
            ResetAllNode();
            _currentPresetNodeNumber = 0;
            _presetNodes.Clear();
            _currentPattern = DrawPatternPresetID.Empty_Breath;

            NodePlaceType currentPlace;
            for (int i = 0; i < Global_CampaignData._randomPatternNodeCount.FinalValue; i++)
            {
                currentPlace = nodePlaceTypes.PickRandomWeighted(_weightRandom);
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
                        throw new ArgumentException("No More ProperNode : Pattern No ID");
                        break;
                }
                _presetNodes.Add(_temptid);
            }

            //������ ������ �׳� �������� ���� �κе��� �����.
            PresetPatternShower.SetPresetPatternList(_presetNodes, LaserKindID.Default_laser);
            SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);
        
            //Debug.Log(_presetNodes[_currentPresetNodeNumber]);
        
        }
        
        #region // delayed
        [SerializeField]
        PatternDelayingShowManager _delayingManager;
        bool _isPatternDelayed = false;
        int _coolTimeToken = 0;
        int _maxCoolTimeToken = 0;
        float _currentDelayPercent = 0;
        [FormerlySerializedAs("_increaseAmount")] [SerializeField]
        float TokenIncreaseAmount = 0.2f;
        void StartDelayData() 
        {
            _delayingManager = GameObject.Find("PatternDelayingShowManager").GetComponent<PatternDelayingShowManager>();
            _delayingManager.InitialzeShowManager();
        
            _maxCoolTimeToken = Mathf.RoundToInt(Global_CampaignData._coolTimeTokenCount.FinalValue);
            _coolTimeToken = _maxCoolTimeToken;
            _currentDelayPercent = 0;
            _delayingManager.SetValueofDelay(_currentDelayPercent, _coolTimeToken);
        
        }
        void CheckDelayData() 
        {
            if (_maxCoolTimeToken <= _coolTimeToken)
                return;

            _currentDelayPercent += Time.deltaTime * TokenIncreaseAmount;

            if (_currentDelayPercent > 1) 
            {
                _currentDelayPercent = 0;
        
                if (_isPatternDelayed)
                {
                    SetRandomPattern(_lastNode);
                    _isPatternDelayed = false;
                }
                else 
                {
                    _coolTimeToken++;
                }
            }
            _delayingManager.SetValueofDelay(_currentDelayPercent, _coolTimeToken);
        }
        #endregion


        #region//����
        void SetNodeWeightby(float[] weight)
        {
            _weightRandom[0] += weight[0];
            _weightRandom[1] += weight[1];
            _weightRandom[2] += weight[2];

        }

        //��带 �������� ��ġ�ϴ� �޼ҵ� id�� ��ġ���ʵ��� �ϴ°� 
        void CheckReach(int reachedNode)
        {
            string str = "";
            foreach (var a in _inactivatedNode)
                str += a;
            
            Debug.Log("contain : "+_inactivatedNode.Contains(reachedNode)+"reached : " + reachedNode+"left : " + str);
        }

        public int ReachTriggeredNode_Random(int reachedNode)
        {
            //CheckReach(reachedNode);
            //������ ������ ��ġ�� ���Ұ��� �������Ѵ�.
            if (_inactivatedNode.Contains(reachedNode))
                _inactivatedNode.Remove(reachedNode);
            //else
                //print("No More ProperNode : random when it reached " +reachedNode );
            
            //���� �������� �������� �������Ҷ��� ���ؼ� �������� �Ѵ�.
            int i = _inactivatedNode.Count;
            int _deleteTarget = _inactivatedNode[Random.Range(0, i)];
            //Debug.Log(i + "set" + _deleteTarget);
            //SetNodeToNextReach(_inactivatedNode[_deleteTarget]);
            return _deleteTarget;
        }
        public int ReachTriggeredNode_Close(int reachedNode)
        {
            //CheckReach(reachedNode);
            
            if (_inactivatedNode.Contains(reachedNode))
                _inactivatedNode.Remove(reachedNode);
            

            int[] _targetArr = (int[])_IDWithCloseDic[reachedNode].Clone();
            int _deleteTarget = _targetArr.PickRandom();
            //SetNodeToNextReach(_deleteTarget);
            Global_BattleEventSystem.CallOnNodeSetClose();
            return _deleteTarget;
        }
        public int ReachTriggeredNode_Far(int reachedNode)
        {
            //CheckReach(reachedNode);

            if (_inactivatedNode.Contains(reachedNode))
                _inactivatedNode.Remove(reachedNode);

            int[] _targetArr = (int[])_IDWithFarDic[reachedNode].Clone();
            int _deleteTarget = _targetArr.PickRandom();
            //Debug.Log(reachedNode+" -> :" + _deleteTarget);
            //SetNodeToNextReach(_deleteTarget);
            Global_BattleEventSystem.CallOnNodeSetFar();
            return _deleteTarget;
        }

        #endregion
        //���� ��尡 ������ �ϴ� ���ְ� ���°�. 
        void ResetAllNode()
        {
            _inactivatedNode = _defaultNode.ToList();
            foreach (PatternNodeScript _nodes in _patternNodes)
                _nodes.SetIsReachable(false);

        }

        void SetNodeToNextReachWithCheckIsPlacable(int i)
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
                throw new Exception("Wrong node Error: Already Exist");
        }
        void SetNodeToNextReach(int i)
        {
            //Debug.Log("input " + i );
            _signParticle.Play();
            //Vector3 targetpos = new Vector3((_IDDic[i].x - 1) * 1.75f, (-_IDDic[i].y) * 1.75f, 0);
            Vector3 targetpos = _patternNodes[i].transform.position;
            _signParticle.gameObject.transform.position = targetpos;
            _patternNodes[i].SetIsReachable(true);
            _inactivatedNode.Remove(i);
        }

        #endregion



        #region//charge

        [SerializeField]
        float _maxCharge = 100;
        [SerializeField]
        float _currentCharge = 0;
        [SerializeField]
        float _chargeAmount_forDebug = 25f;
        [SerializeField]
        float _chargeReduction = 10f;

        [SerializeField]
        bool _isChargeStart = false;

        //���۽� , ������ �̺�Ʈ Ż����
        void StartChargeEvent()
        {
            Global_BattleEventSystem._onPatternSuccessed += CallPatternEvent;

        }
        void DeleteChargeEvent()
        {
            Global_BattleEventSystem._onPatternSuccessed -= CallPatternEvent;

        }

        void CheckIsCharge()
        {
            // ���� �Ͻ����� ���¸� �׳� �ѱ�

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
            if (_isChargeStart)
                return;
            if (Global_CampaignData._currentChargePattern == DrawPatternPresetID.Empty_Breath)
                return;
            if (_IsChargeReady) 
                return;

            if (_isDebug)
                _currentCharge += _chargeAmount_forDebug;
            else
                _currentCharge += Global_CampaignData._chargeGaugeData.FinalValue;


            if (_currentCharge >= _maxCharge )
            {
                _IsChargeReady = true;
                //�̷��� ������ �Ǹ� ������ ��� �ÿ� ���� ���η� ������.
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
            SetPresetPattern(Global_CampaignData._currentChargePattern);
            _isChargeStart = true;
            _IsChargeReady = false;
        }

        //������ ���� �Ǿ��� �� ����
        void EndChargeSequence()
        {
            ChargeGaugeUIScript.EndChargeSkill();
            _isChargeStart = false;
        }
        private void CallPatternEvent(DrawPatternPresetID _patternPreset)
        {
            GlobalDataStorage.PatternWIthActionDic[_patternPreset].StartPatternAction();
        }



        #endregion


        #region//private dictionary

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