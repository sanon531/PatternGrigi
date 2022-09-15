using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PG.Event;
using PG.Data;

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


        //1-4로 들어가는 것이 최초의 공격이 될것.
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


        //데미지가 산출 되었을때의 정보
        public static void DamageCallWhenNodeReach(int nodeID)
        {
            //먼저 게이지를 채우고 만약 게이지가 다찼을경우 주어진 노드가 나오도록함. 
            //_instance.SetGaugeChange();
            //일단 차지는 잠시 없애고하자. 그게 정신 건강에 좋다 
            //길이 계산한다음에 초기화 해줘야함 아래 두줄 순서 바꾸지 마셈
            float _length =
                _instance.GetNodePositionByID(_instance._lastNode, nodeID) *
                Global_CampaignData._lengthMagnData.FinalValue;
            _instance.CheckNodeOnDamage(nodeID);
            float _resultDamage = _length * Global_CampaignData._charactorAttackDic[CharacterID.Player].FinalValue;
            Global_BattleEventSystem.CallOnCalcPlayerAttack(_resultDamage);


            //이부분에서 경험치 관련 코드를 변동 해야함.
            //
            if(_instance._isDebug)
                Global_BattleEventSystem.CallOnGainEXP(_instance._gainEXP_byDebug);
            else
                Global_BattleEventSystem.CallOnGainEXP(Global_CampaignData._chargeEXPData.FinalValue);

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
        //전투가 시작될 때 사용하는 코드
        void StartTriggerNode()
        {
            SetPresetPattern(DrawPatternPresetID.Empty_Breath);
        }



        //지금이 랜덤 노드를 선택하는 상황인가 아닌가를 정하는 부분.
        //차지는 현재 패턴이 랜덤 패턴으로 되었는가를 보이는 부분.
        bool _IsChargeReady = false;
        [SerializeField]
        List<int> _presetNodes = new List<int>();
        [SerializeField]
        int _currentPresetNodeNumber = 0;
        DrawPatternPresetID _currentPattern;

        //데미지가 가해졌을때 다음 노드를 결정하는 메소드
        //처음에는 무조건 랜덤만.
        float[] _weightRandom = new float[3] { 1.0f, 0f, 0f };
        NodePlaceType[] nodePlaceTypes = new NodePlaceType[3] { NodePlaceType.Random, NodePlaceType.Close, NodePlaceType.Far };
        void CheckNodeOnDamage(int nodeID)
        {
            SetGaugeChange();
            _lastNode = nodeID;
            //아직 패턴 수가 안끝남이면
            //Debug.Log(_currentPresetNodeNumber + ":" + _presetNodes.Count);
            if (_currentPresetNodeNumber != 0)
                PresetPatternShower.HidePresetPatternByID(_currentPresetNodeNumber - 1);
            _currentPresetNodeNumber++;
            if (_currentPresetNodeNumber < _presetNodes.Count)
            {
                ResetAllNode();
                SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);
            }
            else
            {
                //스킬성공시 랜덤하는 공격이 나감.
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
                //일단 차지 공격 끝나면 바로 패턴 성공 하도록 함
            }
            //처음의 공격은 무시한다.
            //게이지는 데미지 딜링때 꽉찬다음 다른 패턴이 남는게 없을 때 발동하도록 함
        }

        // 패턴 세팅을 하는곳 
        void SetPresetPattern(DrawPatternPresetID drawPattern)
        {
            _currentPresetNodeNumber = 0;
            _currentPattern = drawPattern;
            _presetNodes = GlobalDataStorage.PatternPresetDic[drawPattern].ToList();
            //Debug.Log(_presetNodes.Count);
            PresetPatternShower.SetPresetPatternList(_presetNodes, GlobalDataStorage.PatternWIthLaserDic[drawPattern]);
            PresetPatternShower.ShowPresetPatternAll();
            //presetDataDic 은 새로운 딕셔너리로 키값으로EPresetOfDrawPattern를 받는다.
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
                        Debug.LogError("CheckNodeOnDamage Error: no id");
                        break;
                }
                _presetNodes.Add(_temptid);
            }

            //기존의 노드들을 그냥 랜덤으로 놓는 부분들을 만든다.
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
        float _increaseAmount = 0.2f;
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

            _currentDelayPercent += Time.deltaTime * _increaseAmount;

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


        #region//무게
        void SetNodeWeightby(float[] weight)
        {
            _weightRandom[0] += weight[0];
            _weightRandom[1] += weight[1];
            _weightRandom[2] += weight[2];

        }

        //노드를 랜덤으로 배치하는 메소드 id는 겹치지않도록 하는것 
        public int ReachTriggeredNode_Random(int reachedNode)
        {
            //Debug.Log("reached : " + reachedNode);
            //기존의 도달한 위치는 사용불가로 만들어야한다.
            if (_inactivatedNode.Contains(reachedNode))
                _inactivatedNode.Remove(reachedNode);
            else
                Debug.Log("No More ProperNode : Random");
            //추후 여러개의 도달점을 가져야할때를 위해서 무작위로 한다.
            int i = _inactivatedNode.Count;
            int _deleteTarget = _inactivatedNode[Random.Range(0, i)];
            //Debug.Log(i + "set" + _deleteTarget);
            //SetNodeToNextReach(_inactivatedNode[_deleteTarget]);
            return _deleteTarget;
        }
        public int ReachTriggeredNode_Close(int reachedNode)
        {
            if (_inactivatedNode.Contains(reachedNode))
                _inactivatedNode.Remove(reachedNode);
            else
                Debug.Log("No More ProperNode : Close");

            int[] _targetArr = (int[])_IDWithCloseDic[reachedNode].Clone();
            int _deleteTarget = _targetArr.PickRandom();
            //SetNodeToNextReach(_deleteTarget);
            Global_BattleEventSystem.CallOnNodeSetClose();
            return _deleteTarget;
        }
        public int ReachTriggeredNode_Far(int reachedNode)
        {
            if (_inactivatedNode.Contains(reachedNode))
                _inactivatedNode.Remove(reachedNode);
            else
                Debug.Log("No More ProperNode : Far");

            int[] _targetArr = (int[])_IDWithFarDic[reachedNode].Clone();
            int _deleteTarget = _targetArr.PickRandom();
            //Debug.Log(reachedNode+" -> :" + _deleteTarget);
            //SetNodeToNextReach(_deleteTarget);
            Global_BattleEventSystem.CallOnNodeSetFar();
            return _deleteTarget;
        }

        #endregion
        //현재 노드가 뭐든지 일단 없애고 보는거. 
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
                Debug.LogError("Wrong node Error: Already Exist");
        }
        void SetNodeToNextReach(int i)
        {
            //Debug.Log("input " + i );
            _signParticle.Play();
            Vector3 _targetpos = new Vector3((_IDDic[i].x - 1) * 1.75f, (-_IDDic[i].y) * 1.75f, 0);
            _signParticle.gameObject.transform.position = _targetpos;
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

        //시작시 , 해제시 이벤트 탈부착
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
            // 만약 일시정지 상태면 그냥 넘김

            if (_isChargeStart)
            {
                //ShowDebugtextScript.SetDebug(_currentCharge.ToString());
                //차지 시간은 간다
                if (_currentCharge > 0)
                {
                    _currentCharge -= Time.deltaTime * _chargeReduction;
                    ChargeGaugeUIScript.SetChargeGauge(_currentCharge / _maxCharge);
                }
                //시간 초과 시실패 처리. 이전에 
                else
                {
                    //일단은 뭐가 되었든 패턴이 끝이나면 차지도 끝나도록 설계함,
                    //차지 이어가면서 뭐 하는거는 나중에 생각해보도록 함.
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
                //이렇게 세팅이 되면 데미지 계산 시에 차지 여부로 결정됨.
            }
            ChargeGaugeUIScript.SetChargeGauge(_currentCharge / _maxCharge);
        }

        //차지 시작, 차지는 일종의 별도의시스템이며 있을 수도 없을 수도 있게 하자.
        void StartChargeSequence()
        {
            Global_BattleEventSystem.CallOnChargeStart();
            ChargeGaugeUIScript.StartChargeSkill();
            CameraShaker.ShakeCamera(3f, 0.5f);
            //플레이어에게 패턴을 받아온다.
            SetPresetPattern(Global_CampaignData._currentChargePattern);
            _isChargeStart = true;
            _IsChargeReady = false;
        }

        //차지가 종료 되었을 때 사용됨
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
        //거리 재는 부분
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