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


        //1-4로 들어가는 것이 최초의 공격이 될것.
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


        //데미지가 산출 되었을때의 정보
        public static void DamageCall(int nodeID)
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
        //전투가 시작될 때 사용하는 코드
        void StartTriggerNode()
        {
            SetPresetPattern(DrawPatternPreset.Empty_Breath);
        }



        //지금이 랜덤 노드를 선택하는 상황인가 아닌가.
        //지금 패턴이 설치 되었는가. 처음에는 패턴이 있는채로 시작한다.
        bool _IsPatternSetted = false;
        //차지는 현재 패턴이
        bool _IsChargeReady = false;
        [SerializeField]
        List<int> _presetNodes = new List<int>();
        [SerializeField]
        int _currentPresetNodeNumber = 0;
        DrawPatternPreset _currentPattern;

        //데미지가 가해졌을때 다음 노드를 결정하는 메소드
        //처음에는 무조건 랜덤만.
        float[] _weightRandom = new float[3] { 1.0f, 0f, 0f };
        NodePlaceType[] nodePlaceTypes = new NodePlaceType[3] { NodePlaceType.Random, NodePlaceType.Close, NodePlaceType.Far };
        void CheckNodeOnDamage(int nodeID)
        {
            _lastNode = nodeID;
            if (!_IsPatternSetted)
            {
                //랜덤 패턴을 생성하는 부분 
                if (!_IsChargeReady)
                    SetRandomPattern(nodeID);
                else 
                {
                
                }

            }
            else
            {
                //아직 차지 공격 안끝남이면
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
                    //스킬성공시 랜덤하는 공격이 나감.
                    _IsPatternSetted = false;
                    SetRandomPattern(nodeID);
                    Global_BattleEventSystem.CallOnPatternSuccessed(_currentPattern);
                    //ShowDebugtextScript.SetDebug("Pattern Success!");
                    //일단 차지 공격 끝나면 바로 패턴 성공 하도록 함
                }
                //처음의 공격은 무시한다.
            }
            //게이지는 데미지 딜링때 꽉찬다음 다른 패턴이 남는게 없을 때 발동하도록 함
            //SetGaugeChange();
        }

        // 패턴 세팅을 하는곳 
        void SetPresetPattern(DrawPatternPreset drawPattern)
        {
            _currentPresetNodeNumber = 0;
            _currentPattern = drawPattern;
            _presetNodes = GlobalDataStorage.PatternPresetDic[drawPattern];
            //Debug.Log(_presetNodes.Count);
            PresetPatternShower.SetPresetPatternList(_presetNodes);
            PresetPatternShower.ShowPresetPatternAll();
            //presetDataDic 은 새로운 딕셔너리로 키값으로EPresetOfDrawPattern를 받는다.
            _IsPatternSetted = true;
            SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);
        }
        void SetRandomPattern(int nodeID)
        {
            int _temptid = nodeID;
            ResetAllNode();

            //기존의 노드들을 그냥 랜덤으로 놓는 부분들을 만든다.
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
            Debug.Log("reached : " + reachedNode);
            //기존의 도달한 위치는 사용불가로 만들어야한다.
            _inactivatedNode.Remove(reachedNode);
            //추후 여러개의 도달점을 가져야할때를 위해서 무작위로 한다.
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
        //현재 노드가 뭐든지 일단 없애고 보는거. 
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

        //일시정지용 코드임 ㅇㅅㅇ
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

        //시작시 , 해제시 이벤트 탈부착
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
            // 만약 일시정지 상태면 그냥 넘김
            if (_isPaused)
                return;

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
            if (_isChargeStart) return;
            _currentCharge += _chargeAmount;
            if (_maxCharge <= _currentCharge)
            {
                StartChargeSequence();
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
            SetPresetPattern(Player_Script.GetPlayerStatus()._currentChargePattern);
            _isChargeStart = true;
        }

        //차지가 종료 되었을 때 사용됨
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